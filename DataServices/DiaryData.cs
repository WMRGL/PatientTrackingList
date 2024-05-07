using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class DiaryData
    {
        private readonly DataContext _context;

        public DiaryData(DataContext context)
        {
            _context = context;            
        }
             

        public List<Diary> GetDiaryList(int iRefID)
        {
            var DiaryList = from d in _context.Diary
                        where d.RefID == iRefID
                        select d;

            return DiaryList.ToList();
        }
                
    }
}
