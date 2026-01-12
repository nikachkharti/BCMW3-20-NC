using EFCoreTableRelationsTutorial.Repository;

namespace EFCoreTableRelationsTutorial
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            StudentRepository studentRepository = new(new ApplicationDbContext());

            var result = await studentRepository.GroupStudentsByCourseCountAsync();
        }
    }
}
