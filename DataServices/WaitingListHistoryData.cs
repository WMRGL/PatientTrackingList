using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface IWaitingListHistoryData
    {
        public IEnumerable<WLHistory> GetWaitingListHistory(DateTime startDate, DateTime endDate);
    }
    public class WaitingListHistoryData : IWaitingListHistoryData
    {
        private readonly DataContext _context;

        public WaitingListHistoryData(DataContext context)
        {
            _context = context;            
        }
        
        public IEnumerable<WLHistory> GetWaitingListHistory(DateTime startDate, DateTime endDate) 
        {
            IQueryable<WLHistory> wlhistory = from s in _context.WaitingListHistory
                                              where s.ModifiedDate >= startDate && s.ModifiedDate <= endDate
                                            orderby s.ModifiedDate
                                              select s;

            return wlhistory; //.ToList();
        }
                
    }
}
