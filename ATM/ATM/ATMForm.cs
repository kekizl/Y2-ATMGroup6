using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM
{
    public partial class ATMForm : Form
    {
       
        private Bank Bank;
        //local referance to the array of accounts
        private Account[] ac;

        String input;
        int acNum = 0;
        int pin = 0;

        Label textControlMain = new Label();
        Label textControl1 = new Label();
        Label textControl2 = new Label();
        Label textControl3 = new Label();

        //this is a referance to the account that is being used
        private Account activeAccount = null;

        //creates button grid
        Button[,] btn = new Button[4, 4];
        Button[,] ctrl = new Button[2, 4];
        public ATMForm(Bank bank)
        {
            //initialization process 
            InitializeComponent();
            this.Bank = bank;
            retrieveAccounts();

            input = "";

            //how to add text to panels: https://stackoverflow.com/questions/57994165/how-can-i-write-text-on-a-panel-in-c

            textControlMain.Location = new Point(panel1.Location.X - 225, panel1.Location.Y + 20);
            textControlMain.ForeColor = Color.White;
            panel1.Controls.Add(textControlMain);
            textControl1.Location = new Point(panel1.Location.X - 225, textControlMain.Location.Y + 50);
            textControl1.ForeColor = Color.White;
            panel1.Controls.Add(textControl1);
            textControl2.Location = new Point(panel1.Location.X - 225, textControl1.Location.Y + 50);
            textControl2.ForeColor = Color.White;
            panel1.Controls.Add(textControl2);

            int count = 1;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(250 + (75 * x), 225 + (55 * y), 65, 45);
                    btn[x, y].BackColor = Color.Gray;
                    btn[x, y].Text = Convert.ToString(count);
                    btn[x, y].Click += new EventHandler(this.btnEvent_Click); 
                    Controls.Add(btn[x, y]);
                    count += (x < 3 && y < 3) ? 1 : 0;
                }
            }
            btn[0, 3].Text = "";
            btn[1, 3].Text = "0";
            btn[2, 3].Text = "";
            btn[3, 3].Text = "";
            btn[3, 3].Text = "";
            btn[3, 0].Text = "cancel";
            btn[3, 1].Text = "clear";
            btn[3, 2].Text = "enter";
            btn[3, 0].BackColor = Color.Red;
            btn[3, 1].BackColor = Color.Orange;
            btn[3, 2].BackColor = Color.Green;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    ctrl[x, y] = new Button();
                    ctrl[x, y].SetBounds(160 + (400 * x), 5 + (55 * y), 65, 45);
                    ctrl[x, y].BackColor = Color.Gray;
                    ctrl[x, y].Text = (x == 0) ? ">" : "<";
                    ctrl[x, y].Click += new EventHandler(this.ctrlEvent_Click);
                    Controls.Add(ctrl[x, y]);
                    count++;
                }
            }
  
            if(activeAccount == null)
            {
                textControlMain.Text = "enter your account number..";
            }
        }

        /*
         *    this method checks if input matches an existing account
         * 
         */
        private void retrieveAccounts() {
            this.ac = Bank.getAccounts();
        
        }
        private Account findAccount()
        {

            for (int i = 0; i < ac.Length; i++)
            {
                if (ac[i].getAccountNum() == Convert.ToInt32(acNum))
                {
                    return ac[i];
                }
            }

            return null;
        }

        /*
         * 
         *  give the use the options to do with the account
         *  
         *  
         */
        private void dispOptions()
        {
            textControlMain.Text = "take out cash";
            textControl1.Text =  "balance";
            textControl2.Text = "exit";
        }

        /*
         * 
         * offer withdrawable amounts
         * 
         * 
         */
        private void dispWithdraw()
        {
            textControlMain.Text ="10";
            textControl1.Text = "50";
            textControl2.Text = "500";

        }
        /*
         *  display balance of activeAccount and await keypress
         *  
         */
        private void dispBalance()
        {
            if (this.activeAccount != null)
            {
                textControlMain.Text = "balance " + activeAccount.getBalance();
                textControl1.Text = "";
                textControl2.Text = "exit";
            }
        }

        void btnEvent_Click(object sender, EventArgs e)
        {

            int test;
            if (int.TryParse(((Button)sender).Text, out test))
            {
                input += ((Button)sender).Text;
            }
            else if (((Button)sender) == btn[3, 2])
            {
                if (input.Length == 6)
                {
                    acNum = Convert.ToInt32(input);
                    activeAccount = findAccount();
                    textControlMain.Text = (activeAccount != null) ? "enter pin" : "enter your account number..";
                    input = "";
                }
                else if (input.Length == 4)
                {
                    pin = Convert.ToInt32(input);
                    if (activeAccount.checkPin(pin))
                    {
                        dispOptions();
                    }
                }
            }
            else if (((Button)sender) == btn[3, 1])
            {
                input = "";
            }
            else if (((Button)sender) == btn[3, 0])
            {
                textControlMain.Text = ("enter your account number..");
                textControl1.Text = "";
                textControl2.Text = "";
                input = "";
                acNum = 0;
                pin = 0;
                activeAccount = null;
            }
        }
            
        void ctrlEvent_Click(object sender, EventArgs e) {
            if (((Button)sender) == ctrl[0, 0] && textControlMain.Text == "take out cash")
            {
                dispWithdraw();
            }
            else if (((Button)sender) == ctrl[0, 1] && textControl1.Text == "balance")
            {
                dispBalance();
            }
            //opiton one is entered by the user
            else if (((Button)sender) == ctrl[0, 0] && textControlMain.Text == "10")
            {

                //attempt to decrement account by 10 punds
                if (activeAccount.decrementBalance(10))
                {

                    //if this is possible display new balance and await key press
                    textControlMain.Text = "new balance " + activeAccount.getBalance();
                    textControl1.Text = "";
                    textControl2.Text = "exit";
                }
                else
                {
                    //if this is not possible inform user and await key press
                    textControlMain.Text = ("insufficent funds");
                    textControl1.Text = "";
                    textControl2.Text = "exit";
                }
            }
            else if (((Button)sender) == ctrl[0, 1] && textControl1.Text == "50")
            {
                if (activeAccount.decrementBalance(50))
                {
                    textControlMain.Text = "new balance " + activeAccount.getBalance();
                    textControl1.Text = "";
                    textControl2.Text = "exit";
                }
                else
                {
                    textControlMain.Text = ("insufficent funds");
                    textControl1.Text = "";
                    textControl2.Text = "exit";
                }
            }
            else if (((Button)sender) == ctrl[0, 2] && textControl2.Text == "500")
            {
                if (activeAccount.decrementBalance(500))
                {
                    textControlMain.Text = "new balance " + activeAccount.getBalance();
                    textControl1.Text = "";
                    textControl2.Text = "exit";
                }
                else
                {
                    textControlMain.Text = ("insufficent funds");
                    textControl1.Text = "";
                    textControl2.Text = "exit";
                }

            }
            else if (((Button)sender) == ctrl[0, 2] && textControl2.Text == "exit")
            {
                textControlMain.Text = ("enter your account number..");
                textControl1.Text = "";
                textControl2.Text = "";
                input = "";
                acNum = 0;
                pin = 0;
                activeAccount = null;
            }
        }

        private void ATMForm_Load(object sender, EventArgs e)     
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }
    }
}
