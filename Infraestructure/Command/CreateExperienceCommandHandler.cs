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
    public class CreateExperienceCommandHandler : ICommandHandler<CreateExperienceRequest, CreateExperienceResponse>
    {
        private readonly IRedisContext _redis;
        private const string ExperienceKeyPrefix = "portfolio:experience:";
        private const string ExperienceCounterKey = "portfolio:experience:counter";

        public CreateExperienceCommandHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<CreateExperienceResponse> HandleAsync(CreateExperienceRequest request)
        {
            try
            {
                // Generar ID único
                var id = (int)await _redis.Database.StringIncrementAsync(ExperienceCounterKey);

                var entity = new Experience
                {
                    Id = id,
                    Company = request.Company,
                    Position = request.Position,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsCurrentJob = request.IsCurrentJob,
                    Location = request.Location
                };

                await _redis.SetAsync($"{ExperienceKeyPrefix}{id}", entity);

                return new CreateExperienceResponse
                {
                    Success = true,
                    Message = "Experiencia creada correctamente",
                    Id = id
                };
            }
            catch (Exception ex)
            {
                return new CreateExperienceResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }


        }
    }
}
