using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class LetterData
    {
        private readonly DataContext _context;

        public LetterData(DataContext context)
        {
            _context = context;            
        }
             


        public List<Letters> GetLetterList(int iRefID)
        {
            var LetterList = from l in _context.Letters
                         where l.RefID == iRefID
                         select l;

            return LetterList.ToList();
        }
        
    }
}
