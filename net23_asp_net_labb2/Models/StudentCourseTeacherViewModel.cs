namespace net23_asp_net_labb2.Models;

public class StudentCourseTeacherViewModel
{
    public Course Course { get; set; }
    public List<Student> Students { get; set; }
    public List<Teacher> Teachers { get; set; }
}
