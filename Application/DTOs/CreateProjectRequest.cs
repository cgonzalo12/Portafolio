using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateProjectRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Technologies { get; set; }
        public string ImageUrl { get; set; }
        public string GitHubUrl { get; set; }
        public string LiveUrl { get; set; }
    }
}
