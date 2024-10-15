using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Data_Anootation_2
{/// <summary>
 ///     //here we wanna discuss two things 
    //when we gonna do foreign keys we should first determine:
    //1-who is dependent entity      2-sho is prinicipal entity
    //the entity that we will put the foreign key in it , it's the dependent entity
    //the entity that we will get the foreign key from it , this is the principal entity
    /// </summary>


    //we are discussing another thing , we have two properties:
    //1-Navigation property   , 2-ForeignKey Property
   // public int StandardId { get; set; } //this is the ForeignKey Property
  //  public Standard Standard { get; set; } //this is the Navigation Property



    //so now we have 3 methods to perfrom the foreign key attribute
//1- put the attribute on the ForeignKey Property in the Dependent entity((passing the
//navigation property name on the parameter of the attribute))



//2-put the attribute on the navigation property in the Dependent entity ((passing the
//ForeignKey Property in the parameter of the attribute))



//3-put the attribute on the Navigation Property of the Prinicipal Entity ((passing the
//ForeignKey Property of the Dependent Entity on the parameter of the attribute ))




    public class Student  //Dependent Entity
    {
        public int StudentID { get; set; }

        //first example , METHOD 1
        [ForeignKey("Standard")]
        public int StandardRefId { get; set; } //ForeignKey Property
        public Standard Standard { get; set; } //Navigation Property
        //-----------------------------------------------------------
        //second example , METHOD 2
        public int StandardRefId { get; set; } //ForeignKey Property

        [ForeignKey("StandardRefId")]
        public Standard Standard { get; set; } //Navigation Property

        //---------------------------------

        public string StudentName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] Photo { get; set; }
        public decimal Height { get; set; }
        public float Weight { get; set; }
        public Grade Grade { get; set; }

    }
    public class Standard  //Principal Entity
    {
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        //---------------------------------

        //third example   ,,METHOD 3
        [ForeignKey("StandardRefId")]
        public ICollection<Student> Students { get; set; } //Navigation Property
    }

    public class StudentContext : DbContext
    {
        public StudentContext() : base("StudentDB")
        {
        }

        //making datasets for each entity
        public DbSet<Student> Students { get; set; }
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
        }
    }
}