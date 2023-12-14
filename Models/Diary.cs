using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientTrackingList.Models
{
    [Table("Clinical_XP_PatientDiary", Schema = "dbo")]
    public class Diary
    {
        [Key]
        public int DiaryId { get; set; }
        public int WMFACSID { get; set; }
        public DateTime? DiaryDate { get; set; }
        public string? DiaryWith {  get; set; }
        public string? DiaryAction { get; set; }
        public string? DiaryText { get; set; }
        public string? DocCode { get; set; }
        public int RefID { get; set; }
    }
}
