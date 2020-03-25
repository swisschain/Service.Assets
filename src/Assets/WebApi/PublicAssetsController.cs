using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Client.Models;
using Assets.Client.Models.Assets;
using Assets.Domain.Services;
using Assets.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assets.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicAssetsController : ControllerBase
    {
        private readonly IAssetsService _assetsService;
        private readonly IMapper _mapper;

        public PublicAssetsController(IAssetsService assetsService, IMapper mapper)
        {
            _assetsService = assetsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string filter, string sortField, string sortOrder, int pageIndex,
            int pageSize)
        {
            var assets = await _assetsService.GetAllAsync();

            var query = assets.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
                query = query.Where(asset => asset.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(sortField))
                query = query.Order(sortField, sortOrder);

            var count = query.Count();

            query = query
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            var model = _mapper.Map<List<AssetModel>>(query.ToList());

            return Ok(new PagedResponse<AssetModel> { Items = model, Total = count });
        }

        [HttpGet("{assetId}")]
        public async Task<IActionResult> GetByIdAsync(string assetId)
        {
            var asset = await _assetsService.GetByIdAsync(assetId);

            var model = _mapper.Map<AssetModel>(asset);

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AssetEditModel model)
        {
            model.Id = Guid.NewGuid().ToString();

            var asset = await _assetsService.AddAsync(model.Id, model.Name, model.Description, model.Accuracy, model.IsDisabled);

            var newModel = _mapper.Map<AssetModel>(asset);

            return Ok(newModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetEditModel model)
        {
            await _assetsService.UpdateAsync(model.Id, model.Name, model.Description, model.Accuracy, model.IsDisabled);

            return NoContent();
        }

        [HttpDelete("{assetId}")]
        public async Task<IActionResult> DeleteAsync(string assetId)
        {
            await _assetsService.DeleteAsync(assetId);

            return NoContent();
        }
    }
}
