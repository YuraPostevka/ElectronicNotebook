using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace webApiTask.Helpers
{
    public class ImageHelper
    {
        public void SaveImage(int id, string base64String)
        {
            var path = ConfigurationManager.AppSettings["directoryRoot"];
            var directoryForUser = path + id.ToString();
            try
            {
                if (!Directory.Exists(directoryForUser))
                {
                    Directory.CreateDirectory(directoryForUser);

                }
                base64String = base64String.Replace("data:image/png;base64,", "");
                var img = Base64ToImage(base64String);
                img.Save(directoryForUser + "/profile.png", ImageFormat.Png);

            }
            catch
            {

            }

        }
        public string GetImage(int id)
        {
            var directory = ConfigurationManager.AppSettings["CDNBase"];

            return directory + id.ToString() + "/profile.png";
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
        public void CropAndSaveImage(int userId, HttpPostedFileBase image, string points, string zoom)
        {

            var path = ConfigurationManager.AppSettings["directoryRoot"];
            var directoryForUser = path + userId.ToString();

            var pointsArray = points.Split(',');
            zoom = zoom.Replace(",", ".");
            var doubleZoom = Convert.ToDouble(zoom);
            var originalImage = Image.FromStream(image.InputStream, true, true);

            var croppedImage = CropImage(originalImage, pointsArray);

            croppedImage.Save(directoryForUser + "/profile.png", ImageFormat.Png);

        }
        public Image CropImage(Image originalImage, string[] points)
        {

            var x = Convert.ToInt32(points[0]);
            var y = Convert.ToInt32(points[1]);
            var w = Convert.ToInt32(points[2]);
            var h = Convert.ToInt32(points[3]);
            Rectangle rect = new Rectangle(x, y, w, h);


            Bitmap bmpImage = new Bitmap(originalImage);

            return bmpImage.Clone(rect, bmpImage.PixelFormat);
        }
    }
}