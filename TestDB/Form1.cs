using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBLoveleenJames;
namespace TestDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DataRow dr = DB.GetCustomer("ALFKI");
            DataTable dt = DB.GetProductTable();

            //int i = 2;
            //dt = DB.GetProductTable(i);
        }
    }
}
