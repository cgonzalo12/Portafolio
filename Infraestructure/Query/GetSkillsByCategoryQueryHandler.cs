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
    public class GetSkillsByCategoryQueryHandler : IQueryHandler<GetSkillsByCategoryRequest, GetAllSkillsResponse>
    {
        private readonly IRedisContext _redis;
        private const string SkillListKey = "portfolio:skills";

        public GetSkillsByCategoryQueryHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<GetAllSkillsResponse> HandleAsync(GetSkillsByCategoryRequest request)
        {
            var skills = await _redis.GetListAsync<Skill>(SkillListKey);

            var filtered = skills
                .Where(s => s.Category.Equals(request.Category, StringComparison.OrdinalIgnoreCase))
                .Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Level = s.Level,
                    Category = s.Category
                })
                .OrderByDescending(s => s.Level)
                .ToList();

            return new GetAllSkillsResponse { Skills = filtered };
        }
    }
}
