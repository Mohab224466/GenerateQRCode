using GenerateQRCode.Models;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace GenerateQRCode.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult CreateQRCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateQRCode(QRCodeModel qRCode)
        {
            using (QRCodeGenerator QrGenerator = new QRCodeGenerator())
            using (QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qRCode.QRText, QRCodeGenerator.ECCLevel.Q))
            using (QRCode QrCode = new QRCode(QrCodeInfo))
            {
                Bitmap QrBitmap = QrCode.GetGraphic(60);
                byte[] BitmapArray = QrBitmap.BitmapToByteArray();
                string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                ViewBag.QrCodeUri = QrUri;

            }
            return View();
        }
    }

    //Extension method to convert Bitmap to Byte Array
    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
