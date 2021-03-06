using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    public static class GlobalAlert
    {
        private static string _message;
        private static string _type;

        public static string _old;

        public static void SendGlobalAlert(string message, string type)
        {
            _message = message;
            _type = type;
        }

        public static string SetGlobalAlertType()
        {
            if (!string.IsNullOrEmpty(_type))
            {
                return _type switch
                {
                    "primary" => $"alert alert-primary global-info shadow alert-dismissible fade show",
                    "secondary" => $"alert alert-secondary global-info shadow alert-dismissible fade show",
                    "success" => $"alert alert-success global-info shadow alert-dismissible fade show",
                    "danger" => $"alert alert-danger global-info shadow alert-dismissible fade show",
                    "warning" => $"alert alert-warning global-info shadow alert-dismissible fade show",
                    "info" => $"alert alert-info global-info shadow alert-dismissible fade show",
                    "light" => $"alert alert-light global-info shadow alert-dismissible fade show",
                    "dark" => $"alert alert-dark global-info shadow alert-dismissible fade show",
                    _ => $"alert alert-primary global-info shadow alert-dismissible fade show",
                };
            }

            return $"alert alert-primary global-info shadow alert-dismissible fade show";
            
        }
        public static string ShowGlobalAlert()
        {
            return _message;
        }

        public static void SetOld(string old)
        {
            _old = old;
        }
        //private static IDictionary<string, string> NotificationKey = new Dictionary<string, string>
        //{
        //    { "Info", "App.Notifications.Info" }
        //};
        //public static void SendGlobalAlert(this ControllerBase controller, string message, string notificationType) 
        //{
        //    string NotificationKey = getNotificationKeyByType(notificationType);
        //    ICollection<string> messages = controller.TempData[NotificationKey] as ICollection<string>;
        //    //TempData["SuccessInfo"] = message;
        //}
        //public static IEnumerable<string> GetNotifications(this HtmlHelper htmlHelper, string notificationType)
        //{
        //    string NotificationKey = getNotificationKeyByType(notificationType);
        //    return htmlHelper.ViewContext.Controller.TempData[NotificationKey] as ICollection<string> ?? null;
        //}
        //private static string getNotificationKeyByType(string notificationType)
        //{
        //    try
        //    {
        //        return NotificationKey[notificationType];
        //    }
        //    catch (IndexOutOfRangeException e)
        //    {
        //        ArgumentException exception = new ArgumentException("Key is invalid", "notificationType", e);
        //        throw exception;
        //    }
        //}
    }

    public static class NotificationType
    {
        public const string INFO = "Info";
    }
}
