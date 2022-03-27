using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    [SerializeField] public List<Camera> CameraList;
    [SerializeField] public IntReactiveProperty EnableCameraIndex = new IntReactiveProperty(0);
    // Start is called before the first frame update
    void Start()
    {
        EnableCameraIndex.Subscribe(x => EnableCamera(x));
    }

    public int IncrementalCameraIndex(int currentIndex, int listSize)
    {
        if (currentIndex + 1 > listSize) return 0;
        return ++currentIndex;
    }
    private void EnableCamera(int index)
    {
        CameraList.ForEach(x => x.enabled = false);
        CameraList[index].enabled = true;
    }
}