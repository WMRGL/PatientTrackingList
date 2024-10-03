using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    
    [Table("ListCancellationReason", Schema = "dbo")]
    public class CancellationReason
    {
        [Key]
        public int Id { get; set; }
        public string? Reason { get; set; }
    }
}
