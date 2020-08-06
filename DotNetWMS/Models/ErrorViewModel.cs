using System;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the view describing the application error
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
