using Application.DTOs;
using Application.Interfaces;
using System.Threading.Tasks;

namespace Application.Service
{
    public class SkillService : ISkillService
    {
        private readonly IQueryHandler<GetAllSkillsRequest, GetAllSkillsResponse> _getAllQueryHandler;
        private readonly IQueryHandler<GetSkillsByCategoryRequest, GetAllSkillsResponse> _getByCategoryQueryHandler;
        private readonly ICommandHandler<CreateSkillRequest, CreateSkillResponse> _createCommandHandler;

        public SkillService(
            IQueryHandler<GetAllSkillsRequest, GetAllSkillsResponse> getAllQueryHandler,
            IQueryHandler<GetSkillsByCategoryRequest, GetAllSkillsResponse> getByCategoryQueryHandler,
            ICommandHandler<CreateSkillRequest, CreateSkillResponse> createCommandHandler)
        {
            _getAllQueryHandler = getAllQueryHandler;
            _getByCategoryQueryHandler = getByCategoryQueryHandler;
            _createCommandHandler = createCommandHandler;
        }

        public async Task<GetAllSkillsResponse> GetAllSkillsAsync()
        {
            return await _getAllQueryHandler.HandleAsync(new GetAllSkillsRequest());
        }

        public async Task<GetAllSkillsResponse> GetSkillsByCategoryAsync(string category)
        {
            return await _getByCategoryQueryHandler.HandleAsync(
                new GetSkillsByCategoryRequest { Category = category }
            );
        }

        public async Task<CreateSkillResponse> CreateSkillAsync(CreateSkillRequest request)
        {
            return await _createCommandHandler.HandleAsync(request);
        }
    }
}
