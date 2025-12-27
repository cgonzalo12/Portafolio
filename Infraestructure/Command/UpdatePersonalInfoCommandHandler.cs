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
    public class UpdatePersonalInfoCommandHandler : ICommandHandler<UpdatePersonalInfoRequest, UpdatePersonalInfoResponse>
    {
        private readonly IRedisContext _redis;
        private const string PersonalInfoKey = "portfolio:personalinfo";

        public UpdatePersonalInfoCommandHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<UpdatePersonalInfoResponse> HandleAsync(UpdatePersonalInfoRequest request)
        {
            try
            {
                var entity = new PersonalInfo
                {
                    Id = request.Id == 0 ? 1 : request.Id,
                    FullName = request.FullName,
                    Title = request.Title,
                    Email = request.Email,
                    Phone = request.Phone,
                    Location = request.Location,
                    Bio = request.Bio,
                    LinkedInUrl = request.LinkedInUrl,
                    GitHubUrl = request.GitHubUrl,
                    ProfileImageUrl = request.ProfileImageUrl
                };

                await _redis.SetAsync(PersonalInfoKey, entity);

                return new UpdatePersonalInfoResponse
                {
                    Success = true,
                    Message = "Información personal actualizada correctamente",
                    Id = entity.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdatePersonalInfoResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
