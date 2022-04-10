using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UtilsModule;
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
        public static void DrawLandmark(Mat mat, float[] landmarks, List<Point> points,
                            bool isDrawIndexNumber = false, bool isDrawLandmarkLine = false, bool isDrawLandmarkOutline = false)
        {
            if (isDrawLandmarkLine) DrawFaceLandmarkLine(mat, points, new Scalar(0, 0, 0, 255), 2);
            if (isDrawLandmarkOutline) DrawLandmarkPointOutline(mat, landmarks);
            if (isDrawIndexNumber) DrawLandmarkIndexNumber(mat, points);
        }
        public static void DrawLandmarkIndexNumber(Mat mat, List<Point> points)
        {
            for (int i = 0; i < points.Count; ++i)
            {
                Imgproc.putText(mat, i.ToString(), points[i],
                    Imgproc.FONT_HERSHEY_SIMPLEX, 0.2, new Scalar(255, 255, 255, 255), 1, Imgproc.LINE_8, false);
            }
        }
        public static void DrawFaceLandmarkLine(Mat mat, List<Point> points, Scalar color, int thickness)
        {
            if (points.Count == 5)
            {
                Imgproc.line(mat, points[0], points[1], color, thickness);
                Imgproc.line(mat, points[1], points[4], color, thickness);
                Imgproc.line(mat, points[4], points[3], color, thickness);
                Imgproc.line(mat, points[3], points[2], color, thickness);
            }
            else if (points.Count == 68)
            {
                //顔の外枠
                for (int i = 1; i <= 16; ++i)
                    Imgproc.line(mat, points[i], points[i - 1], color, thickness);
                //鼻筋
                for (int i = 28; i <= 30; ++i)
                    Imgproc.line(mat, points[i], points[i - 1], color, thickness);
                //左眉
                for (int i = 18; i <= 21; ++i)
                    Imgproc.line(mat, points[i], points[i - 1], color, thickness);
                //右眉
                for (int i = 23; i <= 26; ++i)
                    Imgproc.line(mat, points[i], points[i - 1], color, thickness);
                //鼻
                for (int i = 31; i <= 35; ++i)
                    Imgproc.line(mat, points[i], points[i - 1], color, thickness);
                Imgproc.line(mat, points[30], points[35], color, thickness);

                //左目
                for (int i = 37; i <= 41; ++i)
                    Imgproc.line(mat, points[i], points[i - 1], color, thickness);
                Imgproc.line(mat, points[36], points[41], color, thickness);

                //右目
                for (int i = 43; i <= 47; ++i)
                    Imgproc.line(mat, points[i], points[i - 1], color, thickness);
                Imgproc.line(mat, points[42], points[47], color, thickness);

                //唇の外枠
                for (int i = 49; i <= 59; ++i)
                    Imgproc.line(mat, points[i], points[i - 1], color, thickness);
                Imgproc.line(mat, points[48], points[59], color, thickness);

                //唇の内枠
                for (int i = 61; i <= 67; ++i)
                    Imgproc.line(mat, points[i], points[i - 1], color, thickness);
                Imgproc.line(mat, points[60], points[67], color, thickness);
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                    Imgproc.circle(mat, points[i], 2, color, -1);
            }
        }
        public static void DrawLandmarkPointOutline(Mat mat, float[] landmarks)
        {
            for (int j = 0; j < landmarks.Length; j = j + 2)
                Imgproc.circle(mat, new Point(landmarks[j], landmarks[j + 1]), 2, new Scalar(255, 0, 0, 255), 1);
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
                pts[i] = new Point(arr[i * 2], arr[i * 2 + 1]);

            return pts;
        }
        public static float CalculateFaceTiltAngle(Point pointA, Point pointB)
        {
            return Mathf.Atan2(
                (float)pointB.y - (float)pointA.y,
                (float)pointB.x - (float)pointA.x)
                * 180f / Mathf.PI;
        }
    }
}