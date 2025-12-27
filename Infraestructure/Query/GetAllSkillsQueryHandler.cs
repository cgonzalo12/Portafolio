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
    public class GetAllSkillsQueryHandler : IQueryHandler<GetAllSkillsRequest, GetAllSkillsResponse>
    {
        private readonly IRedisContext _redis;
        private const string SkillListKey = "portfolio:skills";

        public GetAllSkillsQueryHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<GetAllSkillsResponse> HandleAsync(GetAllSkillsRequest request)
        {
            var skills = await _redis.GetListAsync<Skill>(SkillListKey);

            return new GetAllSkillsResponse
            {
                Skills = skills.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Level = s.Level,
                    Category = s.Category
                }).OrderByDescending(s => s.Level).ToList()
            };
        }
    }
}
