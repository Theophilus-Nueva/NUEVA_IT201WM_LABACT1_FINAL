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
        private Button selectedSlotButton = null;

        public Form1()
        {
            InitializeComponent();
            InitializeParkingSlots();
        }

        private void selectParkingSlot(object sender, EventArgs e)
        {
            selectedSlotButton = sender as Button;
            if (selectedSlotButton != null)
            {
                txtAssignedSlot.Text = selectedSlotButton.Text;
            }
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
            string slotId = txtAssignedSlot.Text.Trim();
            string plateNum = txtPlateNumber.Text.Trim();
            string vType = combVehicleType.Text.Trim();
            string hoursText = txtHoursParked.Text.Trim();

            if (string.IsNullOrEmpty(slotId) ||
                string.IsNullOrEmpty(plateNum) ||
                string.IsNullOrEmpty(vType) ||
                !int.TryParse(hoursText, out int hours) ||
                !allSlots.ContainsKey(slotId))
            {
                return;
            }

            Parking_Slot activeSlot = allSlots[slotId];

            activeSlot.PlateNumber = plateNum;
            activeSlot.VehicleType = vType;
            activeSlot.NumberOfHoursParked = hours;

            if (vType == "Motorcycle") activeSlot.ParkingRate = 30m;
            else if (vType == "Car") activeSlot.ParkingRate = 50m;
            else activeSlot.ParkingRate = 80m;

            Button targetButton = null;
            Stack<Control> stack = new Stack<Control>();
            stack.Push(this);

            while (stack.Count > 0)
            {
                Control current = stack.Pop();

                if (current is Button btn && btn.Text == slotId)
                {
                    targetButton = btn;
                    break;
                }

                foreach (Control child in current.Controls)
                {
                    stack.Push(child);
                }
            }

            if (targetButton != null)
            {
                targetButton.UseVisualStyleBackColor = false;
                targetButton.FlatStyle = FlatStyle.Flat;
                targetButton.BackColor = Color.Red;
                targetButton.ForeColor = Color.White;

                targetButton.Text = plateNum;
            }

            selectedSlotButton = null;
            txtAssignedSlot.Clear();
            txtPlateNumber.Clear();
            combVehicleType.SelectedIndex = -1;
            txtHoursParked.Clear();
        }
    }

    
}
        