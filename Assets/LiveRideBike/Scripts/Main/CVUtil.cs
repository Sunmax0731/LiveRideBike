using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using UnityEngine;

namespace Sunmax
{
    static class CVUtil
    {
        public static Mat RotateMat(float rotate, Mat source)
        {
            Mat rMat = Imgproc.getRotationMatrix2D(new Point(source.cols() / 2, source.cols() / 2), rotate, 1);
            Mat dest = new Mat();
            Imgproc.warpAffine(source, dest, rMat, new Size(source.cols(), source.rows()));
            return dest;
        }

        public static void DrawFaceLandmark(Mat imgMat, List<Point> points, Scalar color, int thickness, bool drawIndexNumbers = false, bool isDrawLandmarkLine = false)
        {
            if (points.Count == 5 && isDrawLandmarkLine)
            {
                Imgproc.line(imgMat, points[0], points[1], color, thickness);
                Imgproc.line(imgMat, points[1], points[4], color, thickness);
                Imgproc.line(imgMat, points[4], points[3], color, thickness);
                Imgproc.line(imgMat, points[3], points[2], color, thickness);
            }
            else if (points.Count == 68)
            {
                if (isDrawLandmarkLine)
                {
                    //顔の外枠
                    for (int i = 1; i <= 16; ++i)
                        Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                    //鼻筋
                    for (int i = 28; i <= 30; ++i)
                        Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                    //左眉
                    for (int i = 18; i <= 21; ++i)
                        Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                    //右眉
                    for (int i = 23; i <= 26; ++i)
                        Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                    //鼻
                    for (int i = 31; i <= 35; ++i)
                        Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                    Imgproc.line(imgMat, points[30], points[35], color, thickness);

                    //左目
                    for (int i = 37; i <= 41; ++i)
                        Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                    Imgproc.line(imgMat, points[36], points[41], color, thickness);

                    //右目
                    for (int i = 43; i <= 47; ++i)
                        Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                    Imgproc.line(imgMat, points[42], points[47], color, thickness);

                    //唇の外枠
                    for (int i = 49; i <= 59; ++i)
                        Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                    Imgproc.line(imgMat, points[48], points[59], color, thickness);

                    //唇の内枠
                    for (int i = 61; i <= 67; ++i)
                        Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                    Imgproc.line(imgMat, points[60], points[67], color, thickness);
                }
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Imgproc.circle(imgMat, points[i], 2, color, -1);
                }
            }

            // Draw the index number of facelandmark points.
            if (drawIndexNumbers)
            {
                for (int i = 0; i < points.Count; ++i)
                    Imgproc.putText(imgMat, i.ToString(), points[i], Imgproc.FONT_HERSHEY_SIMPLEX, 0.2, new Scalar(255, 255, 255, 255), 1, Imgproc.LINE_AA, false);
            }
        }
        public static List<Point> ConvertArrayToPointList(float[] arr, List<Point> pts = null)
        {
            if (pts == null) pts = new List<Point>();

            if (pts.Count != arr.Length / 2)
            {
                pts.Clear();
                for (int i = 0; i < arr.Length / 2; i++) pts.Add(new Point());
            }

            for (int i = 0; i < pts.Count; ++i)
            {
                pts[i] = new Point(arr[i * 2], arr[i * 2 + 1]);
            }

            return pts;
        }
    }
}
