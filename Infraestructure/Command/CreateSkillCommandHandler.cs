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
    public class CreateSkillCommandHandler : ICommandHandler<CreateSkillRequest, CreateSkillResponse>
    {
        private readonly IRedisContext _redis;
        private const string SkillListKey = "portfolio:skills";
        private const string SkillCounterKey = "portfolio:skill:counter";

        public CreateSkillCommandHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<CreateSkillResponse> HandleAsync(CreateSkillRequest request)
        {
            try
            {
                var id = (int)await _redis.Database.StringIncrementAsync(SkillCounterKey);

                var entity = new Skill
                {
                    Id = id,
                    Name = request.Name,
                    Level = request.Level,
                    Category = request.Category
                };

                await _redis.AddToListAsync(SkillListKey, entity);

                return new CreateSkillResponse
                {
                    Success = true,
                    Message = "Skill creada correctamente",
                    Id = id
                };
            }
            catch (Exception ex)
            {
                return new CreateSkillResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
