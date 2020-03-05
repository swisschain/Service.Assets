using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Domain.Services;
using Assets.WebApi.Models.Assets;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Assets.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetAllAsync()
        {
            var assets = await _assetsService.GetAllAsync();

            var model = _mapper.Map<List<AssetModel>>(assets);

            return Ok(model);
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
            var asset = await _assetsService.AddAsync(model.Id, model.Name, model.Description, model.Accuracy);

            var newModel = _mapper.Map<AssetModel>(asset);

            return Ok(newModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetEditModel model)
        {
            await _assetsService.UpdateAsync(model.Id, model.Name, model.Description, model.Accuracy);

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
