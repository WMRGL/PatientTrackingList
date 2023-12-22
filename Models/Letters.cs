using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    //[Table("Clinical_XP_MasterActivityTable", Schema = "dbo")]
    [Table("DictatedLetters", Schema = "dbo")]
    public class Letters
    {
        [Key]
        public int DotID { get; set; }
        public int RefID { get; set; }
        public DateTime? DateDictated { get; set; }
        public string? LetterFrom { get; set; }
        public string? LetterTo { get; set; }
    }
}
