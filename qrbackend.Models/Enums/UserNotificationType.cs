
using System.ComponentModel;

namespace qrbackend.Models.Enums
{
    public enum UserNotificationType
    {
        [Description("email")]
        Email = 1,
        [Description("pushNotification")]
        PushNotification = 2,
        [Description("sms")]
        Sms = 3
    }
}
