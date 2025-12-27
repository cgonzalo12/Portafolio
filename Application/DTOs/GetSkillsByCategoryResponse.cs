using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetSkillsByCategoryResponse
    {
        public List<SkillDto> Skills { get; set; }
        public string Category { get; set; }
    }
}
