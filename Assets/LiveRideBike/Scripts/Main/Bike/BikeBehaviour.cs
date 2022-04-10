using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
public class BikeBehaviour : MonoBehaviour
{
    [Header("Bike"), Tooltip("バイクオブジェクトのルートを設定")]
    [SerializeField] private Transform BikeModel;
    [Header("Parameter")]
    [SerializeField] public float OffsetLeanAngle = 0f;

    [Header("Handle"), Tooltip("バイクオブジェクトのHandleボーンを設定")]
    [SerializeField] private Transform Handle;
    [SerializeField, Range(1f, 10f)] private float TiltSensitivity = 5f;
    [HideInInspector] public FloatReactiveProperty HandleAngle;
    [HideInInspector] private Quaternion DefaultHandleAngle;

    [Header("Wheel"), Tooltip("バイクオブジェクトのホイールボーンを設定")]
    [SerializeField] private Transform FrontWheel;
    [SerializeField] private Transform RearWheel;
    [SerializeField] private float WheelRotateSpeed = 18;

    [Header("Chain"), Tooltip("バイクオブジェクトのマテリアルを設定")]
    [SerializeField] private SkinnedMeshRenderer ChainMeshRenderer;
    [SerializeField] private int MaterialIndex = 3;
    [SerializeField] private float ChainRotateSpeed = 16;

    void Start()
    {
        //毎フレーム回転
        this.UpdateAsObservable().Subscribe(_ =>
        {
            RotateTransform(FrontWheel, WheelRotateSpeed);
            RotateTransform(RearWheel, WheelRotateSpeed);
        }).AddTo(this);

        //ハンドルの角度
        if (Handle != null) DefaultHandleAngle = Handle.localRotation;
        HandleAngle.Zip(HandleAngle.Skip(1), (x, y) => new Tuple<float, float>(x, y))
        .Subscribe(t => SetTransformaAngle(Handle, -t.Item2 * (TiltSensitivity / 10f), Vector3.up)).AddTo(this);

        //チェーンのアニメーション設定
        if (ChainMeshRenderer.materials.Length > MaterialIndex && MaterialIndex >= 0)
            SetChainMaterialAnimation(ChainMeshRenderer.materials[MaterialIndex], -ChainRotateSpeed);
    }

    //Handleを回転させる
    private void SetTransformaAngle(Transform transform, float angle, Vector3 axis)
    {
        if (transform == null) return;
        Quaternion rotate = Quaternion.AngleAxis(angle, axis);

        transform.localRotation = DefaultHandleAngle * rotate;
    }

    //Transformを回転させる
    private void RotateTransform(Transform transform, float rotateSpeed)
    {
        if (transform == null) return;
        transform.Rotate(new Vector3(rotateSpeed, 0f, 0f));
    }

    //chainMat(MToon)のAutoAnimation>ScrollY(per second)の値をanimationSpeedに設定
    private void SetChainMaterialAnimation(Material chainMat, float animationSpeed)
    {
        if (chainMat == null) return;
        chainMat.SetFloat("_UvAnimScrollY", animationSpeed);
    }

    //バイクの傾き
    public void LeanBikeModel(float leanAngle)
    {
        if (BikeModel == null) return;
        var angle = BikeModel.localEulerAngles;
        angle.z = leanAngle + OffsetLeanAngle;
        BikeModel.rotation
            = Quaternion.Lerp(BikeModel.rotation, Quaternion.Euler(angle), 0.1f);
    }
}