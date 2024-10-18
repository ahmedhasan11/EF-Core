using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace FluentApi
{
    //In Entity Framework 6, the DbModelBuilder class
    //acts as a Fluent API using which we can configure many different things

    //To write Fluent API configurations,,
    //override the OnModelCreating() method of DbContext in a context class

    //FluentApi configures the following aspect:
    //1-Model-wide Configuration: Configures the default Schema, entities to be excluded in mapping
    //2-Entity Configuration: Configures entity to table and relationship mappings e.g. PrimaryKey, Index, table name, one-to-one, one-to-many, many-to-many
    //3-Property Configuration: Configures property to column mappings e.g. column name, nullability, Foreignkey, data type, concurrency column
    public class SchoolContext : DbContext
    {
        //Code-First will create the database tables
        //with the name of DbSet properties in the context class
        public DbSet<Student> Students { get; set; }
        public DbSet<Student> Standard { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here

           // following example sets the Admin schema as a default schema
            modelBuilder.HasDefaultSchema("Admin");//configure the default schema

            //mapping entity to tables
            modelBuilder.Entity<Student>().ToTable("StudentInfo");
            modelBuilder.Entity<Standard>().ToTable("StandardInfo", "dbo");
            //"dbo" because standard is in the dbo schema

            //mapping student entity into multiple tables
            //we mapped some properties of the Student entity to the StudentInfo table and
            //other properties to the StudentInfoDetail table using the Map() method

            //Map() method requires a delegate method parameter
            //You can pass an Action delegate or a lambda expression in the Map() method
            modelBuilder.Entity<Student>().Map(m =>
            {
                m.Properties(p => new { p.StudentId, p.StudentName });
                m.ToTable("StudentInfo");
            }).Map(m => {
                m.Properties(p => new { p.StudentId, p.Height, p.Weight, p.Photo, p.DateOfBirth });
                m.ToTable("StudentInfoDetail");
            });

            //instead of the previous code :

            modelBuilder.Entity<Student>().Map(delegate (EntityMappingConfiguration<Student> studentConfig)
            {
                studentConfig.Properties(p => new { p.StudentId, p.StudentName });
                studentConfig.ToTable("StudentInfo");
            });

            Action<EntityMappingConfiguration<Student>> studentMapping = m =>
            {
                m.Properties(p => new { p.StudentId, p.Height, p.Weight, p.Photo, p.DateOfBirth });
                m.ToTable("StudentInfoDetail");
            };

            modelBuilder.Entity<Student>().Map(studentMapping);


            //Configure primary key
            modelBuilder.Entity<Student>().HasKey<int>(s => s.StudentKey);
            modelBuilder.Entity<Standard>().HasKey<int>(s => s.StandardKey);

            //Configure composite primary key
            modelBuilder.Entity<Student>().HasKey<int>(s => new { s.StudentKey, s.StudentName });

            //Configure Column
            modelBuilder.Entity<Student>().Property(p => p.DateOfBirth)
                        .HasColumnName("DoB")
                        .HasColumnOrder(3)
                        .HasColumnType("datetime2");
            //The HasColumnName() method is used to change the column name of the DateOfBirth property. Also, the HasColumnOrder()
            //and HasColumnType() methods change the order and datatype of the corresponding column


            //Configure Null Column
            modelBuilder.Entity<Student>()
                    .Property(p => p.Heigth)
                    .IsOptional();

            //Configure NotNull Column
            modelBuilder.Entity<Student>()
                .Property(p => p.Weight)
                .IsRequired();

            //Set StudentName column size to 50 and change datatype to nchar 
            //IsFixedLength() change datatype from nvarchar to nchar
            modelBuilder.Entity<Student>()
                    .Property(p => p.StudentName)
                    .HasMaxLength(50).IsFixedLength();

            //Set size decimal(2,2)
            modelBuilder.Entity<Student>()
                .Property(p => p.Height)
                .HasPrecision(2, 2);
        }
    }

    public class Student
    {

    }
    public class Standard
    {

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}