using Application.DTOs;
using Application.Interfaces;
using System.Threading.Tasks;

namespace Application.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IQueryHandler<GetAllProjectsRequest, GetAllProjectsResponse> _getAllQueryHandler;
        private readonly ICommandHandler<CreateProjectRequest, CreateProjectResponse> _createCommandHandler;

        public ProjectService(
            IQueryHandler<GetAllProjectsRequest, GetAllProjectsResponse> getAllQueryHandler,
            ICommandHandler<CreateProjectRequest, CreateProjectResponse> createCommandHandler)
        {
            _getAllQueryHandler = getAllQueryHandler;
            _createCommandHandler = createCommandHandler;
        }

        public async Task<GetAllProjectsResponse> GetAllProjectsAsync(int? limit)
        {
            return await _getAllQueryHandler.HandleAsync(new GetAllProjectsRequest { Limit = limit });
        }

        public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
        {
            return await _createCommandHandler.HandleAsync(request);
        }
    }
}