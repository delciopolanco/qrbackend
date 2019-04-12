using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.ViewModels.Notification
{
    public class SavePersonalNotificationType: NotificationBase
    {
        public SavePersonalNotificationType ()
        {
            FunctionName = "SavePersonalNotificationType";
            NotificationConfig = string.Empty;
        }
    }
}
