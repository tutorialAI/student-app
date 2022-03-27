using System.Collections.Generic;

namespace MobileStore.Models
{
    public class Course
    {   
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }  
        public List<Student> Students { get; set; }
    }
}