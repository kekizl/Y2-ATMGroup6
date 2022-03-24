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


        /// <summary>
        /// Main Function of the program
        /// </summary>
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //bank object initialised
            Bank aBank = new Bank();

            //Modified from https://stackoverflow.com/questions/13776846/pass-parameters-through-parameterizedthreadstart
            //thread to make first ATM
            var anATM = new Thread(
                () => startATM(aBank));
            anATM.Start();

            //thread to make second ATM
            var secondATM = new Thread(
                () => startATM(aBank));
            secondATM.Start();
        }
        /// <summary>
        /// Creates an ATM form
        /// </summary>
        private static void startATM(Bank bank) {
            Application.Run(new ATMForm(bank));
        }
    }
}
