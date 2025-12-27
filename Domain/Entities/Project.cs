using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Technologies { get; set; }
        public string ImageUrl { get; set; }
        public string GitHubUrl { get; set; }
        public string LiveUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
