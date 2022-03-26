using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Sunmax
{
    public class AvatarBehaviour : MonoBehaviour
    {
        [Header("AvatarIK")]
        [SerializeField] private VRIK _VRIK;
        [SerializeField, Range(1f, 10f)] public float SqiveOffsetSensitivity = 0f;
        [SerializeField] public FloatReactiveProperty leftBendSwivelOffset = new FloatReactiveProperty(0f);
        [HideInInspector] private float defaultLeftBendSqivelOffset;
        [SerializeField] public FloatReactiveProperty rightBendSwivelOffset = new FloatReactiveProperty(0f);
        [HideInInspector] private float defaultRightBendSqivelOffset;
        void Start()
        {
            defaultLeftBendSqivelOffset = _VRIK.solver.leftLeg.swivelOffset;
            defaultRightBendSqivelOffset = _VRIK.solver.rightLeg.swivelOffset;

            leftBendSwivelOffset.Skip(1).Subscribe(
                x => UpdateBendSwiveOffset(x * (SqiveOffsetSensitivity / 10f), defaultLeftBendSqivelOffset, _VRIK.solver.leftLeg)).AddTo(this);

            rightBendSwivelOffset.Skip(1).Subscribe(
                x => UpdateBendSwiveOffset(x * (SqiveOffsetSensitivity / 10f), defaultRightBendSqivelOffset, _VRIK.solver.rightLeg)).AddTo(this);
        }

        private void UpdateBendSwiveOffset(float offset, float defaultValue, IKSolverVR.Leg leg)
        {
            leg.swivelOffset = defaultValue - offset;
        }
    }
}