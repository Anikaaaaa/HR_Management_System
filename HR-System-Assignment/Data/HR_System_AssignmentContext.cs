using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HR_System_Assignment.Models;

namespace HR_System_Assignment.Data
{
    public class HR_System_AssignmentContext : DbContext
    {
        public HR_System_AssignmentContext ()
           
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        //entities
        public DbSet<Employee> Students { get; set; }
       

    }
}
