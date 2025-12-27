using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEducationService
    {
        Task<GetAllEducationResponse> GetAllEducationAsync();
        Task<CreateEducationResponse> CreateEducationAsync(CreateEducationRequest request);
    }
}
