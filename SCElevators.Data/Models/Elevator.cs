using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCElevators.Data.Models
{
    public class Elevator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        public DateTimeOffset InsertedAt { get; set; }
        public DateTimeOffset LastSeen { get; set; }
    }
}