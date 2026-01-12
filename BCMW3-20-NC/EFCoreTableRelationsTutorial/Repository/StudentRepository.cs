using EFCoreTableRelationsTutorial.Dtos;
using EFCoreTableRelationsTutorial.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Xml;

namespace EFCoreTableRelationsTutorial.Repository
{
    /*
        Include ზედმეტად არ გამოიყენოთ, რადგან ის იწვევს დამატებით JOIN ოპერაციებს და შეიძლება დააზიანოს შესრულება.
     */

    public class StudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        //2. Include
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .ToListAsync();
        }

        //3. Include ThenInclude
        public async Task<List<Student>> GetAllStudentsWithCoursesAsync()
        {
            return await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .ToListAsync();
        }


        //4. Join Select AsNoTracking
        public async Task<IEnumerable<object>> GetAllStudentsWithCoursesUsingJoinAsync()
        {
            //ანონიმური ობიექტით, Join
            //return await _context.StudentCourses
            //    .Join(_context.Courses,
            //          sc => sc.CourseId,
            //          c => c.Id,
            //          (sc, c) => new
            //          {
            //              StudentName = sc.Student.Name,
            //              CourseTitle = c.Title
            //          })
            //    .AsNoTracking()
            //    .ToListAsync();


            //ანონიმური ობიექტით
            //return await _context.StudentCourses
            //    .Select(sc => new
            //    {
            //        StudentName = sc.Student.Name,
            //        CourseTitle = sc.Course.Title
            //    })
            //    .AsNoTracking()
            //    .ToListAsync();


            //ანონიმური ობიექტის გარეშე, DTO - ს გამოყენებთ
            return await _context.StudentCourses
                .Select(sc => new StudentCourseDto()
                {
                    StudentName = sc.Student.Name,
                    CourseTitle = sc.Course.Title
                })
                .AsNoTracking()
                .ToListAsync();
        }


        //5. Where და OrderByDescending
        public async Task<List<Student>> GetAllStudentsFilteredAndSortedAsync()
        {
            return await _context.Students
                .Where(s => s.Name.StartsWith("N"))
                .OrderByDescending(s => s.Name)
                .ToListAsync();
        }


        //6. Cycle Exception
        public async Task<string> GetAllBooksWithAuthorsInJsonAsync()
        {
            var result = await GetAllBooksAsync();

            //Map tp DTO
            var mapppedResult = MapToDto(result);
            return JsonSerializer.Serialize(mapppedResult, new JsonSerializerOptions() { WriteIndented = true });
        }


        //7. GroupBy
        public async Task<IEnumerable<object>> GroupStudentsByCourseCountAsync()
        {
            return await _context.StudentCourses
                .GroupBy(sc => sc.StudentId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    CourseCount = g.Count()
                })
                .ToListAsync();
        }



        private static IEnumerable<BookForGettingDto> MapToDto(List<Book> result)
        {
            return result.Select(r => new BookForGettingDto()
            {
                Id = r.Id,
                Title = r.Title,
                Author = new AuthorForGettingDto()
                {
                    Id = r.Author.Id,
                    FullName = r.Author.FullName
                }
            });
        }
    }
}
