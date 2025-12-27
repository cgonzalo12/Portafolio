using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Query
{
    public class GetExperienceByIdQueryHandler : IQueryHandler<GetExperienceByIdRequest, GetExperienceByIdResponse>
    {
        private readonly IRedisContext _redis;
        private const string ExperienceKeyPrefix = "portfolio:experience:";

        public GetExperienceByIdQueryHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<GetExperienceByIdResponse> HandleAsync(GetExperienceByIdRequest request)
        {
            var key = $"{ExperienceKeyPrefix}{request.Id}";
            var entity = await _redis.GetAsync<Experience>(key);

            if (entity == null)
            {
                return new GetExperienceByIdResponse { Found = false };
            }

            return new GetExperienceByIdResponse
            {
                Found = true,
                Experience = new ExperienceDto
                {
                    Id = entity.Id,
                    Company = entity.Company,
                    Position = entity.Position,
                    Description = entity.Description,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    IsCurrentJob = entity.IsCurrentJob,
                    Location = entity.Location
                }
            };
        }

    }
}
