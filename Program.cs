using System;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n\n{ex.StackTrace}", "Error de inicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}