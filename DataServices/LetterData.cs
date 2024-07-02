using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface ILetterData
    {
        public List<Letters> GetLetterList(int iRefID);
    }
    public class LetterData : ILetterData
    {
        private readonly DataContext _context;

        public LetterData(DataContext context)
        {
            _context = context;            
        }
             
        public List<Letters> GetLetterList(int iRefID)
        {
            IQueryable<Letters> LetterList = from l in _context.Letters
                         where l.RefID == iRefID
                         select l;

            return LetterList.ToList();
        }
        
    }
}
