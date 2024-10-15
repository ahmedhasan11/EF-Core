using System.Data.Entity;
using System.Diagnostics;

namespace Entity_Framework
{
    // the base constructor of the context class can have
    //1-No Parameter
    //creates a database in your local SQLEXPRESS server with
    //a name that matches your {Namespace}.{Context class name}

    //here the name of DB is Entity_Framework.SchoolContext
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base()
        {
            //DB initializaers

            Database.SetInitializer<SchoolContext>(new CreateDatabaseIfNotExists<SchoolContext>());

            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseAlways<SchoolDBContext>());


            //Disable initializer
            Database.SetInitializer<SchoolContext>(null);//Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer());

        }
        //The DbSet is a collection of entity classes
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }


    //2-DataBase Name
    //creates a database with the name you specified
    //in the base constructor in the local SQLEXPRESS database server
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base("MySchoolB")
        {

        }
        //The DbSet is a collection of entity classes
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }

    //3-Connection String Name
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base("name=SchoolDBConnectionString")
        {

        }
        //The DbSet is a collection of entity classes
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }
    public class Grade
    {
        public int GradeId { get; set; }
//EF will create a primary key column for the property named Id or <Entity Class Name> + "Id" (case insensitive).
        public string GradeName { get; set; }
        public string Section { get; set; }

        public ICollection<Student> Students { get; set; }
    }
    public class Student
    {
        public int StudentID { get; set; }
//EF will create a primary key column for the property named Id or <Entity Class Name> + "Id" (case insensitive).
        public string StudentName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] Photo { get; set; }
        public decimal Height { get; set; }
        public float Weight { get; set; }

        public Grade Grade { get; set; }
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

            using (var ctx = new SchoolContext())//making object from context class
            {
                var stud = new Student() { StudentName = "Bill" };
                //making object from student class

                ctx.Students.Add(stud);
                ctx.SaveChanges();
            }
        }
    }
}