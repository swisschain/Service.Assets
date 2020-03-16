using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Client.Models.AssetPairs;
using Assets.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Assets.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetAllAsync()
        {
            var assetPairs = await _assetPairsService.GetAllAsync();

            var model = _mapper.Map<List<AssetPairModel>>(assetPairs);

            return Ok(model);
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
            var asset = await _assetPairsService.AddAsync(model.Id, model.Name, model.BaseAssetId, model.QuotingAssetId,
                model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                model.MarketOrderPriceThreshold);

            var newModel = _mapper.Map<AssetPairModel>(asset);

            return Ok(newModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetPairEditModel model)
        {
            await _assetPairsService.UpdateAsync(model.Id, model.Name, model.BaseAssetId, model.QuotingAssetId,
                model.Accuracy, model.MinVolume, model.MaxVolume, model.MaxOppositeVolume,
                model.MarketOrderPriceThreshold);

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
