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
        private static int? _count;
        private static string _type;

        public static string _old;

        public static void SendGlobalAlert(string message, string type)
        {
            _message = message;
            _type = type;
        }

        public static void SendQuantity(int count)
        {
            _count = count;
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
        public static int? ShowQuantity()
        {
            return _count;
        }

        public static void SetOld(string old)
        {
            _old = old;
        }
        
    }

}
