using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class ExperienceService : IExperienceService
    {
        private readonly IQueryHandler<GetAllExperiencesRequest, GetAllExperiencesResponse> _getAllQueryHandler;
        private readonly IQueryHandler<GetExperienceByIdRequest, GetExperienceByIdResponse> _getByIdQueryHandler;
        private readonly ICommandHandler<CreateExperienceRequest, CreateExperienceResponse> _createCommandHandler;
        private readonly ICommandHandler<UpdateExperienceRequest, UpdateExperienceResponse> _updateCommandHandler;
        private readonly ICommandHandler<DeleteExperienceRequest, DeleteExperienceResponse> _deleteCommandHandler;

        public ExperienceService(
            IQueryHandler<GetAllExperiencesRequest, GetAllExperiencesResponse> getAllQueryHandler,
            IQueryHandler<GetExperienceByIdRequest, GetExperienceByIdResponse> getByIdQueryHandler,
            ICommandHandler<CreateExperienceRequest, CreateExperienceResponse> createCommandHandler,
            ICommandHandler<UpdateExperienceRequest, UpdateExperienceResponse> updateCommandHandler,
            ICommandHandler<DeleteExperienceRequest, DeleteExperienceResponse> deleteCommandHandler)
        {
            _getAllQueryHandler = getAllQueryHandler;
            _getByIdQueryHandler = getByIdQueryHandler;
            _createCommandHandler = createCommandHandler;
            _updateCommandHandler = updateCommandHandler;
            _deleteCommandHandler = deleteCommandHandler;
        }

        public async Task<GetAllExperiencesResponse> GetAllExperiencesAsync()
        {
            return await _getAllQueryHandler.HandleAsync(new GetAllExperiencesRequest());
        }

        public async Task<GetExperienceByIdResponse> GetExperienceByIdAsync(int id)
        {
            return await _getByIdQueryHandler.HandleAsync(new GetExperienceByIdRequest { Id = id });
        }

        public async Task<CreateExperienceResponse> CreateExperienceAsync(CreateExperienceRequest request)
        {
            return await _createCommandHandler.HandleAsync(request);
        }

        public async Task<UpdateExperienceResponse> UpdateExperienceAsync(UpdateExperienceRequest request)
        {
            return await _updateCommandHandler.HandleAsync(request);
        }

        public async Task<DeleteExperienceResponse> DeleteExperienceAsync(int id)
        {
            return await _deleteCommandHandler.HandleAsync(new DeleteExperienceRequest { Id = id });
        }
    }
}
