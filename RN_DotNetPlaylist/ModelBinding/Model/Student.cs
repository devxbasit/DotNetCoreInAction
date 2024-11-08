using System.Collections.Generic;

namespace ModelBinding.Model
{
    public class Student
    {
        public int Name { get; set; }
        public int[] Marks { get; set; }
        public Dictionary<int, string> SelectedCourses { get; set; }
    }
}