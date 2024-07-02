using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface IDiaryData
    {
        public List<Diary> GetDiaryList(int iRefID);
    }
    public class DiaryData : IDiaryData
    {
        private readonly DataContext _context;

        public DiaryData(DataContext context)
        {
            _context = context;            
        }
             

        public List<Diary> GetDiaryList(int iRefID)
        {
            IQueryable<Diary> DiaryList = from d in _context.Diary
                        where d.RefID == iRefID
                        select d;

            return DiaryList.ToList();
        }
                
    }
}
