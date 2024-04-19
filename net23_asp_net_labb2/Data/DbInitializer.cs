using net23_asp_net_labb2.Models;

namespace net23_asp_net_labb2.Data;

public class DbInitializer(ApplicationDbContext _context)
{
    private static readonly Random _random = new();
    public async Task Initialize()
    {
        await _context.Database.EnsureCreatedAsync();

        if (_context.Students.Any())
        {
            return;
        }

        // -- Create and Initialize Teachers and add then to the DB -- //

        await InitTeachers();

        // -- Create and Initialize Classes and add then to the DB -- //

        await InitClasses();

        // -- Create and Initialize Courses and add then to the DB -- //

        await InitCourses();

        // -- Create and Initialize Students and add then to the DB -- //

        await InitStudents();

        // -- Create and Initialize the jointable StudentCourses and add then to the DB -- //

        await InitStudentCourses();

        // -- Create and Initialize the jointable TeacherCourses and add then to the DB -- //

        await InitTeacherCourses();
    }

    private async Task InitTeacherCourses()
    {
        var teacherCourses = new List<TeacherCourse>
        {
            new() { TeacherId = 1, CourseId = 1 },
            new() { TeacherId = 1, CourseId = 2 },
            new() { TeacherId = 1, CourseId = 4 },
            new() { TeacherId = 1, CourseId = 5 },
            new() { TeacherId = 2, CourseId = 1 },
            new() { TeacherId = 2, CourseId = 2 },
            new() { TeacherId = 2, CourseId = 4 },
            new() { TeacherId = 2, CourseId = 5 },
            new() { TeacherId = 3, CourseId = 1 },
            new() { TeacherId = 3, CourseId = 2 },
            new() { TeacherId = 3, CourseId = 3 },
            new() { TeacherId = 3, CourseId = 4 },
            new() { TeacherId = 3, CourseId = 5 },

            new() { TeacherId = 4, CourseId = 3 },
            new() { TeacherId = 4, CourseId = 6 },

            new() { TeacherId = 5, CourseId = 3 },

            new() { TeacherId = 6, CourseId = 1 },
            new() { TeacherId = 6, CourseId = 7 },
            new() { TeacherId = 6, CourseId = 8 },

            new() { TeacherId = 7, CourseId = 8 },

            new() { TeacherId = 1, CourseId = 9 },
            new() { TeacherId = 1, CourseId = 10 },

            new() { TeacherId = 2, CourseId = 9 },
            new() { TeacherId = 2, CourseId = 10 },
        };

        foreach (var teacherCourse in teacherCourses)
        {
            await _context.AddAsync(teacherCourse);
        }

        await _context.SaveChangesAsync();
    }

    private async Task InitStudentCourses()
    {
        var studentCourses = new List<StudentCourse>();

        for (int studentId = 1; studentId <= 42; studentId++)
        {
            var courses = Enumerable.Range(1, 10).ToList();

            Shuffle(courses);

            int numCourses = _random.Next(3, 9);
            numCourses = Math.Min(numCourses, courses.Count);

            for (int i = 0; i < numCourses; i++)
            {
                studentCourses.Add(new StudentCourse { StudentId = studentId, CourseId = courses[i] });
            }
        }

        foreach (var studentCourse in studentCourses)
        {
            await _context.AddAsync(studentCourse);
        }

        await _context.SaveChangesAsync();
    }



    private async Task InitCourses()
    {
        var courses = new List<Course>
        {
            new() { Name = "Object-Oriented Programming with C# & .NET" },              // 1 
            new() { Name = "Database Programming and SQL" },                            // 2
            new() { Name = "Project Management and Agile Methods" },                    // 3
            new() { Name = "Web Development: Frontend with HTML5, CSS, JavaScript" },   // 4
            new() { Name = "Web Applications in C#, ASP.NET" },                         // 5
            new() { Name = "Design Patterns and Architecture" },                        // 6
            new() { Name = "AI Components and Machine Learning in MS Azure" },          // 7
            new() { Name = "DevOps" },                                                  // 8
            new() { Name = "Programming 1" },                                           // 9
            new() { Name = "Programming 2" }                                            // 10
        };

        foreach (var course in courses)
        {
            await _context.AddAsync(course);
        }
        await _context.SaveChangesAsync();
    }

    private async Task InitClasses()
    {
        var classes = new List<Class>
        {
            new() { Name = "Stack Overflow Exception" },
            new() { Name = "Null Reference Exception" },
            new() { Name = "Divide By Zero Exception" }
        };

        foreach (var c in classes)
        {
            await _context.AddAsync(c);
        }

        await _context.SaveChangesAsync();
    }

    private async Task InitStudents()
    {
        var students = new List<Student>
        {
            new() { Name = "Josefin Mikaelsson", ClassId = 1 },
            new() { Name = "Theodor Hägg" , ClassId = 1 },
            new() { Name = "Elias Petrusson" , ClassId = 1 },
            new() { Name = "Wille Persson" , ClassId = 1 },
            new() { Name = "Peter Molen" , ClassId = 1 },
            new() { Name = "Alexander Doja" , ClassId = 1 },
            new() { Name = "Eric Sällström" , ClassId = 1 },
            new() { Name = "Emil Nordin" , ClassId = 1 },
            new() { Name = "Jonna Gustafsson" , ClassId = 1 },
            new() { Name = "Erik Berglund" , ClassId = 1 },
            new() { Name = "Theres Sundberg Selin" , ClassId = 1 },
            new() { Name = "Anette Johansson" , ClassId = 1 },
            new() { Name = "Patrik Petterson" , ClassId = 1 },
            new() { Name = "Malin Lövqvist" , ClassId = 1 },
            new() { Name = "Frida Eriksson" , ClassId = 2 },
            new() { Name = "Oa Blom" , ClassId = 2 },
            new() { Name = "Olof Maleki Nordin" , ClassId = 2 },
            new() { Name = "Daniel Frykman" , ClassId = 2 },
            new() { Name = "Fredrik Nellbeck" , ClassId = 2 },
            new() { Name = "Agnes Perälä" , ClassId = 2 },
            new() { Name = "Adrian Moreno Nyström" , ClassId = 2 },
            new() { Name = "Emma Lind" , ClassId = 2 },
            new() { Name = "Madde Lundström" , ClassId = 2 },
            new() { Name = "Mohamed Mohamud" , ClassId = 2 },
            new() { Name = "Tobias Skog" , ClassId = 2 },
            new() { Name = "Fredrik Halvarsson" , ClassId = 2 },
            new() { Name = "Zia Nourozi" , ClassId = 2 },
            new() { Name = "Yarub Adnan" , ClassId = 3 },
            new() { Name = "Angelica Lindström" , ClassId = 3 },
            new() { Name = "Gabriella Nilsson" , ClassId = 3 },
            new() { Name = "Jana Johansson" , ClassId = 3 },
            new() { Name = "Heba Derawi" , ClassId = 3 },
            new() { Name = "Morgan Westin" , ClassId = 3 },
            new() { Name = "Tobias Söderqvist" , ClassId = 3 },
            new() { Name = "Ester Zetterlund" , ClassId = 3 },
            new() { Name = "Hasan AL Hasan" , ClassId = 3 },
            new() { Name = "Viktoria Wallström" , ClassId = 3 },
            new() { Name = "Efrem Ghebre" , ClassId = 3 },
            new() { Name = "Olov Olsson" , ClassId = 3 },
            new() { Name = "Caroline Uthawong-Burr" , ClassId = 3 },
            new() { Name = "Martin Lindenhöök" , ClassId = 3 },
            new() { Name = "Amie Molin" , ClassId = 3 }
        };

        foreach (var student in students)
        {
            await _context.AddAsync(student);
        }

        await _context.SaveChangesAsync();
    }

    private async Task InitTeachers()
    {
        var teachers = new List<Teacher>
        {
            new() { Name = "Aldor Besher" },         // 1
            new() { Name = "Reidar Nilsen" },        // 2
            new() { Name = "Tobias Landén" },        // 3
            new() { Name = "Arnar Johannsson" },     // 4
            new() { Name = "Andreas Rosenqvist" },   // 5
            new() { Name = "Anas Alhussain" },       // 6
            new() { Name = "Jessia Niord" }          // 7
       };

        foreach (var teacher in teachers)
        {
            await _context.AddAsync(teacher);
        }

        await _context.SaveChangesAsync();
    }
    private static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}