using System;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the view describing the application error
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Property that stores the query ID
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// Method to display the query ID
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
