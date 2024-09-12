using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PatientTrackingList.Models
{
    [Table("ICP", Schema = "dbo")]
    public class Icp
    {
        [Key]
        public int REFID { get; set; }
        public int ICPID { get; set; }
    }
}
