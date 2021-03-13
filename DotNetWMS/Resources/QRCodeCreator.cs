using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    /// <summary>
    /// A class to create and show QRCodes in views
    /// </summary>
    public static class QRCodeCreator
    {
        /// <summary>
        /// A method to create and show QRCodes in views
        /// </summary>
        /// <param name="url">URL address to encode</param>
        /// <returns>String with code which is necessary to present it in views</returns>
        public static string ShowQRCode(string url)
        {
            string result;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using Bitmap bitmap = qrCode.GetGraphic(20);
                bitmap.Save(ms, ImageFormat.Png);
                result = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
            }
            return result;

        }
    }
}
