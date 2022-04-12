using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Sunmax
{
    public class CameraPresenter : MonoBehaviour
    {
        [SerializeField] private CameraSettings _CameraSettings;

        [SerializeField] private Slider DistanceSlider;
        [SerializeField] private Slider HeightSlider;
        [SerializeField] private Slider AngleSlider;

        void Start()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.C))
                .Subscribe(_ => _CameraSettings.IncrementalCameraIndex());
        }
    }
}
