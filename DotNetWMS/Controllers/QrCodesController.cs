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

namespace DotNetWMS.Controllers
{
    public class QrCodesController : Controller
    {
        // GET: QrCodesController
        public ActionResult Index()
        {
            return PartialView("_QrCodePartial");
        }

        public IActionResult ShowQRCode(string url)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitmap = qrCode.GetGraphic(20))
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    TempData["QRCode"] = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
            return PartialView("_QrCodePartial");

        }

    }   
}
