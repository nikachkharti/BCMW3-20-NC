using EFCoreTableRelationsTutorial.Repository;
using System.Threading.Tasks;

namespace EFCoreTableRelationsTutorial
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            StudentRepository studentRepository = new StudentRepository(new ApplicationDbContext());


            var result = await studentRepository.GroupStudentsByCourseCountAsync();

        }
    }
}
