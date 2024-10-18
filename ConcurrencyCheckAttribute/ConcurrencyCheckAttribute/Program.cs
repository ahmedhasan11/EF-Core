using System.ComponentModel.DataAnnotations;

namespace ConcurrencyCheckAttribute
{
    //[ConcurrencyCheck]
    //the corresponding column in the database table
    //will be used in the optimistic concurrency check using the where clause.
    public class Student
    {
        public int StudentId { get; set; }

        [ConcurrencyCheck]
        public string StudentName { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}