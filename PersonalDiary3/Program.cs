using System.Diagnostics;
using System.Security.Principal;

namespace PersonalDiaryUpdater
{
    internal static class Program
    {
       

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if(!AdministratorConfirmed()) //관리자권한이 아닐때 관리자 권한으로 실행 
            {
                try
                {
                    ProcessStartInfo info = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = Application.ExecutablePath,
                        WorkingDirectory = Environment.CurrentDirectory,
                        Verb = "runas"
                    };

                    Process.Start(info);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message); 
                }
            }
            else
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                //ApplicationConfiguration.Initialize();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            
        }

        public static bool AdministratorConfirmed()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            bool administratorconfirm = false; 
            if(identity != null )
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                administratorconfirm = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return administratorconfirm;
        }
    }
}