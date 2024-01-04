using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    [Table("MasterPatientTable", Schema ="dbo")]
    public class Patients
    {
        [Key]
        public int MPI {  get; set; }
        public int INTID { get; set; }
        public string CGU_No { get; set; }
    }
}
