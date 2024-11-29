using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class NotificationDetailsVM : BaseVM
    {
        [DisplayName("Recipient Name")]
        public UserDetailsVM RecipientName { get; set; }

        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Date Sent")]
        public DateTime DateSent { get; set; }

        [DisplayName("Is Read")]
        public bool IsRead { get; set; }
    }
}
