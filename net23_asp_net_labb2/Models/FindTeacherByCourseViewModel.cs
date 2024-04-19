using System.ComponentModel;

namespace net23_asp_net_labb2.Models;

public class FindTeacherByCourseViewModel
{
    [DisplayName("Course ID")]
    public int CourseId { get; set; }

    [DisplayName("Course name")]
    public string CourseName { get; set; }

    public List<Teacher> Teachers { get; set; }
}
