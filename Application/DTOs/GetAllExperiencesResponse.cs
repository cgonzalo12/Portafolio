using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllExperiencesResponse
    {
        public List<ExperienceDto> Experiences { get; set; }
        public int TotalCount { get; set; }
    }
}
