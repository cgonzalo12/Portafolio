using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Infraestructure.Query
{
    public class GetAllEducationQueryHandler
        : IQueryHandler<GetAllEducationRequest, GetAllEducationResponse>
    {
        private readonly IRedisContext _redis;
        private const string EducationKey = "portfolio:educations";

        public GetAllEducationQueryHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<GetAllEducationResponse> HandleAsync(GetAllEducationRequest request)
        {
            var educations = await _redis.GetListAsync<Education>(EducationKey);

            var result = educations
                .Select(edu => new EducationDto
                {
                    Id = edu.Id,
                    Institution = edu.Institution,
                    Degree = edu.Degree,
                    FieldOfStudy = edu.FieldOfStudy,
                    StartDate = edu.StartDate,
                    EndDate = edu.EndDate,
                    Description = edu.Description
                })
                .OrderByDescending(e => e.StartDate)
                .ToList();

            return new GetAllEducationResponse
            {
                Education = result
            };
        }
    }
}
