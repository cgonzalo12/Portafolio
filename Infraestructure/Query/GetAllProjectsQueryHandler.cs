using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infraestructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructure.Query
{
    public class GetAllProjectsQueryHandler : IQueryHandler<GetAllProjectsRequest, GetAllProjectsResponse>
    {
        private readonly IRedisContext _redis;
        private const string ProjectListKey = "portfolio:projects";

        public GetAllProjectsQueryHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<GetAllProjectsResponse> HandleAsync(GetAllProjectsRequest request)
        {
            var projects = await _redis.GetListAsync<Project>(ProjectListKey);

            var projectDtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Technologies = p.Technologies,
                ImageUrl = p.ImageUrl,
                GitHubUrl = p.GitHubUrl,
                LiveUrl = p.LiveUrl,
                CreatedDate = p.CreatedDate
            }).OrderByDescending(p => p.CreatedDate).ToList();

            if (request.Limit.HasValue)
            {
                projectDtos = projectDtos.Take(request.Limit.Value).ToList();
            }

            return new GetAllProjectsResponse
            {
                Projects = projectDtos,
                TotalCount = projects.Count
            };
        }
    }
}