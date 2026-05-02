using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUEVA_IT201WM_LABACT1_FINAL
{
    public class Parking_Slot
    {
        public string SlotID { get; set; }
        public bool IsOccupied => !string.IsNullOrEmpty(PlateNumber);
        public string PlateNumber { get; set; }
        public string VehicleType { get; set; }
        public int NumberOfHoursParked { get; set; }
        public string DiscountType { get; set; }
        public decimal ParkingRate
        {
            get
            {
                switch (VehicleType)
                {
                    case "Motorcycle":
                        return 30m;
                    case "Car":
                        return 50m;
                    case null:
                    case "Van":
                        return 70m; 
                    default:
                        return 0; 
                }
            }
        }


        public decimal FixedServiceCharge { get; } = 20m;

        public decimal GetDiscountDeduction()
        {
            decimal subtotal = GetSubtotalFee(); 

            switch (DiscountType)
            {
                case "Senior Citizen":
                    return subtotal * 0.20m;
                case "Employee":
                    return subtotal * 0.20m;
                default:
                    return 0m;
            }
        }

        public Parking_Slot(string assignedSlotId)
        {
            SlotID = assignedSlotId;
        }


        public decimal GetStandardFee()
        {
            return NumberOfHoursParked * ParkingRate;
        }

        public decimal GetOvertimeFee()
        {
            if (NumberOfHoursParked > 8)
            {
                return 50m;
            }
            return 0m;
        }

        public decimal GetSubtotalFee()
        {
            return GetStandardFee() + FixedServiceCharge + GetOvertimeFee();
        }

        public decimal GetTotalFee()
        {
            if (!IsOccupied) return 0;

            return GetSubtotalFee() - GetDiscountDeduction();
        }

        public void VacateSlot()
        {
            PlateNumber = null;
            VehicleType = null;
            NumberOfHoursParked = 0;
        }
    }
}
