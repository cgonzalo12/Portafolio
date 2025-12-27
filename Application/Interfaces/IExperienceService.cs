using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExperienceService
    {
        Task<GetAllExperiencesResponse> GetAllExperiencesAsync();
        Task<GetExperienceByIdResponse> GetExperienceByIdAsync(int id);
        Task<CreateExperienceResponse> CreateExperienceAsync(CreateExperienceRequest request);
        Task<UpdateExperienceResponse> UpdateExperienceAsync(UpdateExperienceRequest request);
        Task<DeleteExperienceResponse> DeleteExperienceAsync(int id);
    }
}
