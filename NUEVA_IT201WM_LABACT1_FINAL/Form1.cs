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
        private Dictionary<string, Parking_Slot> allSlots = new Dictionary<string, Parking_Slot>();

        public Form1()
        {
            InitializeComponent();
            InitializeParkingSlots();
        }

        private void selectParkingSlot(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            txtAssignedSlot.Text = clickedButton.Text;
        }

        private void InitializeParkingSlots()
        {
            for (char row = 'A'; row <= 'G'; row++)
            {
                for (int col = 1; col <= 5; col++)
                {
                    string currentId = $"{row}{col}";
                    Parking_Slot newSlot = new Parking_Slot();
                    newSlot.SlotID = currentId;
                    allSlots.Add(currentId, newSlot);
                }
            }
        }

        private void btnRegisterVehicle_Click(object sender, EventArgs e)
        {

        }
    }

    
}
        