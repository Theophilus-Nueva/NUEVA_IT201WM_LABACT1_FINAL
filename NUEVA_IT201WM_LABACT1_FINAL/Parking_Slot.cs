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
        public bool IsOccupied => !string.IsNullOrEmpty(VehicleType);

        public string VehicleType { get; set; }
        public int NumberOfHoursParked { get; set; }
        public decimal ParkingRate { get; set; }

        public decimal FixedServiceCharge { get; } = 20m;


        public decimal CalculateTotalFee()
        {
            if (!IsOccupied) return 0;
            return FixedServiceCharge + (NumberOfHoursParked * ParkingRate);
        }

        public void VacateSlot()
        {
            VehicleType = null;
            NumberOfHoursParked = 0;
            ParkingRate = 0m;
        }
    }
}
