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
        [SerializeField] private bool SwitchBendSwivelOffset = false;
        [SerializeField, Range(1f, 10f)] public float SqiveOffsetSensitivity = 0f;
        [SerializeField] public FloatReactiveProperty leftBendSwivelOffset = new FloatReactiveProperty(0f);
        [HideInInspector] private float defaultLeftBendSqivelOffset;
        [SerializeField] public FloatReactiveProperty rightBendSwivelOffset = new FloatReactiveProperty(0f);
        [HideInInspector] private float defaultRightBendSqivelOffset;

        [Header("IK Curve")]
        [SerializeField] private AnimationCurve LeftLegCurve;
        [SerializeField] private AnimationCurve RightLegCurve;

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

            leg.swivelOffset = SwitchBendSwivelOffset ? -(defaultValue - offset) : defaultValue - offset;
        }
        public void SetVRIKComponent(VRIK component)
        {
            _VRIK = component;

            //コンポーネントの設定
            _VRIK.solver.plantFeet = false;
            _VRIK.solver.spine.minHeadHeight = 0.8f;
            _VRIK.solver.spine.pelvisPositionWeight = 1.0f;
            _VRIK.solver.spine.pelvisRotationWeight = 1.0f;
            _VRIK.solver.spine.maintainPelvisPosition = 1.0f;
            _VRIK.solver.spine.chestGoalWeight = 1.0f;

            _VRIK.solver.leftArm.bendGoalWeight = 1.0f;
            _VRIK.solver.leftArm.palmToThumbAxis = new Vector3(0f, 0f, 1f);

            _VRIK.solver.rightArm.bendGoalWeight = 1.0f;
            _VRIK.solver.rightArm.palmToThumbAxis = new Vector3(0f, 0f, -1f);

            _VRIK.solver.leftLeg.positionWeight = 1.0f;
            _VRIK.solver.leftLeg.rotationWeight = 0.0f;
            _VRIK.solver.leftLeg.bendGoalWeight = 1.0f;
            _VRIK.solver.leftLeg.bendToTargetWeight = 1.0f;
            _VRIK.solver.leftLeg.stretchCurve = LeftLegCurve;

            _VRIK.solver.rightLeg.positionWeight = 1.0f;
            _VRIK.solver.rightLeg.rotationWeight = 0.0f;
            _VRIK.solver.rightLeg.bendGoalWeight = 1.0f;
            _VRIK.solver.rightLeg.bendToTargetWeight = 1.0f;
            _VRIK.solver.rightLeg.stretchCurve = RightLegCurve;

            _VRIK.solver.locomotion.weight = 0.0f;

        }
        public void SetSolverTarget(List<Transform> Targets)
        {
            _VRIK.solver.spine.headTarget = Targets[0];
            _VRIK.solver.spine.pelvisTarget = Targets[1];
            _VRIK.solver.spine.chestGoal = Targets[2];
            _VRIK.solver.leftArm.target = Targets[3];
            _VRIK.solver.leftArm.bendGoal = Targets[4];
            _VRIK.solver.rightArm.target = Targets[5];
            _VRIK.solver.rightArm.bendGoal = Targets[6];
            _VRIK.solver.leftLeg.target = Targets[7];
            _VRIK.solver.leftLeg.bendGoal = Targets[8];
            _VRIK.solver.rightLeg.target = Targets[9];
            _VRIK.solver.rightLeg.bendGoal = Targets[10];
        }
    }
}