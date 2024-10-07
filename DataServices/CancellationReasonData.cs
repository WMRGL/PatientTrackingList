using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface ICancellationReasonData
    {
        public IEnumerable<CancellationReason> GetCancellationReasonsList();

    }
    public class CancellationReasonData : ICancellationReasonData
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public CancellationReasonData(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<CancellationReason> GetCancellationReasonsList()
        {
            IEnumerable<CancellationReason> cancellationReasons = _context.CancellationReasons.AsNoTracking();
            return cancellationReasons;
        }
    }
}
