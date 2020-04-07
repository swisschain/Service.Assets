using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Services;
using Assets.WebApi.Models.AssetPairs;
using Assets.WebApi.Models.Common;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swisschain.Sdk.Server.Authorization;
using Swisschain.Sdk.Server.WebApi.Pagination;

namespace Assets.WebApi
{
    [Authorize]
    [ApiController]
    [Route("api/asset-pairs")]
    public class AssetPairsController : ControllerBase
    {
        private readonly IAssetPairsService _assetPairsService;
        private readonly IMapper _mapper;

        public AssetPairsController(IAssetPairsService assetPairsService, IMapper mapper)
        {
            _assetPairsService = assetPairsService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AssetPairRequestMany), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetManyAsync([FromQuery] AssetPairRequestMany request)
        {
            if (request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should not be more than 1000");

                return BadRequest(ModelState);
            }

            var sortOrder = request.Order == PaginationOrder.Asc
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;

            var assetPairs = await _assetPairsService.GetAllAsync(request.Name, request.AssetPairId, request.BaseAssetId, request.QuoteAssetId,
                request.IsDisabled, sortOrder, request.Cursor, request.Limit);

            var result = _mapper.Map<List<AssetPair>>(assetPairs);

            return Ok(result.Paginate(request, Url, x => x.Id));
        }

        [HttpGet("{assetPairId}")]
        [ProducesResponseType(typeof(AssetPair), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(string assetPairId)
        {
            var assetPair = await _assetPairsService.GetByIdAsync(assetPairId);

            if (assetPair == null)
                return NotFound();

            var model = _mapper.Map<AssetPair>(assetPair);

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AssetPair), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAsync([FromBody] AssetPairEdit model)
        {
            model.Id = Guid.NewGuid().ToString();

            var brokerId = User.GetTenantId();

            var asset = await _assetPairsService.AddAsync(model.Id, brokerId, model.Name, model.BaseAssetId,
                model.QuotingAssetId, model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                model.MarketOrderPriceThreshold, model.IsDisabled);

            var newModel = _mapper.Map<AssetPair>(asset);

            return Ok(newModel);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AssetPair), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetPairEdit model)
        {
            var found = await _assetPairsService.UpdateAsync(model.Id, model.Name, model.BaseAssetId,
                model.QuotingAssetId, model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                model.MarketOrderPriceThreshold, model.IsDisabled);

            if (!found)
                return NotFound();

            var updatedModel = await _assetPairsService.GetByIdAsync(model.Id);

            var newModel = _mapper.Map<AssetPair>(updatedModel);

            return Ok(newModel);
        }

        [HttpDelete("{assetPairId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(string assetPairId)
        {
            var found = await _assetPairsService.DeleteAsync(assetPairId);

            if (!found)
                return NotFound();

            return Ok();
        }
    }
}
