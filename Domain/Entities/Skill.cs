using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; } // 1-5
        public string Category { get; set; } // Frontend, Backend, Database, etc
    }
}
