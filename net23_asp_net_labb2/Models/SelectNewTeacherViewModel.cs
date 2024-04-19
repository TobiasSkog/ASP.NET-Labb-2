using Microsoft.AspNetCore.Mvc.Rendering;

namespace net23_asp_net_labb2.Models;

public class SelectNewTeacherViewModel
{
    public Student? Student { get; set; }
    public Course? Course { get; set; }
    public SelectList? CourseList { get; set; }

    public int? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public int? NewTeacherId { get; set; }
    public Teacher? NewTeacher { get; set; }
    public SelectList? NewTeacherList { get; set; }
}
