using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ATM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
     
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var anATM = new Thread(
                () => startATM());

            anATM.Start();

            var secondATM = new Thread(
                () => startATM());

            secondATM.Start();
        }

        private static void startATM() {
            Application.Run(new ATMForm());
        }
    }
}
