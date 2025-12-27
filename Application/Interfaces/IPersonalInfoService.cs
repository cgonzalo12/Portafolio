using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPersonalInfoService
    {
        Task<GetPersonalInfoResponse> GetPersonalInfoAsync();
        Task<UpdatePersonalInfoResponse> UpdatePersonalInfoAsync(UpdatePersonalInfoRequest request);
        Task<UpdatePersonalInfoResponse> CreatePersonalInfoAsync(CreatePersonalInfoRequest request);
    }
}
