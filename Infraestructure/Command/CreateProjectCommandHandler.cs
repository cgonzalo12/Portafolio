using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class CreateProjectCommandHandler : ICommandHandler<CreateProjectRequest, CreateProjectResponse>
    {
        private readonly IRedisContext _redis;
        private const string Key = "portfolio:projects";
        private const string CounterKey = "portfolio:project:counter";

        public CreateProjectCommandHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<CreateProjectResponse> HandleAsync(CreateProjectRequest request)
        {
            var id = (int)await _redis.Database.StringIncrementAsync(CounterKey);

            var project = new Project
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                Technologies = request.Technologies,
                ImageUrl = request.ImageUrl,
                GitHubUrl = request.GitHubUrl,
                LiveUrl = request.LiveUrl,
                CreatedDate = DateTime.UtcNow
            };

            await _redis.AddToListAsync(Key, project);

            return new CreateProjectResponse { Success = true, Id = id };
        }
    }
}
