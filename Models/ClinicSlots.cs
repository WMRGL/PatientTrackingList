using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    //[Table("Clinical_XP_MasterActivityTable", Schema = "dbo")]
    [Table("ClinicSlotsAll", Schema = "dbo")]
    public class ClinicSlots
    {
        [Key]
        public int SlotID { get; set; }
        public string ClinicianID { get; set; }
        public string ClinicID { get; set; }        
        public DateTime? SlotDate { get; set; }
        public DateTime? SlotTime { get; set; }
        public string? SlotStatus{ get; set; }
        public int Duration { get; set; }
    }
}
