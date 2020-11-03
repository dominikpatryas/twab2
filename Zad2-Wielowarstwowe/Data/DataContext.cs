using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zad2_Wielowarstwowe.Models;

namespace Zad2_Wielowarstwowe.Data
{
    class DataContext : DbContext
    {
        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .HasMany(b => b.TeamMembers)
                .WithOne(c => c.Team)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
    }
}
