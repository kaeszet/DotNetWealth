using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    /// <summary>
    /// Static class for creating global alerts
    /// </summary>
    public static class GlobalAlert
    {
        /// <summary>
        /// Type of message which is used to colorize alerts
        /// </summary>
        private static string _type;
        /// <summary>
        /// Message to display in alert
        /// </summary>
        private static string _message;
        /// <summary>
        /// Quantity to display on dashboard
        /// </summary>
        private static int? _count;
        /// <summary>
        /// Static field to check is message changes
        /// </summary>
        public static string _old;
        /// <summary>
        /// Method to receive info from controller
        /// </summary>
        /// <param name="message">Message to display in alert</param>
        /// <param name="type">Type of message which is used to colorize alerts</param>
        public static void SendGlobalAlert(string message, string type)
        {
            _message = message;
            _type = type;
        }
        /// <summary>
        /// Method to receive quantity from controller
        /// </summary>
        /// <param name="count">Quantity to display on dashboard</param>
        public static void SendQuantity(int? count)
        {
            _count = count;
        }
        /// <summary>
        /// Method to set bootstrap class in global-info according to chosen type
        /// </summary>
        /// <returns>String with bootstrap class</returns>
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
        /// <summary>
        /// Method to show alert in view
        /// </summary>
        /// <returns>Message to display in alert</returns>
        public static string ShowGlobalAlert()
        {
            return _message;
        }
        /// <summary>
        /// Method to show quantity in dashboard
        /// </summary>
        /// <returns>Quantity to display on dashboard</returns>
        public static int? ShowQuantity()
        {
            return _count;
        }
        /// <summary>
        /// Method to set old value until later cheking
        /// </summary>
        /// <param name="old">Old value from view</param>
        public static void SetOld(string old)
        {
            _old = old;
        }
        
    }

}
