using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public interface ITrueWaitingListData
    {
        IQueryable<TrueWaitingList> GetTrueWaitingList();
    }

    public class TrueWaitingListData : ITrueWaitingListData
    {
        private readonly DataContext _context;

        public TrueWaitingListData(DataContext context)
        {
            _context = context;
        }

        public IQueryable<TrueWaitingList> GetTrueWaitingList()
        {
            return _context.TrueWaitingLists
                .Where(w => w.Complete == "Active"
                         && w.RefDate != null
                         && w.ClockStartDate != null
                         && w.Status_Admin != "Complete"
                         && w.PATHWAY != null)
                .AsQueryable();
        }
    }
}



