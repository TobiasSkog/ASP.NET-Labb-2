using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net23_asp_net_labb2.Data;
using net23_asp_net_labb2.Models;

namespace net23_asp_net_labb2.Controllers;
public class CourseController : Controller
{
    private readonly ApplicationDbContext _context;
    public CourseController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult FindTeacherBySubject()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> FindTeacherBySubject(int? courseId)
    {
        if (courseId == null)
        {
            return View();
        }

        var viewModel = await _context.Courses
           .Where(c => c.Id == courseId)
           .Include(c => c.TeacherCourses)
           .ThenInclude(tc => tc.Teacher)
           .Select(c => new FindTeacherByCourseViewModel
           {
               CourseId = c.Id,
               CourseName = c.Name,
               Teachers = c.TeacherCourses.Select(tc => tc.Teacher).ToList()
           })
           .FirstOrDefaultAsync();

        return View("ViewTeachersWithSubject", viewModel);
    }

    [HttpGet]
    public IActionResult ViewTeachersWithSubject(FindTeacherByCourseViewModel viewModel)
    {
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudentsWithTeachers()
    {
        var viewModelList = await _context.Students
            .Include(s => s.StudentCourses)
            .ThenInclude(sc => sc.Course)
            .ThenInclude(c => c.TeacherCourses)
            .ThenInclude(tc => tc.Teacher)
            .Select(s => new StudentWithAllTeachersViewModel
            {
                Student = s,
                Teachers = s.StudentCourses
                    .SelectMany(sc => sc.Course.TeacherCourses.Select(tc => tc.Teacher))
                    .Distinct()
                    .ToList()
            })
            .ToListAsync();

        return View(viewModelList);
    }


    [HttpGet]
    public IActionResult GetAllStudentsByCourseWithTeachers()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> GetAllStudentsByCourseWithTeachers(int? courseId)
    {
        if (courseId == null)
        {
            return View();
        }

        var viewModel = await _context.Courses
            .Where(c => c.Id == courseId)
            .Include(c => c.StudentCourses)
            .Include(c => c.TeacherCourses)
            .Select(c => new StudentCourseTeacherViewModel
            {
                Course = c,
                Students = c.StudentCourses.Select(sc => sc.Student).ToList(),
                Teachers = c.TeacherCourses.Select(tc => tc.Teacher).ToList()
            })
            .FirstOrDefaultAsync();



        return View("StudentsByCourseWithTeachersResult", viewModel);
    }
    [HttpGet]
    public IActionResult StudentsByCourseWithTeachersResult(SelectCourseViewModel viewModel)
    {
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        return View(await _context.Courses.ToListAsync());
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", course.Id);
        return View(course);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Course course)
    {
        if (id != course.Id)
        {
            return NotFound();
        }

        try
        {
            _context.Update(course);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(course.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(GetAllCourses));
    }
    [HttpGet]
    public IActionResult UpdateTeacherForStudentInCourse()
    {
        SelectStudentViewModel viewModel = new()
        {
            StudentList = new SelectList(_context.Students, "Id", "Name")
        };

        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateTeacherForStudentInCourse(SelectStudentViewModel viewModel)
    {
        var student = await _context.Students
            .Include(s => s.StudentCourses)
            .FirstOrDefaultAsync(s => s.Id == viewModel.StudentId);

        if (student != null)
        {
            return RedirectToAction("SelectCourse", student);
        }

        return View();
    }

    [HttpGet]
    public IActionResult SelectCourse(Student student)
    {
        SelectCourseViewModel viewModel = new()
        {
            StudentId = student.Id,
            Student = student,
            CourseList = new SelectList(_context.StudentCourses
               .Where(sc => sc.StudentId == student.Id)
               .Select(sc => sc.Course)
               .ToList(), "Id", "Name")
        };
        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> SelectCourse(SelectCourseViewModel viewModel)
    {
        var studentCourse = await _context.StudentCourses
            .Where(sc => sc.CourseId == viewModel.CourseId && sc.StudentId == viewModel.StudentId)
            .Include(sc => sc.Student)
            .Include(sc => sc.Course)
            .FirstOrDefaultAsync();
        if (studentCourse != null)
        {
            return RedirectToAction("SelectTeacher", new { studentId = viewModel.StudentId, courseId = viewModel.CourseId });
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> SelectTeacher(int studentId, int courseId)
    {
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        if (student == null || course == null)
        {
            return NotFound();
        }

        var teacher = await _context.TeacherCourses
            .Where(tc => tc.CourseId == courseId)
            .Include(tc => tc.Teacher)
            .Join(
                _context.StudentCourses.Where(sc => sc.StudentId == studentId),
                tc => tc.CourseId,
                sc => sc.CourseId,
                (tc, cs) => tc.Teacher
            )
            .FirstOrDefaultAsync();

        if (teacher == null)
        {
            return NotFound();
        }

        SelectNewTeacherViewModel viewModel = new()
        {
            Student = student,
            Course = course,
            TeacherId = teacher.Id,
            Teacher = teacher,
            NewTeacherList = new SelectList(_context.Teachers
                .Where(t => !_context.TeacherCourses.Any(tc => tc.CourseId == courseId && tc.TeacherId == t.Id))
                .ToList(), "Id", "Name")
        };
        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> SelectTeacher(int courseId, int teacherId, int newTeacherId)
    {
        var existingTeacherCourse = _context.TeacherCourses
            .Where(tc => tc.TeacherId == teacherId && tc.CourseId == courseId)
            .FirstOrDefault();
        var newTeacher = _context.Teachers.FirstOrDefault(t => t.Id == newTeacherId);
        if (existingTeacherCourse == null || newTeacher == null)
        {
            return NotFound();
        }

        try
        {
            _context.TeacherCourses.Remove(existingTeacherCourse);
            await _context.SaveChangesAsync();

            TeacherCourse newTeacherCourse = new()
            {
                TeacherId = newTeacherId,
                CourseId = courseId
            };
            _context.TeacherCourses.Add(newTeacherCourse);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TeacherCourseExists(existingTeacherCourse.TeacherId, existingTeacherCourse.CourseId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(UpdateTeacherForStudentInCourse));
    }
    private bool TeacherCourseExists(int teacherId, int courseId)
    {
        return _context.TeacherCourses.Any(tc => tc.TeacherId == teacherId && tc.CourseId == courseId);
    }
    private bool CourseExists(int id)
    {
        return _context.Courses.Any(c => c.Id == id);
    }
}
