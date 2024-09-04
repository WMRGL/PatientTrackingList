using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public interface ITrueWaitingListData
    {
        public IQueryable<TrueWaitingList> GetTrueWaitingList(bool? includeAll = false);

    }

    public class TrueWaitingListData : ITrueWaitingListData
    {
        public IQueryable<TrueWaitingList> GetTrueWaitingList()
        {
            return GetTrueWaitingList(false); // Call the overloaded version with default value
        }
        private readonly DataContext _context;

        public TrueWaitingListData(DataContext context)
        {
            _context = context;
        }

        public IQueryable<TrueWaitingList> GetTrueWaitingList(bool? includeAll = false)
        {
            var query = _context.TrueWaitingLists
            .Where(w => w.Complete == "Active"
                && w.RefDate != null
                && w.ClockStartDate != null
                && w.Status_Admin != "Complete"
                && w.PATHWAY != null);

            if (!includeAll.GetValueOrDefault())
            {
                query = query.Where(w => w.RefDate >= DateTime.Now.AddYears(-2));
            }

            return query.AsQueryable();
        }
    }
        

}



