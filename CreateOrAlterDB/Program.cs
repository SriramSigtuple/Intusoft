using INTUSOFT.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CreateOrAlterDB
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            
            // Hide
            ShowWindow(handle, SW_HIDE);
            //MessageBox.Show(args[0]);
            if (args.Length >= 1)
            {
                var path = args[0];
                for (int i = 1; i < args.Length; i++)
                {
                    path = path + " " + args[i];
                }
                
                NHibernateHelper_MySQL.CreateOrAlterDB(path);
            }

        }
    }
}
