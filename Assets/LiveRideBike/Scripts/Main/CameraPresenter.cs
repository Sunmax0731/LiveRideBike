using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UnityEngine;
public class CameraPresenter : MonoBehaviour
{
    [SerializeField] private CameraSwitcher _CameraSwitcher;

    void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.C))
            .Subscribe(_ => IncrementalCameraIndex());
    }
    public void IncrementalCameraIndex()
    {
        var newIndex = _CameraSwitcher.EnableCameraIndex.Value + 1;
        if (newIndex + 1 > _CameraSwitcher.CameraList.Count)
        {
            _CameraSwitcher.EnableCameraIndex.Value = 0;
            return;
        };
        _CameraSwitcher.EnableCameraIndex.Value = newIndex;
    }
}