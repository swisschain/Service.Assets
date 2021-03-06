﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Services;
using Assets.WebApi.Models.Assets;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swisschain.Sdk.Server.Authorization;
using Swisschain.Sdk.Server.WebApi.Common;
using Swisschain.Sdk.Server.WebApi.Pagination;

namespace Assets.WebApi
{
    [Authorize]
    [ApiController]
    [Route("api/assets")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsService _assetsService;
        private readonly IMapper _mapper;

        public AssetsController(IAssetsService assetsService, IMapper mapper)
        {
            _assetsService = assetsService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Paginated<Asset, long>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetManyAsync([FromQuery] AssetRequestMany request)
        {
            var sortOrder = request.Order == PaginationOrder.Asc
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;

            var brokerId = User.GetTenantId();

            var assets = await _assetsService.GetAllAsync(brokerId, request.Symbol, request.IsDisabled, sortOrder, request.Cursor, request.Limit);

            var result = _mapper.Map<List<Asset>>(assets);

            return Ok(result.Paginate(request, Url, x => x.Symbol));
        }

        [HttpGet("{symbol}")]
        [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return NotFound();

            var brokerId = User.GetTenantId();

            var asset = await _assetsService.GetBySymbolAsync(brokerId, symbol);

            if (asset == null)
                return NotFound();

            var model = _mapper.Map<Asset>(asset);

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] AssetEdit model)
        {
            var brokerId = User.GetTenantId();

            Domain.Entities.Asset asset;

            try
            {
                asset = await _assetsService.AddAsync(brokerId, model.Symbol, model.Description, model.Accuracy, model.IsDisabled);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError($"{nameof(model.Symbol)}", e.Message);

                return BadRequest(ModelState);
            }

            var newModel = _mapper.Map<Asset>(asset);

            return Ok(newModel);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetEdit model)
        {
            var brokerId = User.GetTenantId();

            Domain.Entities.Asset asset;

            try
            {
                asset = await _assetsService.UpdateAsync(brokerId, model.Symbol, model.Description, model.Accuracy, model.IsDisabled);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError($"{nameof(model.Symbol)}", e.Message);

                return BadRequest(ModelState);
            }

            if (asset == null)
                return NotFound();

            var updatedModel = await _assetsService.GetBySymbolAsync(brokerId, model.Symbol);

            var newModel = _mapper.Map<Asset>(updatedModel);

            return Ok(newModel);
        }

        [HttpDelete("{symbol}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return NotFound();

            var brokerId = User.GetTenantId();

            try
            {
                var isFound = await _assetsService.DeleteAsync(brokerId, symbol);

                if (!isFound)
                    return NotFound();
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError($"{nameof(symbol)}", e.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
