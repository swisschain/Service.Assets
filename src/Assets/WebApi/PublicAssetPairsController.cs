﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Client.Models;
using Assets.Client.Models.AssetPairs;
using Assets.Domain.Services;
using Assets.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Assets.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicAssetPairsController : ControllerBase
    {
        private readonly IAssetPairsService _assetPairsService;
        private readonly IMapper _mapper;

        public PublicAssetPairsController(IAssetPairsService assetPairsService, IMapper mapper)
        {
            _assetPairsService = assetPairsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string filter, string sortField, string sortOrder, int pageIndex,
            int pageSize)
        {
            var assetPairs = await _assetPairsService.GetAllAsync();

            var query = assetPairs.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
                query = query.Where(assetPair =>
                    assetPair.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(sortField))
                query = query.Order(sortField, sortOrder);

            var count = query.Count();

            query = query
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            var model = _mapper.Map<List<AssetPairModel>>(query.ToList());

            return Ok(new PagedResponse<AssetPairModel> { Items = model, Total = count });
        }

        [HttpGet("{assetPairId}")]
        public async Task<IActionResult> GetByIdAsync(string assetPairId)
        {
            var assetPair = await _assetPairsService.GetByIdAsync(assetPairId);

            var model = _mapper.Map<AssetPairModel>(assetPair);

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AssetPairEditModel model)
        {
            model.Id = Guid.NewGuid().ToString();
            var asset = await _assetPairsService.AddAsync(model.Id, model.Name, model.BaseAssetId,
                model.QuotingAssetId, model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                model.MarketOrderPriceThreshold, model.IsDisabled);

            var newModel = _mapper.Map<AssetPairModel>(asset);

            return Ok(newModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetPairEditModel model)
        {
            await _assetPairsService.UpdateAsync(model.Id, model.Name, model.BaseAssetId,
                model.QuotingAssetId, model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                model.MarketOrderPriceThreshold, model.IsDisabled);

            return NoContent();
        }

        [HttpDelete("{assetPairId}")]
        public async Task<IActionResult> DeleteAsync(string assetPairId)
        {
            await _assetPairsService.DeleteAsync(assetPairId);

            return NoContent();
        }
    }
}