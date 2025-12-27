using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllProjectsResponse
    {
        public List<ProjectDto> Projects { get; set; }
        public int TotalCount { get; set; }
    }
}
