using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UnityEngine;
namespace Sunmax
{
    public class CameraPresenter : MonoBehaviour
    {
        [SerializeField] private CameraSettings _CameraSettings;

        void Start()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.C))
                .Subscribe(_ => IncrementalCameraIndex());
        }
        public void IncrementalCameraIndex()
        {
            var newIndex = _CameraSettings.EnableCameraIndex.Value + 1;
            if (newIndex + 1 > _CameraSettings.CameraList.Count)
            {
                _CameraSettings.EnableCameraIndex.Value = 0;
                return;
            };
            _CameraSettings.EnableCameraIndex.Value = newIndex;
        }
    }
}
