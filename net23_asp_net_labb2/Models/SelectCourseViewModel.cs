using Microsoft.AspNetCore.Mvc.Rendering;

namespace net23_asp_net_labb2.Models;

public class SelectCourseViewModel
{
    public int? StudentId { get; set; }
    public Student? Student { get; set; }

    public int? CourseId { get; set; }
    public Course? Course { get; set; }
    public SelectList? CourseList { get; set; }
}
