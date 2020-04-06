using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Domain.Services;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Service.Assets.Contracts;

namespace Assets.GrpcServices
{
    public class AssetsService : Service.Assets.Contracts.Assets.AssetsBase
    {
        private readonly IAssetsService _assetsService;
        private readonly IMapper _mapper;

        public AssetsService(IAssetsService assetsService, IMapper mapper)
        {
            _assetsService = assetsService;
            _mapper = mapper;
        }

        public override async Task<GetAllAssetsResponse> GetAll(Empty request, ServerCallContext context)
        {
            var assets = await _assetsService.GetAllAsync();

            var response = new GetAllAssetsResponse();

            response.Assets.AddRange(_mapper.Map<List<Asset>>(assets));

            return response;
        }

        public override async Task<GetAssetByIdResponse> GetById(GetAssetByIdRequest request, ServerCallContext context)
        {
            var asset = await _assetsService.GetByIdAsync(request.AssetId);

            return new GetAssetByIdResponse {Asset = _mapper.Map<Asset>(asset)};
        }

        public override async Task<AddAssetResponse> Add(AddAssetRequest request, ServerCallContext context)
        {
            var asset = await _assetsService.AddAsync(request.Id, request.Name, request.Description, request.Accuracy,
                request.IsDisabled);

            return new AddAssetResponse {Asset = _mapper.Map<Asset>(asset)};
        }

        public override async Task<Empty> Update(UpdateAssetRequest request, ServerCallContext context)
        {
            await _assetsService.UpdateAsync(request.Id, request.Name, request.Description, request.Accuracy,
                request.IsDisabled);

            return new Empty();
        }

        public override async Task<Empty> Delete(DeleteAssetRequest request, ServerCallContext context)
        {
            await _assetsService.DeleteAsync(request.AssetId);

            return new Empty();
        }
    }
}
