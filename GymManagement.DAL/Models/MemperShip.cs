using GymManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class MemperShip : BaseEntity
    {
        public Member Member {  get; set; }
        public int MemberId { get; set; }
        public Plan Plan { get; set; }
        public int PlanId { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive => EndDate > DateTime.UtcNow;
        public string Status => EndDate > DateTime.UtcNow ? "Active" : "Expired"; 
    }
}
