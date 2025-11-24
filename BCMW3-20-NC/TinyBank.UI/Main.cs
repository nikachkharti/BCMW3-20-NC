using TinyBank.Repository.Models;
using TinyBank.Service.Helpers;

namespace TinyBank.UI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            //await GenerateDtoClasses();
        }




        private async Task GenerateDtoClasses()
        {
            var rootPath = "C:\\Users\\User\\Desktop\\IT STEP\\BCMW3-20-NC\\BCMW3-20-NC\\TinyBank.Service\\";

            await DtoGenerator
                .GenerateDtosInFolder(
                    typeof(Account).Assembly,
                    outputFolder: Path.Combine(rootPath, "Dtos")
                );
        }
    }
}
