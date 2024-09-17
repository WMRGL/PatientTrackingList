using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface IIcpData
    {
        public Icp GetIcp(int? refid);
        public IcpCancer GetIcpCancer(int? refid);
        public IcpGeneral GetIcpGeneral(int? refid);
       
    }
    public class IcpData : IIcpData
    {
        private readonly DataContext _context;


        public IcpData(DataContext context)
        {
            _context = context;
        }

        public Icp GetIcp(int? refid)
            {

            Icp IcpDetail = _context.Icps.FirstOrDefault(r => r.REFID == refid);
;

            return IcpDetail;
        }

        public IcpCancer GetIcpCancer(int? refid)
        {
            IcpCancer icpCancerDetail = _context.IcpCancers.FirstOrDefault(c => c.ICPID == refid);
            return icpCancerDetail;
        }

        public IcpGeneral GetIcpGeneral(int? refid) 
        {
    
            IcpGeneral icpGeneralDetail = _context.IcpGenerals.FirstOrDefault(g => g.ICPID == refid);
            Console.WriteLine(icpGeneralDetail);
            return icpGeneralDetail;
        }
    }
}
