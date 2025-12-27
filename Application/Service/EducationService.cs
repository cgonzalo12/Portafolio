using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class EducationService  :IEducationService
    {
        private readonly IQueryHandler<GetAllEducationRequest, GetAllEducationResponse> _getAllEducationQuery;
        private readonly ICommandHandler<CreateEducationRequest, CreateEducationResponse> _createEducationCommand;

        public EducationService(
            IQueryHandler<GetAllEducationRequest, GetAllEducationResponse> getAllEducationQuery,
            ICommandHandler<CreateEducationRequest, CreateEducationResponse> createEducationCommand)
        {
            _getAllEducationQuery = getAllEducationQuery;
            _createEducationCommand = createEducationCommand;
        }

        public async Task<GetAllEducationResponse> GetAllEducationAsync()
        {
            return await _getAllEducationQuery.HandleAsync(new GetAllEducationRequest());
        }

        public async Task<CreateEducationResponse> CreateEducationAsync(CreateEducationRequest request)
        {
            return await _createEducationCommand.HandleAsync(request);
        }
    }
}
