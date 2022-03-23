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

            Bank aBank = new Bank();

            var anATM = new Thread(
                () => startATM(aBank));

            anATM.Start();

            var secondATM = new Thread(
                () => startATM(aBank));

            secondATM.Start();
        }

        private static void startATM(Bank bank) {
            Application.Run(new ATMForm(bank));
        }
    }
}
