using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    [SerializeField] public List<Camera> CameraList;
    [SerializeField] public IntReactiveProperty EnableCameraIndex = new IntReactiveProperty(0);
    [SerializeField] public ColorReactiveProperty BackgroundColor = new ColorReactiveProperty(new Color(255, 0, 255, 255));
    void Start()
    {
        EnableCameraIndex.Subscribe(x => EnableCamera(x));
        BackgroundColor.Subscribe(x => SetCameraBackgroundColor(x));
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
    private void EnableCamera(int index)
    {
        CameraList.ForEach(x => x.enabled = false);
        CameraList[index].enabled = true;
    }
    private void SetCameraBackgroundColor(Color color)
    {
        CameraList.ForEach(x => x.backgroundColor = color);
    }
}