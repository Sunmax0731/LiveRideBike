using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.FaceModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.UtilsModule;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

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
        [HideInInspector, Tooltip("画像のZ軸角度")] private float RotateAngle = 0f;
        [HideInInspector, Tooltip("画像を回転させるときの1回あたりの角度")] private float AngleDiff = 1f;
        [HideInInspector, Range(10f, 45f), Tooltip("画像のZ軸の最大角度の絶対視")] private float MaxAngle = 10f;
        [HideInInspector, Tooltip("顔認識できなかったときにUpdateをスキップするフラグ")] private bool SkipFrame = false;
        [SerializeField] private ReactiveProperty<float> FaceTiltAngle;
        [Header("Debug用パラメータ"), Tooltip("以下、デバッグ時のON/OFFフラグ")]
        [HideInInspector] private bool IsUpdateResultImage = true;
        [HideInInspector] private BoolReactiveProperty IsEnableMeshRenderer = new BoolReactiveProperty(false);
        [HideInInspector] private bool IsDrawIndexNumber = false;
        [HideInInspector] private bool IsDrawLandmarkPointOutline = false;
        [HideInInspector] private bool IsDrawLandmarkLine = false;
        void Start()
        {
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

            //メッシュレンダラーを非表示
            IsEnableMeshRenderer.Subscribe(x => { gameObject.transform.GetComponent<MeshRenderer>().enabled = x; }).AddTo(this);
            //傾きをモデルに反映
            FaceTiltAngle.Subscribe(x => { }).AddTo(this);
            //Updateをストリームに変換
            this.UpdateAsObservable().Subscribe(_ => { WebCamImg2DetectFace(); }).AddTo(this);
        }

        private void WebCamImg2DetectFace()
        {
            if (facemark == null || cascade == null) return;
            if (SkipFrame) return;
            if (!webCamTextureToMatHelper.IsPlaying() || !webCamTextureToMatHelper.DidUpdateThisFrame()) return;

            //グレースケールに変換            
            Imgproc.cvtColor(webCamTextureToMatHelper.GetMat(), grayMat, Imgproc.COLOR_RGBA2GRAY);
            //ヒストグラムを平坦化し明るさを調整
            Imgproc.equalizeHist(grayMat, grayMat);
            //顔認識しその結果を取得
            var mat = DetectFace();
            if (IsUpdateResultImage) Utils.matToTexture2D(mat, texture);
        }

        Mat DetectFace()
        {
            var rotateGrayMat = CVUtil.RotateMat(RotateAngle, grayMat);

            cascade.detectMultiScale(rotateGrayMat, faces, 1.1, 2, 2,
                                new Size(rotateGrayMat.cols() * 0.2, rotateGrayMat.rows() * 0.2), new Size());

            if (faces.total() == 1)
            {
                List<MatOfPoint2f> landmarks = new List<MatOfPoint2f>();
                facemark.fit(rotateGrayMat, faces, landmarks);

                //landmark
                for (int i = 0; i < landmarks.Count; i++)
                {
                    MatOfPoint2f lm = landmarks[i];
                    float[] lm_float = new float[lm.total() * lm.channels()];
                    MatUtils.copyFromMat<float>(lm, lm_float);
                    var points = CVUtil.ConvertArrayToPointList(lm_float);
                    FaceTiltAngle.Value = CalculateFaceTiltAngle(points[0], points[16]) + RotateAngle;
                    CVUtil.DrawFaceLandmark(rotateGrayMat, points, new Scalar(0, 0, 0, 255), 2, IsDrawIndexNumber, IsDrawLandmarkLine);

                    if (IsDrawLandmarkPointOutline)
                    {
                        for (int j = 0; j < lm_float.Length; j = j + 2)
                        {
                            Imgproc.circle(rotateGrayMat, new Point(lm_float[j], lm_float[j + 1]), 2, new Scalar(255, 0, 0, 255), 1);
                        }
                    }
                }
            }
            else
            {
                if (RotateAngle + AngleDiff >= MaxAngle)
                {
                    AngleDiff = -1f;
                    return rotateGrayMat;
                }
                else if (RotateAngle + AngleDiff <= -MaxAngle)
                {
                    AngleDiff = 1f;
                    return rotateGrayMat;
                }
                else
                {
                    RotateAngle = RotateAngle + AngleDiff;
                    SkipFrame = true;
                    DetectFace();
                }
                SkipFrame = false;
            }

            return rotateGrayMat;
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
            grayMat = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC1);
            faces = new MatOfRect();
        }
        public void OnWebCamTextureToMatHelperDisposed()
        {
            Debug.Log("OnWebCamTextureToMatHelperDisposed");
            if (grayMat != null) grayMat.Dispose();
            if (texture != null) Texture2D.Destroy(texture);
            if (faces != null) faces.Dispose();
            texture = null;
        }
        public void OnWebCamTextureToMatHelperErrorOccurred(WebCamTextureToMatHelper.ErrorCode errorCode)
        {
            Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
        }

        private float CalculateFaceTiltAngle(Point pointA, Point pointB)
        {
            return Mathf.Atan2((float)pointB.y - (float)pointA.y, (float)pointB.x - (float)pointA.x) * 180f / Mathf.PI;
        }
    }
}