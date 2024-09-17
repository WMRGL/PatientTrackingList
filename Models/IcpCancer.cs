using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    [Table("ICP_Cancer", Schema = "dbo")]
    public class IcpCancer
    {
        [Key] 
        public int? ICPID {  get; set; }
        public string? ActRefInfo {  get; set; } 

    }
}
