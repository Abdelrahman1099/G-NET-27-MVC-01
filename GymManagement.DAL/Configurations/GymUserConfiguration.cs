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
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(p => p.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            builder.Property(p => p.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);
            builder.HasIndex(U => U.Phone).IsUnique();
            builder.HasIndex(U => U.Email).IsUnique();

            builder.OwnsOne(U => U.Adress, adress =>
            {
                adress.Property(A => A.Street).HasColumnName("Street").HasColumnType("varchar(30)");
                adress.Property(A => A.City).HasColumnName("City").HasColumnType("varchar(30)");
                adress.Property(A => A.BulidingNumber).HasColumnName("BulidingNumber");
            });

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("Emailcheck", "Email like '_%@_%._%' ");
                tb.HasCheckConstraint("Phonecheck", "Phone like '010%' or Phone like '011%' or Phone like '012%' or Phone like '015%' ");
            });
        }
    }
}
