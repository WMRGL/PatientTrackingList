using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    //[Table("Clinical_XP_MasterActivityTable", Schema = "dbo")]
    [Table("MasterActivityTable", Schema = "dbo")]
    public class Activity
    {
        [Key]
        public int RefID { get; set; }
        public string CLINICNO { get; set; }
        public int MPI { get; set; }
        public int WMFACSID { get; set; }
        public DateTime? REFERRAL_DATE { get; set; }
        public DateTime? ClockStartDate { get; set; }
        public DateTime? ClockStopDate { get; set; }
        public DateTime? DATE_SCHEDULED { get; set; }
        public string? TYPE { get; set; }
        public string? CLASS { get; set; }
        public string? PATIENT_TYPE { get; set; }
        public string? GC { get; set; }
        public string? REF_FAC { get; set; }
        public string? REF_PHYS { get; set; }
        public string? PATHWAY { get; set; }
        public string? COUNSELED { get; set; }
        public DateTime? BOOKED_DATE { get; set; }
        public string? COMPLETE {  get; set; }
        public string? REASON_FOR_REFERRAL { get; set; }
    }
}
