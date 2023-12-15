using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    //[Table("18rtt_ptl", Schema = "dbo")]
    [Table("PTL", Schema = "dbo")]
    public class PTL
    {
        [Key]
        public string? PPI { get; set; }
        public int? MPI { get; set; }
        public int? WMFACSID { get; set; }
        public string? CGUNo { get; set; }
        public string? ReferralType { get; set; }
        public DateTime? ReferralDate { get; set; }
        public int? RefID { get; set; }
        public DateTime? ClockStart { get; set; }
        public DateTime? ClockStop { get; set; }
        public string? PatientName { get; set; }
        public string? PtPc { get; set; }
        public string? OrgCode { get; set; }
        public string? OrgName { get; set; }
        public string? ReferralBy { get; set; }
        public string? ReferralReason { get; set; }
        public string? ReferralConsultant { get; set; }
        public string? ReferralGC { get; set; }
        public string? NHSNo { get; set; }
        public DateTime? PtDOB { get; set; }
        public string? Class {  get; set; }
        public string? BreachReason {  get; set; }
        public DateTime? TCIDate { get; set; }
        public string? ApptType { get; set; }
        public string? REF_PHYS { get; set; }
        public string? GPPc {  get; set; }
    }
}
