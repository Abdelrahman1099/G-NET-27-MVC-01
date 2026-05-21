using GymManagement.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    internal class Trainer : GymUser
    {
        public Specialty specialty { get; set; }
    }
}
