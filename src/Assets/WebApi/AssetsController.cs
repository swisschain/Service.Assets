using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Services;
using Assets.WebApi.Models.Assets;
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
            if (request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should not be more than 1000");

                return BadRequest(ModelState);
            }

            var sortOrder = request.Order == PaginationOrder.Asc
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;

            var brokerId = User.GetTenantId();

            var assets = await _assetsService.GetAllAsync(brokerId, request.Symbol, request.IsDisabled, sortOrder, request.Cursor, request.Limit);

            var result = _mapper.Map<List<Asset>>(assets);

            return Ok(result.Paginate(request, Url, x => x.Id));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var brokerId = User.GetTenantId();

            var asset = await _assetsService.GetByIdAsync(id, brokerId);

            if (asset == null)
                return NotFound();

            var model = _mapper.Map<Asset>(asset);

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
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
                ModelState.AddModelError($"{nameof(model.Id)}", e.Message);

                return BadRequest(ModelState);
            }

            var newModel = _mapper.Map<Asset>(asset);

            return Ok(newModel);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Asset), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetEdit model)
        {
            var brokerId = User.GetTenantId();

            Domain.Entities.Asset asset;

            try
            {
                asset = await _assetsService.UpdateAsync(model.Id, brokerId, model.Symbol, model.Description, model.Accuracy, model.IsDisabled);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError($"{nameof(model.Id)}", e.Message);

                return BadRequest(ModelState);
            }

            if (asset == null)
                return NotFound();

            var updatedModel = _assetsService.GetByIdAsync(model.Id, brokerId);

            var newModel = _mapper.Map<Asset>(updatedModel);

            return Ok(newModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var brokerId = User.GetTenantId();

            try
            {
                await _assetsService.DeleteAsync(id, brokerId);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError($"{nameof(id)}", e.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
