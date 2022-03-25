using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class RotateWheel : MonoBehaviour
{
    [SerializeField] private float RotateSpeed = 5f;
    void Start()
    {
        this.UpdateAsObservable().Subscribe(_ =>
        {
            transform.Rotate(new Vector3(RotateSpeed, 0f, 0f));
        }).AddTo(this);
    }
}
