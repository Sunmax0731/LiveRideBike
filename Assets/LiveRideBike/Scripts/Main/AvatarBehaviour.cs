using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Sunmax
{
    public class AvatarBehaviour : MonoBehaviour
    {
        [Header("AvatarIKTransform")]
        [HideInInspector] public FloatReactiveProperty LeftLegOffset = new FloatReactiveProperty(0f);
        [SerializeField] private Transform LeftLeg;
        [SerializeField] private Transform LeftLegBend;
        [HideInInspector] private Vector3 DefaultLeftLegPosition;
        [HideInInspector] private Vector3 DefaultLeftLegBendPosition;
        [HideInInspector] public FloatReactiveProperty RightLegOffset = new FloatReactiveProperty(0f);
        [SerializeField] private Transform RightLeg;
        [SerializeField] private Transform RightLegBend;
        [HideInInspector] private Vector3 DefaultRightLegPosition;
        [HideInInspector] private Vector3 DefaultRightLegBendPosition;

        void Start()
        {
            DefaultLeftLegBendPosition = LeftLegBend.transform.localPosition;
            LeftLegOffset.Skip(1).Subscribe(
                x => SetLegBendPosition(LeftLegBend, x, DefaultLeftLegBendPosition));
            DefaultRightLegBendPosition = RightLegBend.transform.localPosition;
            RightLegOffset.Skip(1).Subscribe(
                x => SetLegBendPosition(RightLegBend, x, DefaultRightLegBendPosition));
        }

        private void SetLegBendPosition(Transform transform, float offset, Vector3 defaultPosition)
        {
        }
    }
}