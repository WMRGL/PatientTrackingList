using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{ 
    [Table("ViewWaitingListHistory", Schema = "dbo")]
    public class WLHistory
    {
        [Key]
        public int ID { get; set; }
        public string CGU_No { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string Clinician { get; set; }
        public string ClinicianID { get; set; }
        public string Clinic { get; set; }
        public string ClinicID { get; set; }
        public string? OldClinician { get; set; }
        public string? OldClinicianID { get; set; }
        public string? OldClinic { get; set; }
        public string? OldClinicID { get; set; }
	    public DateTime ModifiedDate { get; set; }
        public string Type { get; set; }
    }
}
