using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class DeleteExperienceCommandHandler : ICommandHandler<DeleteExperienceRequest, DeleteExperienceResponse>
    {
        private readonly IRedisContext _redis;
        private const string ExperienceKeyPrefix = "portfolio:experience:";

        public DeleteExperienceCommandHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<DeleteExperienceResponse> HandleAsync(DeleteExperienceRequest request)
        {
            try
            {
                var key = $"{ExperienceKeyPrefix}{request.Id}";
                var deleted = await _redis.DeleteAsync(key);

                if (!deleted)
                {
                    return new DeleteExperienceResponse
                    {
                        Success = false,
                        Message = "Experiencia no encontrada"
                    };
                }

                return new DeleteExperienceResponse
                {
                    Success = true,
                    Message = "Experiencia eliminada correctamente"
                };
            }
            catch (Exception ex)
            {
                return new DeleteExperienceResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}

