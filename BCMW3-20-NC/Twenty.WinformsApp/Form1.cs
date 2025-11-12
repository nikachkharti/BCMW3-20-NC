using System.Web;

namespace Twenty.WinformsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t1 = new(() => SetLabel("Hello From btn 1", 8000));
            t1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t2 = new(() => SetLabel("Hello From btn 2", 2000));
            t2.Start();
        }

        private void SetLabel(string text, int delay)
        {
            Thread.Sleep(delay);
            messageLabel.Text = text;
        }
    }
}
