using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyBank.Repository.Models;
using TinyBank.Service.Helpers;

namespace TinyBank.WinformsApp
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
            var rootPath = "C:\\Users\\User\\Desktop\\AAAAAA\\BCMW3-20-NC\\BCMW3-20-NC\\TinyBank.Service\\";

            await DtoGenerator.GenerateDtosInFolder(
                typeof(Account).Assembly,
                Path.Combine(rootPath, "Dtos")
            );
        }
    }
}
