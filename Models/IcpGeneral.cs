using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    [Table("ICP_General", Schema = "dbo")]
    public class IcpGeneral
    {
        [Key]
        public int? ICPID { get; set; }
        public int? TreatPath { get; set; }
        public string? TreatPathBy {  get; set; }
		public string? TriageNotes { get; set; }

	}
}
