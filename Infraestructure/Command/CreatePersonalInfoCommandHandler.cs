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
    public class CreatePersonalInfoCommandHandler : ICommandHandler<CreatePersonalInfoRequest, UpdatePersonalInfoResponse>
    {
        private readonly IRedisContext _redis;
        private const string Key = "portfolio:personalinfo";

        public CreatePersonalInfoCommandHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<UpdatePersonalInfoResponse> HandleAsync(CreatePersonalInfoRequest request)
        {
            var exists = await _redis.GetAsync<PersonalInfo>(Key);
            if (exists != null)
            {
                return new UpdatePersonalInfoResponse
                {
                    Success = false,
                    Message = "La información personal ya existe"
                };
            }

            var entity = new PersonalInfo
            {
                Id = 1,
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

            await _redis.SetAsync(Key, entity);

            return new UpdatePersonalInfoResponse
            {
                Success = true,
                Message = "Información personal creada",
                Id = 1
            };
        }
    }
}
