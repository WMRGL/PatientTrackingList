using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    
    [Table("ViewPatientAppointmentDetails", Schema = "dbo")]
    public class CancellationReason
    {
        [Key]
        public string CGU_No { get; set; }
        public int ReferralRefID { get; set; }
        public string? Attendance { get; set; }
    }
}
