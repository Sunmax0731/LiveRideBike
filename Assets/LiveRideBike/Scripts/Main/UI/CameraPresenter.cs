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
        [SerializeField] private Slider DepthSlider;
        [SerializeField] private Button RegisterButton;
        [SerializeField] private Button LoadButton;
        [SerializeField] private Button InitButton;
        private float DefaultDistance = 1.6f;
        private Vector3 DefaultTargetPosition = new Vector3(0f, 0.75f, 0.2f);

        void Start()
        {
            DistanceSlider.OnValueChangedAsObservable().Subscribe(x =>
            {
                _CameraSettings.CameraList.ForEach(camera => camera.orthographicSize = DefaultDistance - x);
            });

            HeightSlider.OnValueChangedAsObservable().Subscribe(x =>
            {
                var pos = _CameraSettings.TargetPosition.Value;
                pos.y = DefaultTargetPosition.y - x;
                _CameraSettings.TargetPosition.Value = pos;
            });

            DepthSlider.OnValueChangedAsObservable().Subscribe(x =>
            {
                var pos = _CameraSettings.TargetPosition.Value;
                pos.z = x + DefaultTargetPosition.z;
                _CameraSettings.TargetPosition.Value = pos;
            });
        }
    }
}
