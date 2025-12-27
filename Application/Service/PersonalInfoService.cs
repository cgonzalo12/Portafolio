using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class PersonalInfoService : IPersonalInfoService
    {
        private readonly IQueryHandler<GetPersonalInfoRequest, GetPersonalInfoResponse> _getQueryHandler;
        private readonly ICommandHandler<UpdatePersonalInfoRequest, UpdatePersonalInfoResponse> _updateCommandHandler;
        private readonly ICommandHandler<CreatePersonalInfoRequest, UpdatePersonalInfoResponse> _createCommandHandler;

        public PersonalInfoService(
            IQueryHandler<GetPersonalInfoRequest, GetPersonalInfoResponse> getQueryHandler,
            ICommandHandler<UpdatePersonalInfoRequest, UpdatePersonalInfoResponse> updateCommandHandler,
            ICommandHandler<CreatePersonalInfoRequest, UpdatePersonalInfoResponse> createCommandHandler)
        {
            _getQueryHandler = getQueryHandler;
            _updateCommandHandler = updateCommandHandler;
            _createCommandHandler = createCommandHandler;
        }

        public async Task<GetPersonalInfoResponse> GetPersonalInfoAsync()
        {
            return await _getQueryHandler.HandleAsync(new GetPersonalInfoRequest());
        }

        public async Task<UpdatePersonalInfoResponse> UpdatePersonalInfoAsync(UpdatePersonalInfoRequest request)
        {
            return await _updateCommandHandler.HandleAsync(request);
        }

        public async Task<UpdatePersonalInfoResponse> CreatePersonalInfoAsync(CreatePersonalInfoRequest request)
        {
            return await _createCommandHandler.HandleAsync(request);
        }

    }
}
