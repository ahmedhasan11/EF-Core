using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Data_Annotations_Attributes
{
    //here we wanna learn the [InverseProperty] attribute , we use it when we have more than 
    //one relationship between two entities 
    //check the relationship between Course & Teacher classes
    //we have two 1-M relation ships
    //First: teacher can teach multiple online courses 
    //Second : teacher can teach multiple class room courses

    //so now we got two relationships between 2 entities , so lets use [InverseProperty] attribute

    //we will put the attribute here on the Teacher Class on the two navigation properties of the
    //Teacher Entity to specify their related navigation properties in Course Entity

    //[InverseProperty("put here the name of the related nav property of the course class")]

    //all of what we discussed gonna make 2 foreign keys in course table 
    //First one :OnlineTeacher_TeacherId     Second one:ClassRoomTeacher_TeacherId
    //so the syntax above was 
    //(Course Entity Navigation Property Name_Teacher Entity Primary Key Name )


    //tip: you can use the [ForeignKey] attribute to change the name of the ForeignKey that have
    //been put in the table
    public class Course
    {

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }

        public Teacher OnlineTeacher { get; set; }

        public Teacher ClassRoomTeacher { get; set; }
    }

    public class Teacher
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }

        [InverseProperty("OnlineTeacher")]
        public ICollection<Course> OnlineCourses { get; set; }

        [InverseProperty("ClassRoomTeacher")]
        public ICollection<Course> ClassRoomCourses { get; set; }
    }

    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public byte[] Photo { get; set; }

        public decimal Height { get; set; }

        public float Weight { get; set; }

        public Grade Grade { get; set; }
    }


    public class StudentContext : DbContext
    {
        public StudentContext(): base("StudentDB")
        {
        }

        //making datasets for each entity
        public DbSet<Student>Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }

    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string Section { get; set; }

        public ICollection<Student> Students { get; set; }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            using(var stdcls=new StudentContext())
            {
                var stud = new Student() { StudentName = "Bill" };

                stdcls.Students.Add(stud);
                stdcls.SaveChanges();
            }
        }
    }
}