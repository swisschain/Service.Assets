using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Domain.Services;
using Assets.WebApi.Models.AssetPairs;
using Assets.WebApi.Models.Common;
using Assets.WebApi.Models.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assets.WebApi
{
    [Authorize]
    [ApiController]
    [Route("api/public-asset-pairs")]
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
        [ProducesResponseType(typeof(AssetPairRequestMany), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetManyAsync([FromQuery] AssetPairRequestMany assetPairRequestMany)
        {
            if (assetPairRequestMany.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(assetPairRequestMany.Limit)}", "Should not be more than 1000");

                return BadRequest(ModelState);
            }

            var take = assetPairRequestMany.Limit;
            var cursor = assetPairRequestMany.Cursor;
            var sortOrder = assetPairRequestMany.Order == PaginationOrder.Asc;
            var idFilter = assetPairRequestMany.AssetPairId;
            var nameFilter = assetPairRequestMany.Name;

            var assets = await _assetPairsService.GetAllAsync();

            var query = assets.AsQueryable();

            if (!string.IsNullOrEmpty(idFilter))
                query = query.Where(asset => asset.Id.Contains(idFilter, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(nameFilter))
                query = query.Where(asset => asset.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

            if (sortOrder)
            {
                if (cursor != null)
                    query = query.Where(x => String.Compare(x.Id, cursor, StringComparison.CurrentCultureIgnoreCase) >= 0);

                query = query
                    .OrderBy(x => x.Id);
            }
            else
            {
                if (cursor != null)
                    query = query.Where(x => String.Compare(x.Id, cursor, StringComparison.CurrentCultureIgnoreCase) < 0);

                query = query.OrderByDescending(x => x.Id);
            }

            query = query.Take(take);

            var model = _mapper.Map<List<AssetPair>>(query.ToList());

            return Ok(model.Paginate(assetPairRequestMany, Url, x => x.Id));
        }

        [HttpGet("{assetPairId}")]
        [ProducesResponseType(typeof(AssetPair), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] AssetPairEdit model)
        {
            model.Id = Guid.NewGuid().ToString();
            var asset = await _assetPairsService.AddAsync(model.Id, model.Name, model.BaseAssetId,
                model.QuotingAssetId, model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                model.MarketOrderPriceThreshold, model.IsDisabled);

            var newModel = _mapper.Map<AssetPair>(asset);

            return Ok(newModel);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetPairEdit model)
        {
            var found = await _assetPairsService.UpdateAsync(model.Id, model.Name, model.BaseAssetId,
                model.QuotingAssetId, model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                model.MarketOrderPriceThreshold, model.IsDisabled);

            if (!found)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{assetPairId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(string assetPairId)
        {
            var found = await _assetPairsService.DeleteAsync(assetPairId);

            if (!found)
                return NotFound();

            return NoContent();
        }
    }
}
