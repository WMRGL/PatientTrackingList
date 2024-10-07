using PatientTrackingList.Data;
using PatientTrackingList.Models;
using Microsoft.EntityFrameworkCore;

namespace PatientTrackingList.DataServices
{
    interface IClinicalOutcomesData
    {
        public IEnumerable<ClinicalOutcome> GetClinicalOutcomesList();

    }

    public class ClicnicalOutcomesData : IClinicalOutcomesData
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public ClicnicalOutcomesData(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<ClinicalOutcome> GetClinicalOutcomesList() 
        {
            IEnumerable<ClinicalOutcome> clinicalOutcomes = _context.ClinicalOutcomes.AsNoTracking();
            return clinicalOutcomes;
        }
    }


}

