using EFCoreTableRelationsTutorial.Dtos;
using EFCoreTableRelationsTutorial.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace EFCoreTableRelationsTutorial.Repository
{
    public class StudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        #region ნაწილი 1

        //1. IEnumerable and IQueryable based on ToListAsync example
        public async Task<List<Student>> GetAllStudentsAsync() => await _context.Students.ToListAsync();


        //2. Include
        public async Task<List<Book>> GetAllBooksWithAuthorsAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .ToListAsync();
        }


        //2.2 Cycle Exception. ციკლური დამოკიდებულება JSON სერიალიზაციის დროს. უნდა გამოვიყენოთ DTO.
        public async Task AlertCycleExceptionExample()
        {
            var result = await GetAllBooksWithAuthorsAsync();

            //Solution: Map to DTO
            //var mappedReult = result.Select(r => new BookForGettingDto()
            //{
            //    Id = r.Id,
            //    Title = r.Title,
            //    Author = new AuthorForGettingDto()
            //    {
            //        Id = r.Author.Id,
            //        FullName = r.Author.FullName,
            //    }
            //});

            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions() { WriteIndented = true });
        }


        //3. Include and ThenInclude როდესაც გინდა Entity Framework-ის tracked entity
        public async Task<List<Student>> GetAllStudentsWithCoursesAsync()
        {
            return await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .ToListAsync();
        }


        //4. Join და Select როდესაც გინდა DTO ან untracked entity
        public async Task<IEnumerable<object>> GetAllStudentsWithCoursesAsyncUsingJoin()
        {
            #region V1 - JOIN არის AsNoTracking. DTO გარეშე

            //return await _context.StudentCourses
            //    .Join(_context.Courses,
            //        sc => sc.CourseId,
            //        c => c.Id,
            //        (sc, c) => new
            //        {
            //            StudentName = sc.Student.Name,
            //            CourseTitle = c.Title
            //        })
            //    .AsNoTracking() // optional
            //    .ToListAsync();

            #endregion


            #region V2 - NAVIGATION PROPERTIES არის AsNoTracking. DTO გარეშე

            //return await _context.StudentCourses
            //.Select(sc => new
            //{
            //    StudentName = sc.Student.Name,
            //    CourseTitle = sc.Course.Title
            //})
            //.AsNoTracking() // optional
            //.ToListAsync();

            #endregion


            #region V1 - JOIN არის AsNoTracking. DTO - თი

            //return await _context.StudentCourses
            //    .Join(_context.Courses,
            //        sc => sc.CourseId,
            //        c => c.Id,
            //        (sc, c) => new StudentCourseDto
            //        {
            //            StudentName = sc.Student.Name,
            //            CourseTitle = c.Title
            //        })
            //    .AsNoTracking() // optional
            //    .ToListAsync();

            #endregion


            #region V2 - NAVIGATION PROPERTIES არის AsNoTracking. DTO - თი

            return await _context.StudentCourses
            .Select(sc => new StudentCourseDto
            {
                StudentName = sc.Student.Name,
                CourseTitle = sc.Course.Title
            })
            .AsNoTracking() // optional
            .ToListAsync();

            #endregion
        }


        //5. Where და OrderByDescending
        public async Task<List<Student>> GetAllStudentsFilteredAndSortedAsync()
        {
            return await _context.Students
                .Where(s => s.Name.StartsWith("N"))
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }


        //6. GroupBy
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


        /*
            რჩევები EF + LINQ-ზე
            Include ზედმეტად არ გამოიყენო → მოაქვს ბევრი JOIN და ანელებს query-ს
            Projection (Select new {}) უკეთესია როცა არ გჭირდება სრული entity
            AsNoTracking() გამოიყენე როცა მხოლოდ წაკითხვა გინდა         
         */

        #endregion




    }
}
