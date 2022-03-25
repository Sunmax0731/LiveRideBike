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
        [Header("学習モデルファイル")]
        public string facemark_cascade_filepath;
        protected static readonly string FACEMARK_CASCADE_FILENAME = "objdetect/lbpcascade_frontalface.xml";
        public string facemark_model_filepath;
        protected static readonly string FACEMARK_MODEL_FILENAME = "face/lbfmodel.yaml";

        [Header("OpenCV")]
        Mat grayMat;
        Texture2D texture;
        CascadeClassifier cascade;
        MatOfRect faces;
        WebCamTextureToMatHelper webCamTextureToMatHelper;
        Facemark facemark;
        [Header("Parameter")]
        [SerializeField] private float FaceTiltAngle;
        [HideInInspector, Tooltip("画像のZ軸角度")] private float RotateAngle = 0f;
        [HideInInspector, Tooltip("画像を回転させるときの1回あたりの角度")] private float AngleDiff = 1f;
        [HideInInspector, Range(10f, 45f), Tooltip("画像のZ軸の最大角度の絶対視")] private float MaxAngle = 45f;
        [HideInInspector, Tooltip("顔認識できなかったときにUpdateをスキップするフラグ")] private bool SkipFrame = false;

        [Header("Debug用パラメータ"), Tooltip("以下、デバッグ時のON/OFFフラグ")]
        [SerializeField] private BoolReactiveProperty IsEnableMeshRenderer = new BoolReactiveProperty(false);
        [SerializeField] private bool IsUpdateResultImage = true;
        [SerializeField] private bool IsDrawIndexNumber = false;
        [SerializeField] private bool IsDrawLandmarkPointOutline = false;
        [SerializeField] private bool IsDrawLandmarkLine = false;

        [Header("Component")]
        [SerializeField] private BikeModelController _BikeModelController;
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

            //Updateをストリームに変換
            this.UpdateAsObservable().Subscribe(_ => { WebCamImg2DetectFace(); }).AddTo(this);
        }

        private void WebCamImg2DetectFace()
        {
            if (facemark == null || cascade == null) return;
            if (!webCamTextureToMatHelper.IsPlaying() || !webCamTextureToMatHelper.DidUpdateThisFrame()) return;

            //グレースケールに変換            
            Imgproc.cvtColor(webCamTextureToMatHelper.GetMat(), grayMat, Imgproc.COLOR_RGBA2GRAY);
            //ヒストグラムを平坦化し明るさを調整
            Imgproc.equalizeHist(grayMat, grayMat);
            //顔認識しその結果を取得
            // var rotateGrayMat = CVUtil.RotateMat(RotateAngle, grayMat);
            var mat = DetectFace();
            if (IsUpdateResultImage) Utils.matToTexture2D(mat, texture);
        }

        Mat DetectFace()
        {
            var rotateGrayMat = CVUtil.RotateMat(RotateAngle, grayMat);

            //顔検出
            cascade.detectMultiScale(
                rotateGrayMat, faces, 1.1, 2, 2,
                new Size(rotateGrayMat.cols() * 0.2, rotateGrayMat.rows() * 0.2));

            //検出した顔が１つであれば処理
            if (faces.total() == 1)
            {
                //ランドマークを検出
                List<MatOfPoint2f> landmarks = new List<MatOfPoint2f>();
                facemark.fit(rotateGrayMat, faces, landmarks);

                //ランドマークを描画
                FaceTiltAngle = CVUtil.DrawLandMark(
                    rotateGrayMat, landmarks, RotateAngle,
                    IsDrawIndexNumber, IsDrawLandmarkLine, IsDrawLandmarkPointOutline);
            }
            //検出した顔が１つもなければ処理する画像を回転させて再度検出処理を行う
            else
            {
                //回転させる角度が指定した範囲内であれば増分し検出処理を行う
                if (Math.Abs(RotateAngle + AngleDiff) < MaxAngle)
                {
                    RotateAngle = RotateAngle + AngleDiff;
                    DetectFace();
                }
                //範囲外であれば増分値の符号を反転させる
                else AngleDiff *= -1f;
            }
            _BikeModelController.LeanBikeModel(FaceTiltAngle);
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

    }
}