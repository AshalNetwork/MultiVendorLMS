using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public  class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<UnitPayment> UnitPayments { get; set; }
        public DbSet<Course> Courses{ get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonPayment> LessonPayments { get; set; }
        public DbSet<LessonDetail> LessonDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UnitPayment>().
                HasKey(o => new { o.StudentId, o.UnitId });

            modelBuilder.Entity<LessonPayment>().
                HasKey(o => new { o.StudentId, o.LessonId });

           
            modelBuilder.Entity<Course>()
                .HasOne(z=>z.Teacher)
                .WithMany(z=>z.Courses)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LessonDetail>().
                HasKey(o => new { o.LessonId, o.VideoId });


            base.OnModelCreating(modelBuilder);
        }

    }
}
