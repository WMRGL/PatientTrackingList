using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    [Keyless]
    [Table("ListStatusAdmin", Schema = "dbo")]
    public class ListStatusAdmin
    {
        public string? Status_Admin { get; set; }
        public Int16? Sequence { get; set; }
    }
}
