using System;

namespace SCElevators.Client.DTOs
{
    public class Elevator
    {
        public Int32 Number { get; set; }
        public string OwnerName { get; set; }
        public string Location { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationZip { get; set; }
        public string County { get; set; }
        public string Status { get; set; }
        public DateTime? NextInspectionDue { get; set; }
        public string NextInspectionDueOther { get; set; }
        public string MachineType { get; set; }
        public string Manufacturer { get; set; }
        public string UnitType { get; set; }
    }
}