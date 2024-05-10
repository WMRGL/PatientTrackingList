using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    //[Table("18rtt_ptl", Schema = "dbo")]
    [Table("Notifications", Schema = "dbo")]
    public class Notifications
    {
        [Key]
        public int ID { get; set; }
        public string MessageCode { get; set; }
        public string Message {  get; set; }
        public bool IsActive { get; set; }
    }
}
