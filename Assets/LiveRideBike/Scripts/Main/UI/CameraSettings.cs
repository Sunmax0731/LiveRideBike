using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace Sunmax
{
    public class CameraSettings : MonoBehaviour
    {
        [SerializeField] public List<Camera> CameraList;
        [SerializeField] public List<Camera> BackgroundCamera;
        [SerializeField] public IntReactiveProperty EnableCameraIndex = new IntReactiveProperty(0);
        [SerializeField] public ColorReactiveProperty CameraBackgroundColor = new ColorReactiveProperty(new Color(255, 0, 255, 255));
        [SerializeField] private Transform CameraTarget;
        [SerializeField] public Vector3ReactiveProperty TargetPosition;
        void Start()
        {
            // EnableCameraIndex.Subscribe(x => EnableCamera(x));
            CameraBackgroundColor.Subscribe(x => SetBackgroundColor(x));

            TargetPosition.Subscribe(pos =>
            {
                LookAtTarget(pos);
            });
        }

        public void IncrementalCameraIndex()
        {
            var newIndex = EnableCameraIndex.Value + 1;
            if (newIndex + 1 > CameraList.Count)
            {
                EnableCameraIndex.Value = 0;
                return;
            };
            EnableCameraIndex.Value = newIndex;
        }
        private void SetBackgroundColor(Color color)
        {
            BackgroundCamera.ForEach(x => x.backgroundColor = color);
        }
        private void EnableCamera(int index)
        {
            CameraList.ForEach(x => x.enabled = false);
            CameraList[index].enabled = true;
        }

        private void LookAtTarget(Vector3 pos)
        {
            CameraTarget.localPosition = pos;
            CameraList.ForEach(camera =>
            {
                camera.transform.LookAt(CameraTarget);
            });
        }
    }
}