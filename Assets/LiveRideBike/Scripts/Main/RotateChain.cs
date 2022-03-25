using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class RotateChain : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material ChainMaterial;
    [SerializeField] private int MaterialIndex;
    void Start()
    {
        ChainMaterial = skinnedMeshRenderer.materials[MaterialIndex];
        this.UpdateAsObservable().Subscribe(_ =>
        {
            ChainMaterial.SetFloat("_UvAnimScrollY", 2f);

        }).AddTo(this);
    }
}
