using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.FaceModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.UtilsModule;
using UnityEngine;
using Rect = OpenCVForUnity.CoreModule.Rect;
namespace Sunmax
{
    public class FaceAngle : MonoBehaviour
    {
        [Header("OpenCV")]
        Mat grayMat;
        Texture2D texture;
        CascadeClassifier cascade;
        MatOfRect faces;
        WebCamTextureToMatHelper webCamTextureToMatHelper;
        Facemark facemark;
        protected static readonly string FACEMARK_CASCADE_FILENAME = "objdetect/lbpcascade_frontalface.xml";
        public string facemark_cascade_filepath;
        protected static readonly string FACEMARK_MODEL_FILENAME = "face/lbfmodel.yaml";
        public string facemark_model_filepath;

        [Header("Parameter")]
        [SerializeField] private bool IsDebugMode = false;
        [SerializeField] private bool ViewWebCamImage = false;
        [SerializeField] private float RotateAngle = 0f;
        [SerializeField] private float AngleDiff = 1f;
        [SerializeField] private float MaxAngle = 10f;
        [SerializeField] private bool SkipFrame = false;

        void Start()
        {
            gameObject.transform.GetComponent<MeshRenderer>().enabled = IsDebugMode;
            Utils.setDebugMode(false);
            webCamTextureToMatHelper = gameObject.GetComponent<WebCamTextureToMatHelper>();
#if UNITY_EDITOR
            facemark_cascade_filepath = Utils.getFilePath(FACEMARK_CASCADE_FILENAME);
            facemark_model_filepath = Utils.getFilePath(FACEMARK_MODEL_FILENAME);
#elif UNITY_STANDALONE_WIN
            facemark_cascade_filepath = "LiveRideBike_Data/StreamingAssets/" + FACEMARK_CASCADE_FILENAME;
            facemark_model_filepath ="LiveRideBike_Data/StreamingAssets/" +FACEMARK_MODEL_FILENAME;
#endif
            Run();
        }

        void Update()
        {
            if (SkipFrame) return;
            Debug.Log("test");
            if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
            {
                Mat rgbaMat = webCamTextureToMatHelper.GetMat();
                // Mat rgbaMat = RotateMat(RotateAngle);
                if (facemark == null || cascade == null)
                {
                    Utils.matToTexture2D(rgbaMat, texture);
                    return;
                }

                Imgproc.cvtColor(rgbaMat, grayMat, Imgproc.COLOR_RGBA2GRAY);
                Imgproc.equalizeHist(grayMat, grayMat);

                // detect faces
                DetectFace();
            }
        }

        void DetectFace()
        {
            var rotateGrayMat = RotateMat(RotateAngle, grayMat);

            cascade.detectMultiScale(rotateGrayMat, faces, 1.1, 2, 2,
                                new Size(rotateGrayMat.cols() * 0.2, rotateGrayMat.rows() * 0.2), new Size());

            if (faces.total() != 0)
            {
                List<MatOfPoint2f> landmarks = new List<MatOfPoint2f>();
                facemark.fit(rotateGrayMat, faces, landmarks);
                // Rect[] rects = faces.toArray();
                // for (int i = 0; i < rects.Length; i++)
                // {
                //     Imgproc.rectangle(rotateGrayMat, new Point(rects[i].x, rects[i].y), new Point(rects[i].x + rects[i].width, rects[i].y + rects[i].height), new Scalar(255, 0, 0, 255), 2);
                // }

                //landmark
                for (int i = 0; i < landmarks.Count; i++)
                {
                    MatOfPoint2f lm = landmarks[i];
                    float[] lm_float = new float[lm.total() * lm.channels()];
                    MatUtils.copyFromMat<float>(lm, lm_float);

                    DrawFaceLandmark(rotateGrayMat, ConvertArrayToPointList(lm_float), new Scalar(0, 255, 0, 255), 2, true);

                    for (int j = 0; j < lm_float.Length; j = j + 2)
                    {
                        Point p = new Point(lm_float[j], lm_float[j + 1]);
                        Imgproc.circle(rotateGrayMat, p, 2, new Scalar(255, 0, 0, 255), 1);
                    }
                }
            }
            else
            {
                RotateAngle = RotateAngle + AngleDiff;
                if (RotateAngle >= MaxAngle)
                {
                    AngleDiff = -1f;
                    return;
                }
                else if (RotateAngle <= -MaxAngle)
                {
                    AngleDiff = 1f;
                    return;
                }
                else
                {
                    SkipFrame = true;
                    DetectFace();
                }
                SkipFrame = false;
            }
            if (ViewWebCamImage)
            {
                Utils.matToTexture2D(rotateGrayMat, texture);
            }
        }

        void Run()
        {
            facemark = Face.createFacemarkLBF();
            facemark.loadModel(facemark_model_filepath);
            cascade = new CascadeClassifier(facemark_cascade_filepath);
            webCamTextureToMatHelper.Initialize();
        }
        public void OnWebCamTextureToMatHelperInitialized()
        {
            Mat webCamTextureMat = webCamTextureToMatHelper.GetMat();

            texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);
            Utils.matToTexture2D(webCamTextureMat, texture);

            gameObject.GetComponent<Renderer>().material.mainTexture = texture;

            gameObject.transform.localScale = new Vector3(webCamTextureMat.cols(), webCamTextureMat.rows(), 1);

            float width = webCamTextureMat.width();
            float height = webCamTextureMat.height();

            float widthScale = (float)Screen.width / width;
            float heightScale = (float)Screen.height / height;
            if (widthScale < heightScale)
            {
                Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
            }
            else
            {
                Camera.main.orthographicSize = height / 2;
            }

            grayMat = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC1);

            faces = new MatOfRect();
        }
        public void OnWebCamTextureToMatHelperDisposed()
        {
            Debug.Log("OnWebCamTextureToMatHelperDisposed");

            if (grayMat != null)
                grayMat.Dispose();


            if (texture != null)
            {
                Texture2D.Destroy(texture);
                texture = null;
            }

            if (faces != null)
                faces.Dispose();
        }
        public void OnWebCamTextureToMatHelperErrorOccurred(WebCamTextureToMatHelper.ErrorCode errorCode)
        {
            Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
        }
        private Mat RotateMat(float rotate, Mat source)
        {
            // var mat = webCamTextureToMatHelper.GetMat();
            var mat = source;
            var center = new Point(mat.cols() / 2, mat.cols() / 2);
            Mat rMat = Imgproc.getRotationMatrix2D(center, rotate, 1);
            Mat dest = new Mat();
            Imgproc.warpAffine(mat, dest, rMat, new Size(texture.width, texture.height));
            return dest;
        }
        private void DrawFaceLandmark(Mat imgMat, List<Point> points, Scalar color, int thickness, bool drawIndexNumbers = false)
        {
            if (points.Count == 5)
            {

                Imgproc.line(imgMat, points[0], points[1], color, thickness);
                Imgproc.line(imgMat, points[1], points[4], color, thickness);
                Imgproc.line(imgMat, points[4], points[3], color, thickness);
                Imgproc.line(imgMat, points[3], points[2], color, thickness);

            }
            else if (points.Count == 68)
            {

                for (int i = 1; i <= 16; ++i)
                    Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);

                for (int i = 28; i <= 30; ++i)
                    Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);

                for (int i = 18; i <= 21; ++i)
                    Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                for (int i = 23; i <= 26; ++i)
                    Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                for (int i = 31; i <= 35; ++i)
                    Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                Imgproc.line(imgMat, points[30], points[35], color, thickness);

                for (int i = 37; i <= 41; ++i)
                    Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                Imgproc.line(imgMat, points[36], points[41], color, thickness);

                for (int i = 43; i <= 47; ++i)
                    Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                Imgproc.line(imgMat, points[42], points[47], color, thickness);

                for (int i = 49; i <= 59; ++i)
                    Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                Imgproc.line(imgMat, points[48], points[59], color, thickness);

                for (int i = 61; i <= 67; ++i)
                    Imgproc.line(imgMat, points[i], points[i - 1], color, thickness);
                Imgproc.line(imgMat, points[60], points[67], color, thickness);
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
                    Imgproc.putText(imgMat, i.ToString(), points[i], Imgproc.FONT_HERSHEY_SIMPLEX, 0.25, new Scalar(0, 0, 0, 255), 1, Imgproc.LINE_AA, false);
            }
        }
        private List<Point> ConvertArrayToPointList(float[] arr, List<Point> pts = null)
        {
            if (pts == null)
            {
                pts = new List<Point>();
            }

            if (pts.Count != arr.Length / 2)
            {
                pts.Clear();
                for (int i = 0; i < arr.Length / 2; i++)
                {
                    pts.Add(new Point());
                }
            }

            for (int i = 0; i < pts.Count; ++i)
            {
                pts[i].x = arr[i * 2];
                pts[i].y = arr[i * 2 + 1];
            }

            return pts;
        }
    }
}

