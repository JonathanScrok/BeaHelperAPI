using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using BeaHelper.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using QRCoder;


namespace SyrusVoluntariado.Controllers
{
    public class QRCodeController : ControllerBase
    {
        #region Gera QrCode em Bytes
        /// <summary>
        /// Gera QrCode.
        /// </summary>
        [HttpGet("{qrTexto}")]
        public IActionResult Index(string qrTexto)
        {
            try
            {
                string nomearquivo = Regex.Replace(qrTexto, ".*visualizar/", "");
                nomearquivo = "evento" + nomearquivo;
                QRCodeGenerator qrGerador = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGerador.CreateQrCode(qrTexto, QRCodeGenerator.ECCLevel.Q);

                qrCodeData.SaveRawData("wwwroot/qrcode/arquivo-" + nomearquivo + ".qrr",
                       QRCodeData.Compression.Uncompressed);
                QRCodeData qrCodeData1 = new QRCodeData("wwwroot/qrcode/arquivo-" + nomearquivo + ".qrr",
                    QRCodeData.Compression.Uncompressed);
                //DeleteArquivo("wwwroot/qrcode/arquivo-" + nomearquivo + ".qrr");
                QRCode qrCode = new QRCode(qrCodeData1);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                //ViewBag.QrCodeByte = BitmapToBytes(qrCodeImage);
                return Ok(BitmapToBytes(qrCodeImage));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return BadRequest(ex);
            }

        }

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public void DeleteArquivo(string arquivo)
        {
            //arquivo = "wwwroot/qrr/arquivo-1254.qrr";
            ValidaArquivo.DeletaArquivo(arquivo);
        }


        [HttpGet("/ToBase64/{caminho}")]
        public IActionResult ToBase64(string caminho)
        {
            try
            {
                using (Image image = Image.FromFile(caminho))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return Ok(base64String);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return BadRequest(ex);
            }
        }

        #endregion

    }
}