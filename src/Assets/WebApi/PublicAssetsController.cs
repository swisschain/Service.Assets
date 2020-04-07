using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Assets.Domain.Services;
using Assets.WebApi.Models.Assets;
using Assets.WebApi.Models.Common;
using Assets.WebApi.Models.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swisschain.Sdk.Server.Authorization;

namespace Assets.WebApi
{
    [Authorize]
    [ApiController]
    [Route("api/public-assets")]
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
        [ProducesResponseType(typeof(AssetRequestMany), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionaryErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetManyAsync([FromQuery] AssetRequestMany request)
        {
            if (request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should not be more than 1000");

                return BadRequest(ModelState);
            }

            var sortOrder = request.Order == PaginationOrder.Asc
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;

            var assets = await _assetsService.GetAllAsync(request.Name, request.AssetId, request.IsDisabled, sortOrder, request.Cursor, request.Limit);

            var result = _mapper.Map<List<Asset>>(assets);

            return Ok(result.Paginate(request, Url, x => x.Id));
        }

        [HttpGet("{assetId}")]
        [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(string assetId)
        {
            var asset = await _assetsService.GetByIdAsync(assetId);

            if (asset == null)
                return NotFound();

            var model = _mapper.Map<Asset>(asset);

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAsync([FromBody] AssetEdit model)
        {
            model.Id = Guid.NewGuid().ToString();

            var brokerId = User.GetTenantId();

            var asset = await _assetsService.AddAsync(model.Id, brokerId, model.Name, model.Description, model.Accuracy, model.IsDisabled);

            var newModel = _mapper.Map<Asset>(asset);

            return Ok(newModel);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetEdit model)
        {
            var found = await _assetsService.UpdateAsync(model.Id, model.Name, model.Description, model.Accuracy, model.IsDisabled);

            if (!found)
                return NotFound();

            var updatedModel = _assetsService.GetByIdAsync(model.Id);

            var newModel = _mapper.Map<Asset>(updatedModel);

            return Ok(newModel);
        }

        [HttpDelete("{assetId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(string assetId)
        {
            var found = await _assetsService.DeleteAsync(assetId);

            if (!found)
                return NotFound();

            return Ok();
        }
    }
}
