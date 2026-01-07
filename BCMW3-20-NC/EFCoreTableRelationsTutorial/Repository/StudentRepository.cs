using EFCoreTableRelationsTutorial.Dtos;
using EFCoreTableRelationsTutorial.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTableRelationsTutorial.Repository
{
    public class StudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        //IEnumerable and IQueryable based on ToListAsync example
        public async Task<List<Student>> GetAllStudentsAsync() => await _context.Students.ToListAsync();


        //Include
        public async Task<List<Book>> GetAllBooksWithAuthorsAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .ToListAsync();
        }


        //Include and ThenInclude როდესაც გინდა Entity Framework-ის tracked entity
        public async Task<List<Student>> GetAllStudentsWithCoursesAsync()
        {
            return await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .ToListAsync();
        }


        //Join და Select როდესაც გინდა DTO ან untracked entity
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


        //Where და OrderByDescending
        public async Task<List<Student>> GetAllStudentsFilteredAndSortedAsync()
        {
            return await _context.Students
                .Where(s => s.Name.StartsWith("N"))
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }


        //GroupBy
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



    }
}
