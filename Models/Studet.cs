using System.Collections.Generic;

namespace MobileStore.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? StandardId { get; set; }
        public List<Course> Courses { get; set; }
    }
}