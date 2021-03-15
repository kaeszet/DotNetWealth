using DotNetWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    /// <summary>
    /// A class to generate <c>Item</c>'s ItemCode
    /// </summary>
    public static class ItemCodeGenerator
    {
        /// <summary>
        /// Method which can be used in controllers to invoke code generation 
        /// </summary>
        /// <param name="item"><c>Item</c> object</param>
        /// <param name="user">Name of logged in user or user from DB</param>
        /// <returns>Invoke private method which returns string with code</returns>
        public static string Generate(Item item, string user)
        {
            return GenerateItemCode(item, user);
        }
        /// <summary>
        /// Private method to generate item code
        /// </summary>
        /// <param name="item"><c>Item</c> object</param>
        /// <param name="user">Name of logged in user or user from DB</param>
        /// <returns>Returns string with code</returns>
        private static string GenerateItemCode(Item item, string user)
        {
            StringBuilder sb = new StringBuilder();

            if (item.State == ItemState.Damaged) sb.Append("uszk");

            sb.Append(item.Id);
            sb.Append(string.IsNullOrEmpty(item.Producer) ? "" : item.Producer.Length >= 3 ? item.Producer.ToLower().Substring(0, 3) : item.Producer.ToLower());
            sb.Append(string.IsNullOrEmpty(item.Model) ? "" : item.Model.Length >= 3 ? item.Model.ToLower().Substring(0, 3) : item.Model.ToLower());
            sb.Append(item.Name.Length >= 3 ? item.Name.ToUpper().Substring(0, 3) : item.Name.ToUpper());
            sb.Append(item.UserId != null ? $"{user}-" : "0-");
            sb.Append(item.WarehouseId != null ? $"{item.WarehouseId}-" : "0-");
            sb.Append(item.ExternalId != null ? item.ExternalId.ToString() : "0");

            return sb.ToString();
        }
    }
}
