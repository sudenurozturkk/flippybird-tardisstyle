using System;
using System.Windows.Forms;
using DoctorWhoFlappy.Win.UI;

namespace DoctorWhoFlappy.Win
{
    /// <summary>
    /// Ana program giriş noktası
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana giriş noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Windows Forms ayarları
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                // Ana formu başlat
                using (var mainForm = new MainForm())
                {
                    Application.Run(mainForm);
                }
            }
            catch (Exception ex)
            {
                // Kritik hata durumunda kullanıcıyı bilgilendir
                MessageBox.Show(
                    "Critical error occurred:\n" + ex.Message + "\n\nApplication will close.",
                    "Doctor Who Flappy - Critical Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
