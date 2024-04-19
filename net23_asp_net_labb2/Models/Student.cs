using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace net23_asp_net_labb2.Models;

public class Student
{
    public int Id { get; set; }

    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public string Name { get; set; }

    public int ClassId { get; set; }
    public Class Class { get; set; }

    [DisplayName("Student Courses")]
    public ICollection<StudentCourse> StudentCourses { get; set; } = [];
}
