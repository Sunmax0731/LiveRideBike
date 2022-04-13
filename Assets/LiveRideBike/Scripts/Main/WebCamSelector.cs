using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.UnityUtils.Helper;
using UnityEngine;
namespace Sunmax
{
    public class WebCamSelector : MonoBehaviour
    {
        [SerializeField] private string DefaultCameraName = "";
        [SerializeField] private WebCamTextureToMatHelper webcamHelper;
        [SerializeField] private FaceAngle faceAngle;
        void Start()
        {
            var cameraName = PlayerPrefs.GetString(PlayerPref.UseCameraName.ToString(), "");

            if (cameraName != "") DefaultCameraName = cameraName;
            webcamHelper.requestedDeviceName = DefaultCameraName;
            webcamHelper.enabled = true;
            faceAngle.enabled = true;
        }
    }
}