using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UnityEngine;
namespace Sunmax
{
<<<<<<< HEAD
    [SerializeField] private CameraSetting _CameraSetting;

    void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.C))
            .Subscribe(_ =>_CameraSetting.IncrementalCameraIndex());
=======
    public class CameraPresenter : MonoBehaviour
    {
        [SerializeField] private CameraSettings _CameraSettings;

        void Start()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.C))
                .Subscribe(_ => _CameraSettings.IncrementalCameraIndex());
        }

>>>>>>> MainScene
    }
}
