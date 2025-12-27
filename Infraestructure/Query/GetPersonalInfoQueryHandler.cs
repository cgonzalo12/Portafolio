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
    public class GetPersonalInfoQueryHandler : IQueryHandler<GetPersonalInfoRequest, GetPersonalInfoResponse>
    {
        private readonly IRedisContext _redis;
        private const string PersonalInfoKey = "portfolio:personalinfo";

        public GetPersonalInfoQueryHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<GetPersonalInfoResponse> HandleAsync(GetPersonalInfoRequest request)
        {
            var entity = await _redis.GetAsync<PersonalInfo>(PersonalInfoKey);

            if (entity == null) return null;

            return new GetPersonalInfoResponse
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Title = entity.Title,
                Email = entity.Email,
                Phone = entity.Phone,
                Location = entity.Location,
                Bio = entity.Bio,
                LinkedInUrl = entity.LinkedInUrl,
                GitHubUrl = entity.GitHubUrl,
                ProfileImageUrl = entity.ProfileImageUrl

            };
        }
    }
}
