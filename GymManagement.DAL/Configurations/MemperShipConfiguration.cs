using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Configurations
{
    public class MemperShipConfiguration : IEntityTypeConfiguration<MemperShip>
    {
        public void Configure(EntityTypeBuilder<MemperShip> builder)
        {
            builder.HasKey(M => M.Id);

            builder.Property(M => M.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");

        }
    }
}
