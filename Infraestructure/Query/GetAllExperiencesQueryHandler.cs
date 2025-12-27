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
    public class GetAllExperiencesQueryHandler : IQueryHandler<GetAllExperiencesRequest, GetAllExperiencesResponse>
    {
        private readonly IRedisContext _redis;
        private const string ExperienceKeyPrefix = "portfolio:experience:";

        public GetAllExperiencesQueryHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<GetAllExperiencesResponse> HandleAsync(GetAllExperiencesRequest request)
        {
            var keys = await _redis.GetKeysAsync($"{ExperienceKeyPrefix}*");
            var experiences = new List<ExperienceDto>();

            foreach (var key in keys)
            {
                if (key.Contains("counter")) continue;

                var exp = await _redis.GetAsync<Experience>(key);
                if (exp != null)
                {
                    experiences.Add(new ExperienceDto
                    {
                        Id = exp.Id,
                        Company = exp.Company,
                        Position = exp.Position,
                        Description = exp.Description,
                        StartDate = exp.StartDate,
                        EndDate = exp.EndDate,
                        IsCurrentJob = exp.IsCurrentJob,
                        Location = exp.Location
                    });
                }
            }

            if (request.OrderByDateDesc)
            {
                experiences = experiences.OrderByDescending(e => e.StartDate).ToList();
            }

            return new GetAllExperiencesResponse
            {
                Experiences = experiences,
                TotalCount = experiences.Count
            };
        }

    }
}
