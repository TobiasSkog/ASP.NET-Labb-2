using Microsoft.AspNetCore.Mvc.Rendering;

namespace net23_asp_net_labb2.Models;

public class SelectStudentViewModel
{
    public int? StudentId { get; set; }
    public Student? Student { get; set; }
    public SelectList? StudentList { get; set; }
}
