using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SchoolSystemManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystemManagement.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<University> Universities { get; set; }

        public DbSet<Staff> Staffs{ get; set; }

        public DbSet<StaffWork> StaffWorks{ get; set; }

        public DbSet<Faculty> Faculties{ get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet< Lecturer> Lecturers { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentLesson> StudentLessons { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<University>().ToTable("University");
            modelBuilder.Entity<StaffWork>().ToTable("StaffWork");
            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Faculty>().ToTable("Faculty");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Lecturer>().ToTable("Lecturer");
            modelBuilder.Entity<Lesson>().ToTable("Lesson");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<StudentLesson>().ToTable("StudentLesson");
            modelBuilder.Entity<User>().ToTable("User");
        }

    }
}
