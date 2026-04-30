using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NUEVA_IT201WM_LABACT1_FINAL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void selectParkingSlot(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            txtAssignedSlot.Text = clickedButton.Text;
        }
    }

    
}
        