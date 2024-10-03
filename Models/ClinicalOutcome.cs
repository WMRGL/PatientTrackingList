using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    [Keyless]
    [Table("CLIN_OUTCOMES", Schema = "dbo")]
    public class ClinicalOutcome
    {
        public string? CLINIC_OUTCOME {  get; set; }
    }
}
