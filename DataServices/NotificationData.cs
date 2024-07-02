
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface INotificationData
    {
        public string GetMessage();
    }
    public class NotificationData : INotificationData
    {
        private readonly DataContext _context;

        public NotificationData(DataContext context)
        {
            _context = context;            
        }

        public string GetMessage()
        {
            string message = "";

            IQueryable<Notifications> messageNotifications = _context.Notifications.Where(n => n.MessageCode == "PTLXOutage" && n.IsActive == true);

            if (messageNotifications.Count() > 0)
            {
                message = messageNotifications.First().Message;
            }

            return message;
        }
    }
}
