namespace net23_asp_net_labb2.Models;

public class TeacherCourse
{
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }
}
