using System;
using Moq;
using System.Drawing;
using System.Drawing.Imaging;
using webApiTask.Helpers;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ImageHelperTest
    {
        [Test]
        public void CropImageTest()
        {
            var helper = new ImageHelper();

            var btm = new Bitmap(400, 400);

            using (Graphics graph = Graphics.FromImage(btm))
            {
                Rectangle blueRect = new Rectangle(0, 0, 400, 400);
                graph.FillRectangle(Brushes.Blue, blueRect);

                Rectangle redRect = new Rectangle(23, 23, 100, 140);
                graph.FillRectangle(Brushes.Red, redRect);
            }
            var img = (Bitmap)btm;
            var points = new string[4] { "23", "23", "100", "140" };

            var croppedImg = helper.CropImage(img, points);

            var newImg = new Bitmap(croppedImg);
            for (int x = 0; x < btm.Width; x++)
            {
                for (int y = 0; y < newImg.Height; y++)
                {
                    Color clr = newImg.GetPixel(x, y);

                    if (clr.ToArgb() != Color.Red.ToArgb())
                    {
                        Assert.Fail("Not correct cropping. Test fail(");
                    }
                    else
                    {
                        Assert.Pass("Test done!");
                    }

                }
            }

        }
    }
}
