using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    [Keyless]
    [Table("Clinicians_WaitingList", Schema="dbo")]
    public class WaitingList
    {
        public int? IntID { get; set; }
        public string? ClinicianID { get; set; }
        public string? ClinicID {  get; set; }
        public string? Comment { get; set; }
        public string? Instructions { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string? ToBeSeenBy { get; set; }
        public bool isChildRequired { get; set; }
        [NotMapped]
        public string? CGU_No {  get; set; }
    }
    
}
