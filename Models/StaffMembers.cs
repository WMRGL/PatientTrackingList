using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    //[Table("18rtt_ptl", Schema = "dbo")]
    [Table("STAFF", Schema = "dbo")]
    public class StaffMembers
    {
        [Key]
        public string STAFF_CODE { get; set; }
        public string NAME { get; set; }
        public string EMPLOYEE_NUMBER { get; set; }
        public string CLINIC_SCHEDULER_GROUPS { get; set; }
        public bool InPost { get; set; }
    }
}
