
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
    public class CreateEducationCommandHandler : ICommandHandler<CreateEducationRequest, CreateEducationResponse>
    {
        private readonly IRedisContext _redis;
        private const string EducationKey = "portfolio:educations";
        private const string CounterKey = "portfolio:education:counter";

        public CreateEducationCommandHandler(IRedisContext redis)
        {
            _redis = redis;
        }

        public async Task<CreateEducationResponse> HandleAsync(CreateEducationRequest request)
        {
            var id = (int)await _redis.Database.StringIncrementAsync(CounterKey);

            var education = new Education
            {
                Id = id,
                Institution = request.Institution,
                Degree = request.Degree,
                FieldOfStudy = request.FieldOfStudy,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Description = request.Description
            };

            await _redis.AddToListAsync(EducationKey, education);

            return new CreateEducationResponse
            {
                Success = true,
                Message = "Educación creada correctamente",
                Id = id
            };
        }
    }
}
