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
        [ProducesResponseType(typeof(Paginated<AssetPair, string>), StatusCodes.Status200OK)]
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

            var brokerId = User.GetTenantId();

            var assetPairs = await _assetPairsService.GetAllAsync(brokerId, request.Symbol,
                request.IsDisabled, sortOrder, request.Cursor, request.Limit);

            var result = _mapper.Map<List<AssetPair>>(assetPairs);

            return Ok(result.Paginate(request, Url, x => x.Symbol));
        }

        [HttpGet("{symbol}")]
        [ProducesResponseType(typeof(AssetPair), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySymbolAsync(string symbol)
        {
            var brokerId = User.GetTenantId();

            var assetPair = await _assetPairsService.GetBySymbolAsync(brokerId, symbol);

            if (assetPair == null)
                return NotFound();

            var model = _mapper.Map<AssetPair>(assetPair);

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AssetPair), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAsync([FromBody] AssetPairAdd model)
        {
            var brokerId = User.GetTenantId();

            Domain.Entities.AssetPair assetPair;

            try
            {
                assetPair = await _assetPairsService.AddAsync(brokerId, model.Symbol, model.BaseAsset,
                    model.QuotingAsset, model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                    model.MarketOrderPriceThreshold, model.IsDisabled);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError($"{nameof(model.Symbol)}", e.Message);

                return BadRequest(ModelState);
            }

            var newModel = _mapper.Map<AssetPair>(assetPair);

            return Ok(newModel);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AssetPair), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetPairUpdate model)
        {
            var brokerId = User.GetTenantId();

            Domain.Entities.AssetPair assetPair;

            try
            {
                assetPair = await _assetPairsService.UpdateAsync(brokerId, model.Symbol,
                    model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                    model.MarketOrderPriceThreshold, model.IsDisabled);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError($"{nameof(model.Symbol)}", e.Message);

                return BadRequest(ModelState);
            }

            if (assetPair == null)
                return NotFound();

            var updatedModel = await _assetPairsService.GetBySymbolAsync(brokerId, model.Symbol);

            var newModel = _mapper.Map<AssetPair>(updatedModel);

            return Ok(newModel);
        }

        [HttpDelete("{symbol}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(string symbol)
        {
            var brokerId = User.GetTenantId();

            try
            {
                var isFound = await _assetPairsService.DeleteAsync(brokerId, symbol);

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
