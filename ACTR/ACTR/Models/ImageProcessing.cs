using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace ACTR
{
    /// <summary>
    /// Image processing stuff goes here
    /// </summary>
    public class ImageProcessing
    {
        /// <summary>
        /// Working directory for saving processed images
        /// </summary>
        private static string imagesWorkingDirectory = Helper.GetImagesDirectory();

        public int radius;
        public Point center;
        /// <summary>
        /// Processes source image
        /// </summary>
        public DateTime ProcessImage(string path)
        {
            // Load source image
            Image<Bgr, Byte> sourceImage;
            sourceImage = LoadImage(path);

            // Get gray image
            Image<Gray, Byte> grayImage;
            grayImage = GetGrayImage(sourceImage);

            // Get blurred image, noise reduction
            Image<Gray, Byte> blurredImage;
            blurredImage = GetBlurredImage(grayImage);

            // Get clock face
            Image<Gray, Byte> circle;
            circle = GetCircle(blurredImage);

            // Get gradient
            Image<Gray, Single> gradientImage;
            gradientImage = GetGradient(circle);

            var lines = GetLines(gradientImage,radius);

            // Read time
            DateTime time =  GetTime(lines, center);
            return time;
        }

        private DateTime GetTime(LineSegment2D[] lines, Point center)
        {
            int hour, minute;
            LineSegment2D minuteLine, hourLine,secLine;
            //secLine = lines.OrderByDescending(l => l.Length).First();
            minuteLine = lines.OrderByDescending(l => l.Length).First(); //lines.OrderByDescending(l => l.Length).ToArray()[1];
            hourLine = lines.OrderByDescending(l => l.Length).Last();

            double distanceX, distanceY,distance;
            List<double> distances = new List<double>();

            //get minute xy
            Point minuteMaxPoint;

            distanceX = center.X - minuteLine.P1.X;
            distanceY = center.Y - minuteLine.P1.Y;
            distance = Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2)));
            distances.Add(distance);
            distanceX = center.X - minuteLine.P2.X;
            distanceY = center.Y - minuteLine.P2.Y;
            distance = Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2)));
            distances.Add(distance);

            if (distances[0]>distances[1])
            {
                minuteMaxPoint = minuteLine.P1;
            }
            else
            {
                minuteMaxPoint = minuteLine.P2;
            }

            //get hourxy
            Point hourMaxPoint;
            distances = new List<double>();

            distanceX = center.X - hourLine.P1.X;
            distanceY = center.Y - hourLine.P1.Y;
            distance = Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2)));
            distances.Add(distance);
            distanceX = center.X - hourLine.P2.X;
            distanceY = center.Y - hourLine.P2.Y;
            distance = Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2)));
            distances.Add(distance);

            if (distances[0] > distances[1])
            {
                hourMaxPoint = hourLine.P1;
            }
            else
            {
                hourMaxPoint = hourLine.P2;
            }
            ////get smaxxy
            //Point secondMaxPoint;
            //distances = new List<double>();

            //distanceX = center.X - secLine.P1.X;
            //distanceY = center.Y - secLine.P1.Y;
            //distance = Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2)));
            //distances.Add(distance);
            //distanceX = center.X - secLine.P2.X;
            //distanceY = center.Y - secLine.P2.Y;
            //distance = Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2)));
            //distances.Add(distance);

            //if (distances[0] > distances[1])
            //{
            //    secondMaxPoint = secLine.P1;
            //}
            //else
            //{
            //    secondMaxPoint = secLine.P2;
            //}


            var matan2 = Math.Atan2(minuteMaxPoint.Y-center.Y, minuteMaxPoint.X-center.X) * 180 / Math.PI+90;
            var hatan2 = Math.Atan2(hourMaxPoint.Y - center.Y, hourMaxPoint.X - center.X) * 180 / Math.PI+90;
            //var satan2 = Math.Atan2(secondMaxPoint.Y - center.Y, secondMaxPoint.X - center.X) * 180 / Math.PI + 90;

            //if (satan2 < 0)
            //{
            //    satan2 = 360 + satan2;
            //}
            if (matan2<0)
            {
                matan2 = 360 + matan2;
            }
            if (hatan2 < 0)
            {
                hatan2 = 360 + hatan2;
            }


            hour = this.ConvertRange(0, 360, 0, 12, (int)hatan2);
            minute = this.ConvertRange(0, 360, 0, 59, (int)matan2);

            return new DateTime(1, 1, 1,hour, minute, 0);
        }

        public int ConvertRange(
                int originalStart, int originalEnd, // original range
                int newStart, int newEnd, // desired range
                int value) // value to convert
        {
            double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
            return (int)(newStart + ((value - originalStart) * scale));
        }

        private static LineSegment2D[] GetLines(Image<Gray, float> gradientImage, int radius)
        {
            var source = new Image<Gray, Byte>(Path.Combine(imagesWorkingDirectory, "gradient.png"));

            Image<Rgb, Byte> lineImage = source.Convert<Rgb, Byte>();

            LineSegment2D[] lines = null;
            var minrad = radius;
            while (lines == null || lines.Count() < 2)
            {
                if (minrad == 0)
                {
                    throw new ArgumentException("Unable to find 3 lines.");
                }
                lines = CvInvoke.HoughLinesP(
                  source,
                  1, //Distance resolution in pixel-related units
                  Math.PI / 720.0, //Angle resolution measured in radians.
                  60, //threshold
                  minrad, //min Line width
                  0); //gap between lines
                minrad = minrad / 2;
            }


            lines = lines.Where(l => l.Length < radius+radius*0.2).OrderByDescending(l=>l.Length).Take(2).ToArray();
            foreach (var l in lines)
            {
                lineImage.Draw(l, new Rgb(Color.Green), 2, LineType.AntiAlias);
            }

            //var ll = source.HoughLinesBinary(1, Math.PI / 720.0, 122, radius/5, 0);

            //foreach (var l in ll)
            //{
            //    foreach (var L in l)
            //    {
            //        lineImage.Draw(L, new Rgb(Color.Green), 2, LineType.AntiAlias);
            //    }

            //}

            var filePath = Path.Combine(imagesWorkingDirectory, "lines.png");
            CvInvoke.Imwrite(filePath, lineImage);
            return lines;
        }

        /// <summary>
        /// Reads time from preprocessed clock face
        /// </summary>
        public static void GetTime()
        {

        }

        /// <summary>
        /// Loads selected image
        /// </summary>
        /// <param name="path"></param>
        public static Image<Bgr, Byte> LoadImage(string path)
        {
            Image<Bgr, Byte> sourceImage;
            string filePath;

            sourceImage = new Image<Bgr, Byte>(path).Resize(800, 600, Inter.Linear, true);

            filePath = Path.Combine(imagesWorkingDirectory, "source.png");

            if (Directory.Exists(imagesWorkingDirectory))
            {
                Directory.Delete(imagesWorkingDirectory, true);
            }
            Directory.CreateDirectory(imagesWorkingDirectory);

            CvInvoke.Imwrite(filePath, sourceImage);

            return sourceImage;
        }

        /// <summary>
        /// Gets and saves gray image
        /// </summary>
        public static Image<Gray, Byte> GetGrayImage(Image<Bgr, Byte> sourceImage)
        {
            Image<Gray, Byte> grayImage;
            grayImage = new Image<Gray, byte>(sourceImage.Width, sourceImage.Height);
            string filePath;

            CvInvoke.CvtColor(sourceImage, grayImage, ColorConversion.Bgr2Gray);

            filePath = Path.Combine(imagesWorkingDirectory, "gray.png");
            CvInvoke.Imwrite(filePath, grayImage);

            return grayImage;
        }

        /// <summary>
        /// Gets and saves blurred image
        /// </summary>
        public static Image<Gray, Byte> GetBlurredImage(Image<Gray, Byte> grayImage)
        {
            Image<Gray, Byte> blurredImage;
            blurredImage = new Image<Gray, Byte>(grayImage.Width, grayImage.Height);
            string filePath;

            Size ksize = new Size { Height = 3, Width = 3 };
            CvInvoke.GaussianBlur(grayImage, blurredImage, ksize, 11);

            filePath = Path.Combine(imagesWorkingDirectory, "blurred.png");
            CvInvoke.Imwrite(filePath, blurredImage);

            return blurredImage;
        }


        /// <summary>
        /// Gets and saves gradient of an image
        /// </summary>
        public static Image<Gray, Single> GetGradient(Image<Gray, Byte> blurredImage)
        {
            Image<Gray, Single> blurredSingle = blurredImage.Convert<Gray, Single>();
            Image<Gray, Single> gradientImage;
            string filePath;

            gradientImage = blurredImage.Laplace(3);
            //gradientImage = (blurredSingle.Sobel(1, 0, 3)).Add(blurredSingle.Sobel(0, 1, 3)).AbsDiff(new Gray(0.0));
                        
            filePath = Path.Combine(imagesWorkingDirectory, "gradient.png");
            CvInvoke.Imwrite(filePath, gradientImage);

            return gradientImage;
        }


        /// <summary>
        /// Gets and saves image of an isolated clock face
        /// </summary>
        public Image<Gray, Byte> GetCircle(Image<Gray, Byte> blurred)
        {
            string filePath;

            int minRadius = blurred.Width;
            CircleF[] circles = null;
            while (circles == null || circles.Count()<=0)
            {
                if (minRadius == 0)
                {
                    throw new ArgumentException("Unable to find circle on image");
                }
                circles = CvInvoke.HoughCircles(blurred, HoughType.Gradient, 1, blurred.Rows / 8, 200, 100, minRadius, 0);
                minRadius = minRadius / 2;

            }
            PointF center = new PointF { X = blurred.Width / 2, Y = blurred.Height / 2 };
            double distanceX, distanceY, distance;
            Dictionary<double, CircleF> circlesData = new Dictionary<double, CircleF>();
            foreach (var c in circles)
            {
                distanceX = center.X - c.Center.X;
                distanceY = center.Y - c.Center.Y;
                distance = Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2)));
                circlesData.Add(distance, c);
            }

            CircleF clockCircle = circlesData.OrderByDescending(c => c.Value.Radius).Take(5).OrderBy(c => c.Key).First().Value;

            Image<Gray, byte> mask = new Image<Gray, byte>(blurred.Width, blurred.Height);
            CvInvoke.Circle(mask, new Point { X=(int)center.X, Y = (int)center.Y }, (int)clockCircle.Radius, new MCvScalar(255, 255, 255), -1,LineType.AntiAlias, 0);
            Image<Gray, byte> dest = new Image<Gray, byte>(blurred.Width, blurred.Height);
            dest = blurred.And(blurred, mask);
            filePath = Path.Combine(imagesWorkingDirectory, "circle.png");
            CvInvoke.Imwrite(filePath, dest);

            this.center = new Point((int)clockCircle.Center.X, (int)clockCircle.Center.Y);
            radius = (int)clockCircle.Radius;
            return dest;
            //CvInvoke.NamedWindow("smooth", NamedWindowType.AutoSize);
            //CvInvoke.Imshow("smooth", dest);
            //CvInvoke.WaitKey(0);

            //grad.Draw(clockCircle, new Gray(255));
            //grad.ROI = new Rectangle(((int)clockCircle.Center.X - (int)clockCircle.Radius) + 10, ((int)clockCircle.Center.Y - (int)clockCircle.Radius) + 30, (int)clockCircle.Radius * 2, (int)clockCircle.Radius * 2);
            //CvInvoke.NamedWindow("smooth", NamedWindowType.AutoSize);
            //CvInvoke.Imshow("smooth", grad);
            //CvInvoke.WaitKey(0);

        }



    }
}
