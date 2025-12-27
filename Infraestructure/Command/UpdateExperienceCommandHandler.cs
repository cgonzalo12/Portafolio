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
    public class UpdateExperienceCommandHandler : ICommandHandler<UpdateExperienceRequest, UpdateExperienceResponse>
    {
        private readonly IRedisContext _redis;
        private const string ExperienceKeyPrefix = "portfolio:experience:";

        public UpdateExperienceCommandHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<UpdateExperienceResponse> HandleAsync(UpdateExperienceRequest request)
        {
            try
            {
                var key = $"{ExperienceKeyPrefix}{request.Id}";
                var existing = await _redis.GetAsync<Experience>(key);

                if (existing == null)
                {
                    return new UpdateExperienceResponse
                    {
                        Success = false,
                        Message = "Experiencia no encontrada"
                    };
                }

                var entity = new Experience
                {
                    Id = request.Id,
                    Company = request.Company,
                    Position = request.Position,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsCurrentJob = request.IsCurrentJob,
                    Location = request.Location
                };

                await _redis.SetAsync(key, entity);

                return new UpdateExperienceResponse
                {
                    Success = true,
                    Message = "Experiencia actualizada correctamente"
                };
            }
            catch (Exception ex)
            {
                return new UpdateExperienceResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
