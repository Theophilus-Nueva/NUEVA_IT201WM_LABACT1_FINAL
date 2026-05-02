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

            if (selectedSlotButton != null && selectedSlotButton.Tag != null)
            {
                string slotId = selectedSlotButton.Tag.ToString();
                txtAssignedSlot.Text = slotId;

                if (allSlots.ContainsKey(slotId))
                {
                    Parking_Slot activeSlot = allSlots[slotId];

                    if (activeSlot.IsOccupied)
                    {
                        txtPlateNumber.Text = activeSlot.PlateNumber;
                        combVehicleType.Text = activeSlot.VehicleType;
                        txtHoursParked.Text = activeSlot.NumberOfHoursParked.ToString();

                        SetCurrentTransaction(activeSlot);
                    }
                    else
                    {
                        txtPlateNumber.Clear();
                        combVehicleType.SelectedIndex = -1;
                        txtHoursParked.Clear();

                        lblPlateNumber.Text = "- - -";
                        lblVehicleInfo.Text = "- - -";
                        lblDuration.Text = "- - -";
                        lblOvertimeFee.Text = "- - -";
                        lblStandardFee.Text = "- - -";
                    }
                }
            }
        }

        private void InitializeParkingSlots()
        {
            foreach (Button button in tableLayoutPanel1.Controls.OfType<Button>())
            {
                string slotId = button.Text;
                Parking_Slot newSlot = new Parking_Slot(slotId);
                allSlots.Add(slotId, newSlot);
                button.Tag = slotId;
            }
        }

        private void SetCurrentTransaction(Parking_Slot slot)
        {
            lblPlateNumber.Text = slot.PlateNumber;
            lblVehicleInfo.Text = slot.VehicleType;
            lblDuration.Text = $"{slot.NumberOfHoursParked}";

            lblStandardFee.Text = $"{slot.GetSubtotalFee():0.00}";
            lblOvertimeFee.Text = $"{slot.GetOvertimeFee():0.00}";

            lblTotal.Text = $"{slot.GetTotalFee():0.00}";
        }

        private void btnRegisterVehicle_Click(object sender, EventArgs e)
        {
            string slotId = txtAssignedSlot.Text;
            string plateNum = txtPlateNumber.Text;
            string vType = combVehicleType.Text;
            string hoursText = txtHoursParked.Text;

            if (string.IsNullOrEmpty(slotId) ||
                string.IsNullOrEmpty(plateNum) ||
                string.IsNullOrEmpty(vType) ||
                !int.TryParse(hoursText, out int hours) ||
                !allSlots.ContainsKey(slotId))
            {
                MessageBox.Show("Please ensure all fields are filled out correctly.", "Input Error");
                return;
            }

            Parking_Slot activeSlot = allSlots[slotId];

            activeSlot.PlateNumber = plateNum;
            activeSlot.VehicleType = vType;
            activeSlot.NumberOfHoursParked = hours;

            Button targetButton = null;
            Stack<Control> stack = new Stack<Control>();
            stack.Push(this);

            while (stack.Count > 0)
            {
                Control current = stack.Pop();

                if (current is Button btn && btn.Tag != null && btn.Tag.ToString() == slotId)
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
                targetButton.BackColor = Color.Red;
                targetButton.Text = plateNum;
            }

            SetCurrentTransaction(activeSlot);

            MessageBox.Show($"Vehicle {plateNum} has been registered to {slotId}.", "Registration Success");
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            string slotId = txtAssignedSlot.Text.Trim();

            if (string.IsNullOrEmpty(slotId) || !allSlots.ContainsKey(slotId))
            {
                MessageBox.Show("Please select a valid slot to update.");
                return;
            }

            Parking_Slot activeSlot = allSlots[slotId];

            if (!activeSlot.IsOccupied)
            {
                MessageBox.Show("This parking slot is already empty!");
                return;
            }

            activeSlot.VacateSlot();

            Button targetButton = null;
            Stack<Control> stack = new Stack<Control>();
            stack.Push(this);

            while (stack.Count > 0)
            {
                Control current = stack.Pop();

                if (current is Button btn && btn.Tag != null && btn.Tag.ToString() == slotId)
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
                targetButton.Text = slotId;

                targetButton.BackColor = Color.Lime;
            }

            selectedSlotButton = null;
            txtAssignedSlot.Clear();
            txtPlateNumber.Clear();
            combVehicleType.SelectedIndex = -1;
            txtHoursParked.Clear();

            lblPlateNumber.Text = "- - -";
            lblVehicleInfo.Text = "- - -";
            lblDuration.Text = "- - -";

            lblStandardFee.Text = "- - -";
            lblTotal.Text = "- - -";

            combDiscount.SelectedIndex = -1;
            txtPayAmount.Clear();
            txtReceipt.Clear();
            lblChange.Text = "- - -";

            MessageBox.Show($"Slot {slotId} has been successfully vacated!");
        }

        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            string slotId = txtAssignedSlot.Text.Trim();

            if (string.IsNullOrEmpty(slotId) || !allSlots.ContainsKey(slotId))
            {
                MessageBox.Show("Please select a valid parking slot first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Parking_Slot activeSlot = allSlots[slotId];

            if (!activeSlot.IsOccupied)
            {
                MessageBox.Show("There is no car in this slot to process!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPayAmount.Text, out decimal paymentAmount))
            {
                MessageBox.Show("Please enter a valid numerical amount for the payment.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalFee = activeSlot.GetTotalFee();

            if (paymentAmount < totalFee)
            {
                MessageBox.Show($"Insufficient funds! The total fee is ₱{totalFee:0.00}", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal change = paymentAmount - totalFee;

            lblChange.Text = $"{change}";
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            combDiscount.SelectedIndex = -1;
            txtPayAmount.Clear();
            txtReceipt.Clear();
            lblChange.Text = "- - -";
        }

        private void combDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string slotId = txtAssignedSlot.Text.Trim();

            if (!string.IsNullOrEmpty(slotId) && allSlots.ContainsKey(slotId))
            {
                Parking_Slot activeSlot = allSlots[slotId];

                if (activeSlot.IsOccupied)
                {
                    activeSlot.DiscountType = combDiscount.Text;

                    lblStandardFee.Text = $"{activeSlot.GetSubtotalFee():0.00}"; 
                    lblTotal.Text = $"{activeSlot.GetTotalFee():0.00}";          
                }
            }
        }

        private void btnGenerateReceipt_Click(object sender, EventArgs e)
        {
            string slotId = txtAssignedSlot.Text.Trim();

            if (string.IsNullOrEmpty(slotId) || !allSlots.ContainsKey(slotId))
            {
                MessageBox.Show("Please select a valid parking slot first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Parking_Slot activeSlot = allSlots[slotId];

            if (!activeSlot.IsOccupied)
            {
                MessageBox.Show("There is no car in this slot to generate a receipt for!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPayAmount.Text, out decimal paymentAmount))
            {
                MessageBox.Show("Please enter the payment amount first.", "Missing Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalFee = activeSlot.GetTotalFee();

            if (paymentAmount < totalFee)
            {
                MessageBox.Show("Insufficient payment. Cannot generate receipt.", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal change = paymentAmount - totalFee;

            string receipt = "=== OFFICIAL PARKING RECEIPT ===\r\n";
            receipt += $"Slot ID:      {activeSlot.SlotID}\r\n";
            receipt += $"Plate Number: {activeSlot.PlateNumber}\r\n";
            receipt += $"Vehicle Type: {activeSlot.VehicleType}\r\n";
            receipt += $"Hours Parked: {activeSlot.NumberOfHoursParked}\r\n";
            receipt += "--------------------------------\r\n";
            receipt += $"Standard Fee:   ₱{activeSlot.GetStandardFee():0.00}\r\n";
            receipt += $"Service Charge: ₱{activeSlot.FixedServiceCharge:0.00}\r\n";

            if (activeSlot.GetOvertimeFee() > 0)
            {
                receipt += $"Overtime Fee:   ₱{activeSlot.GetOvertimeFee():0.00}\r\n";
            }

            if (activeSlot.GetDiscountDeduction() > 0)
            {
                receipt += $"Discount ({activeSlot.DiscountType}): -₱{activeSlot.GetDiscountDeduction():0.00}\r\n";
            }

            receipt += "--------------------------------\r\n";
            receipt += $"TOTAL AMOUNT:   ₱{totalFee:0.00}\r\n";
            receipt += $"CASH TENDERED:  ₱{paymentAmount:0.00}\r\n";
            receipt += $"CHANGE:         ₱{change:0.00}\r\n";
            receipt += "================================\r\n";
            receipt += "   Thank you for parking with us!   ";

            txtReceipt.Text = receipt;
        }
    }

    
}
        