using PatientTrackingList.Data;
using PatientTrackingList.Models;
using Microsoft.EntityFrameworkCore;

namespace PatientTrackingList.DataServices
{

    interface IListStatusAdminData
    {
        public IEnumerable<ListStatusAdmin> GetStatusAdminList();
    }
    public class ListStatusAdminData : IListStatusAdminData
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public ListStatusAdminData(DataContext context) 
        {
            _context = context;
        }

        public IEnumerable<ListStatusAdmin> GetStatusAdminList()
        {
            IEnumerable<ListStatusAdmin> listStatusAdmins = _context.ListStatusAdmins.AsNoTracking();
            return listStatusAdmins;
        }
    }
}
