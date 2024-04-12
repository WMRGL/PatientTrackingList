using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class MetaData
    {
        private readonly DataContext _context;

        public MetaData(DataContext context)
        {
            _context = context;            
        }

        public IEnumerable<PTL> GetPTLList()
        {
            var ptl = from p in _context.PTL
                    where p.ClockStart != null && p.ClockStop == null && p.MPI != 67066
                    orderby p.ClockStart
                    select p;

            return ptl;
        }

        public IEnumerable<StaffMembers> GetStaffMemberList() 
        { 
            var staffMembers = from s in _context.StaffMembers
                               where s.InPost == true
                               select s;

            return staffMembers;
        }

        public List<StaffMembers> GetStaffTypeList(IEnumerable<StaffMembers> staffMembers, string sStaffType) 
        { 
            var staffList = staffMembers.Where(s => s.CLINIC_SCHEDULER_GROUPS == sStaffType).OrderBy(s => s.NAME).ToList();

            return staffList;
        }
        
        public PTL GetPTLEntryDetails(string ppi)
        {
            var RefDet = _context.PTL.FirstOrDefault(r => r.PPI == ppi);
            

            return RefDet;
        }

        public Activity GetReferralDetails(int iRefID)
        {
            var Referral = _context.Activity.FirstOrDefault(r => r.RefID == iRefID);

            return Referral;
        }

        public List<Activity> GetActivityList(string clinicNo)
        {
            var ActivityList = from r in _context.Activity
                               where r.REFERRAL_CLINICNO == clinicNo
                               select r;

            return ActivityList.ToList();
        }

        public List<Diary> GetDiaryList(int iRefID)
        {
            var DiaryList = from d in _context.Diary
                        where d.RefID == iRefID
                        select d;

            return DiaryList.ToList();
        }

        public List<Letters> GetLetterList(int iRefID)
        {
            var LetterList = from l in _context.Letters
                         where l.RefID == iRefID
                         select l;

            return LetterList.ToList();
        }
        
        public StaffMembers GetStaffMemberDetails(string username)
        {
            var staffMember = _context.StaffMembers.FirstOrDefault(s => s.EMPLOYEE_NUMBER == username);

            return staffMember;
        }

        public List<WaitingList> GetWaitingList()
        {
            var waitingList = from w in _context.WaitingList
                          orderby w.AddedDate
                          select w;            
            
            return waitingList.ToList();
        }

        public List<ClinicSlots> GetClinicSlotsList() 
        {
            var clinicSlots = from s in _context.ClinicSlots
                              orderby s.SlotDate
                              select s;

            return clinicSlots.ToList();
        }

        /*public string GetCGUNoByIntID(int intID)
        {
            string cguNo;

            if (_context.Patients.Where(p => p.INTID == intID).Count() > 0)
            {
                cguNo = _context.Patients.FirstOrDefault(p => p.INTID == intID).CGU_No;
            }
            else
            {
                cguNo = "Unknown";
            }

            return cguNo;
        }*/

        public bool GetIclinical(string username)
        {
            string strStaffType = _context.StaffMembers.FirstOrDefault(s => s.EMPLOYEE_NUMBER == username).CLINIC_SCHEDULER_GROUPS;

            if (strStaffType == "GC" || strStaffType == "Consultant" || strStaffType == "SpR") return true;

            return false;
        }

    }
}
