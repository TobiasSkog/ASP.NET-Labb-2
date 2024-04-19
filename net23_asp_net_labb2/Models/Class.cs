using System.ComponentModel.DataAnnotations;

namespace net23_asp_net_labb2.Models;

public class Class
{
    public int Id { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public string Name { get; set; }

    public ICollection<Student> Students { get; set; } = [];
}
