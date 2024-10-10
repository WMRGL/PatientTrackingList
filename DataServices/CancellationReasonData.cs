using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface ICancellationReasonData
    {
        public CancellationReason GetCancellationReason(string? cguno);

    }
    public class CancellationReasonData : ICancellationReasonData
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public CancellationReasonData(DataContext context)
        {
            _context = context;
        }

        public CancellationReason GetCancellationReason(string? cguno)
        {
            CancellationReason cancelReason = _context.CancellationReasons.FirstOrDefault(w => w.CGU_No == cguno);
            return cancelReason;
        }
    }
}
