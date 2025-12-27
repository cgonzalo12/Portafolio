using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetExperienceByIdResponse
    {
        public ExperienceDto Experience { get; set; }
        public bool Found { get; set; }
    }
}
