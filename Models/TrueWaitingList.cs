using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    [Keyless]
    [Table("ViewPatientReferralDetails", Schema = "dbo")]
    public class TrueWaitingList
    {
        [Key]
        public int refid { get; set; }
        public int MPI { get; set; }
        public string CGU_No { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Complete { get; set; }
        public DateTime? RefDate { get; set; }
        public DateTime? ClockStartDate { get; set; }
        public DateTime? ClockStopDate { get; set; }
        public string? Status_Admin {  get; set; }
        public string?PATHWAY { get; set; }
    }
}
