using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Sunmax
{
    public class IKSettingPresenter : MonoBehaviour
    {
        private Vector3 HeadDefaultPosition = new Vector3(0f, 1.108932f, 0.067f);
        private Vector3 WaistDefaultPosition = new Vector3(0f, 1.016f, -0.161f);
        private Vector3 HipDefaultPosition = new Vector3(0f, 0.955f, -0.27f);
        private Vector3 LeftHandDefaultPosition = new Vector3(0.04078105f, -0.04768449f, 0.0751885f);
        private Vector3 RightHandDefaultPosition = new Vector3(-0.04393022f, 0.05902204f, -0.05016252f);
        private Vector3 LeftKneeDefaultPosition = new Vector3(-0.2026f, 0.7405f, -0.007f);
        private Vector3 RightKneeDefaultPosition = new Vector3(0.1543545f, 0.7620963f, -0.021f);
        private Vector3 LeftFootDefaultPosition = new Vector3(-0.2052f, 0.399f, -0.281f);
        private Vector3 RightFootDefaultPosition = new Vector3(0.211f, 0.4298f, -0.3179f);

        [SerializeField] private Slider HeadSlider_X;
        [SerializeField] private Slider HeadSlider_Y;
        [SerializeField] private Slider HeadSlider_Z;
        [SerializeField] private Slider WaistSlider_X;
        [SerializeField] private Slider WaistSlider_Y;
        [SerializeField] private Slider WaistSlider_Z;
        [SerializeField] private Slider HipSlider_X;
        [SerializeField] private Slider HipSlider_Y;
        [SerializeField] private Slider HipSlider_Z;
        [SerializeField] private Slider HandSlider_X;
        [SerializeField] private Slider HandSlider_Y;
        [SerializeField] private Slider HandSlider_Z;
        [SerializeField] private Slider KneeSlider_X;
        [SerializeField] private Slider KneeSlider_Y;
        [SerializeField] private Slider KneeSlider_Z;
        [SerializeField] private Slider FootSlider_X;
        [SerializeField] private Slider FootSlider_Y;
        [SerializeField] private Slider FootSlider_Z;
        [SerializeField] private Button RegisterButton;
        [SerializeField] private Button LoadButton;
        [SerializeField] private Button InitButton;

        [Header("IKPosition")]
        [SerializeField] private Transform Head;
        [SerializeField] private Transform Chest;
        [SerializeField] private Transform Pelvis;
        [SerializeField] private Transform LeftLeg;
        [SerializeField] private Transform LefgLeg_Bend;
        [SerializeField] private Transform RightLeg;
        [SerializeField] private Transform RightLeg_Bend;
        [SerializeField] private Transform LeftHand;
        [SerializeField] private Transform RightHand;
        void Start()
        {
            HeadSlider_X.OnValueChangedAsObservable().Subscribe(x =>
            {
                var pos = Head.localPosition;
                pos.x = x + HeadDefaultPosition.x;
                Head.localPosition = pos;
            });

            HeadSlider_Y.OnValueChangedAsObservable().Subscribe(x =>
            {
                var pos = Head.localPosition;
                pos.y = x + HeadDefaultPosition.y;
                Head.localPosition = pos;
            });

            HeadSlider_Z.OnValueChangedAsObservable().Subscribe(x =>
            {
                var pos = Head.localPosition;
                pos.z = x + HeadDefaultPosition.z;
                Head.localPosition = pos;
            });

            WaistSlider_X.OnValueChangedAsObservable().Subscribe(x =>
            {
                var pos = Chest.localPosition;
                pos.x = x + WaistDefaultPosition.x;
                Chest.localPosition = pos;
            });
            WaistSlider_Y.OnValueChangedAsObservable().Subscribe(x =>
            {
                var pos = Chest.localPosition;
                pos.y = x + WaistDefaultPosition.y;
                Chest.localPosition = pos;
            });
            WaistSlider_Z.OnValueChangedAsObservable().Subscribe(x =>
            {
                var pos = Chest.localPosition;
                pos.z = x + WaistDefaultPosition.z;
                Chest.localPosition = pos;
            });

            HipSlider_X.OnValueChangedAsObservable().Subscribe(x =>
           {
               var pos = Pelvis.localPosition;
               pos.x = x + HipDefaultPosition.x;
               Pelvis.localPosition = pos;
           });

            HipSlider_Y.OnValueChangedAsObservable().Subscribe(x =>
           {
               var pos = Pelvis.localPosition;
               pos.y = x + HipDefaultPosition.y;
               Pelvis.localPosition = pos;
           });

            HipSlider_Z.OnValueChangedAsObservable().Subscribe(x =>
            {
                var pos = Pelvis.localPosition;
                pos.z = x + HipDefaultPosition.z;
                Pelvis.localPosition = pos;
            });

            HandSlider_X.OnValueChangedAsObservable().Subscribe(x =>
            {
                var leftPos = LeftHand.localPosition;
                leftPos.x = x + LeftHandDefaultPosition.x;
                LeftHand.localPosition = leftPos;

                var rightPos = RightHand.localPosition;
                rightPos.x = x + RightHandDefaultPosition.x;
                RightHand.localPosition = rightPos;
            });
            HandSlider_Y.OnValueChangedAsObservable().Subscribe(x =>
           {
               var leftPos = LeftHand.localPosition;
               leftPos.y = x + LeftHandDefaultPosition.y;
               LeftHand.localPosition = leftPos;

               var rightPos = RightHand.localPosition;
               rightPos.y = x + RightHandDefaultPosition.y;
               RightHand.localPosition = rightPos;
           });
            HandSlider_Z.OnValueChangedAsObservable().Subscribe(x =>
            {
                var leftPos = LeftHand.localPosition;
                leftPos.z = x + LeftHandDefaultPosition.z;
                LeftHand.localPosition = leftPos;

                var rightPos = RightHand.localPosition;
                rightPos.z = x + RightHandDefaultPosition.z;
                RightHand.localPosition = rightPos;
            });

            KneeSlider_X.OnValueChangedAsObservable().Subscribe(x =>
            {
                var leftPos = LefgLeg_Bend.localPosition;
                leftPos.x = x + LeftKneeDefaultPosition.x;
                LefgLeg_Bend.localPosition = leftPos;

                var rightPos = RightLeg_Bend.localPosition;
                rightPos.x = x + RightKneeDefaultPosition.x;
                RightLeg_Bend.localPosition = rightPos;
            });
            KneeSlider_Y.OnValueChangedAsObservable().Subscribe(x =>
            {
                var leftPos = LefgLeg_Bend.localPosition;
                leftPos.y = x + LeftKneeDefaultPosition.y;
                LefgLeg_Bend.localPosition = leftPos;

                var rightPos = RightLeg_Bend.localPosition;
                rightPos.y = x + RightKneeDefaultPosition.y;
                RightLeg_Bend.localPosition = rightPos;
            });
            KneeSlider_Z.OnValueChangedAsObservable().Subscribe(x =>
            {
                var leftPos = LefgLeg_Bend.localPosition;
                leftPos.z = x + LeftKneeDefaultPosition.z;
                LefgLeg_Bend.localPosition = leftPos;

                var rightPos = RightLeg_Bend.localPosition;
                rightPos.z = x + RightKneeDefaultPosition.z;
                RightLeg_Bend.localPosition = rightPos;
            });


            FootSlider_X.OnValueChangedAsObservable().Subscribe(x =>
            {
                var leftPos = LeftLeg.localPosition;
                leftPos.x = x + LeftFootDefaultPosition.x;
                LeftLeg.localPosition = leftPos;

                var rightPos = RightLeg.localPosition;
                rightPos.x = x + RightFootDefaultPosition.x;
                RightLeg.localPosition = rightPos;
            });

            FootSlider_Y.OnValueChangedAsObservable().Subscribe(x =>
            {
                var leftPos = LeftLeg.localPosition;
                leftPos.y = x + LeftFootDefaultPosition.y;
                LeftLeg.localPosition = leftPos;

                var rightPos = RightLeg.localPosition;
                rightPos.y = x + RightFootDefaultPosition.y;
                RightLeg.localPosition = rightPos;
            });
            FootSlider_Z.OnValueChangedAsObservable().Subscribe(x =>
            {
                var leftPos = LeftLeg.localPosition;
                leftPos.z = x + LeftFootDefaultPosition.z;
                LeftLeg.localPosition = leftPos;

                var rightPos = RightLeg.localPosition;
                rightPos.z = x + RightFootDefaultPosition.z;
                RightLeg.localPosition = rightPos;
            });

            RegisterButton.onClick.AsObservable().Subscribe(_ =>
            {
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Head_X.ToString(), HeadSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Head_Y.ToString(), HeadSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Head_Z.ToString(), HeadSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Waist_X.ToString(), WaistSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Waist_Y.ToString(), WaistSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Waist_Z.ToString(), WaistSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hip_X.ToString(), HipSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hip_Y.ToString(), HipSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hip_Z.ToString(), HipSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hand_X.ToString(), HandSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hand_Y.ToString(), HandSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hand_Z.ToString(), HandSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Knee_X.ToString(), KneeSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Knee_Y.ToString(), KneeSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Knee_Z.ToString(), KneeSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Foot_X.ToString(), FootSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Foot_Y.ToString(), FootSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Foot_Z.ToString(), FootSlider_Z.value);

                PlayerPrefs.Save();
            });

            LoadButton.onClick.AsObservable().Subscribe(_ =>
            {
                HeadSlider_X.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Head_X.ToString(), HeadSlider_X.value);
                HeadSlider_Y.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Head_Y.ToString(), HeadSlider_Y.value);
                HeadSlider_Z.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Head_Z.ToString(), HeadSlider_Z.value);

                WaistSlider_X.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Waist_X.ToString(), WaistSlider_X.value);
                WaistSlider_Y.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Waist_Y.ToString(), WaistSlider_Y.value);
                WaistSlider_Z.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Waist_Z.ToString(), WaistSlider_Z.value);

                HipSlider_X.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Hip_X.ToString(), HipSlider_X.value);
                HipSlider_Y.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Hip_Y.ToString(), HipSlider_Y.value);
                HipSlider_Z.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Hip_Z.ToString(), HipSlider_Z.value);

                HandSlider_X.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Hand_X.ToString(), HandSlider_X.value);
                HandSlider_Y.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Hand_Y.ToString(), HandSlider_Y.value);
                HandSlider_Z.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Hand_Z.ToString(), HandSlider_Z.value);

                KneeSlider_X.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Knee_X.ToString(), KneeSlider_X.value);
                KneeSlider_Y.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Knee_Y.ToString(), KneeSlider_Y.value);
                KneeSlider_Z.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Knee_Z.ToString(), KneeSlider_Z.value);

                FootSlider_X.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Foot_X.ToString(), FootSlider_X.value);
                FootSlider_Y.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Foot_Y.ToString(), FootSlider_Y.value);
                FootSlider_Z.value = PlayerPrefs.GetFloat(PlayerPref.IKPositionKey.Foot_Z.ToString(), FootSlider_Z.value);
            });

            InitButton.onClick.AsObservable().Subscribe(_ =>
            {
                HeadSlider_X.value = 0f;
                HeadSlider_Y.value = 0f;
                HeadSlider_Z.value = 0f;

                WaistSlider_X.value = 0f;
                WaistSlider_Y.value = 0f;
                WaistSlider_Z.value = 0f;

                HipSlider_X.value = 0f;
                HipSlider_Y.value = 0f;
                HipSlider_Z.value = 0f;

                HandSlider_X.value = 0f;
                HandSlider_Y.value = 0f;
                HandSlider_Z.value = 0f;

                KneeSlider_X.value = 0f;
                KneeSlider_Y.value = 0f;
                KneeSlider_Z.value = 0f;

                FootSlider_X.value = 0f;
                FootSlider_Y.value = 0f;
                FootSlider_Z.value = 0f;

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Head_X.ToString(), HeadSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Head_Y.ToString(), HeadSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Head_Z.ToString(), HeadSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Waist_X.ToString(), WaistSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Waist_Y.ToString(), WaistSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Waist_Z.ToString(), WaistSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hip_X.ToString(), HipSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hip_Y.ToString(), HipSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hip_Z.ToString(), HipSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hand_X.ToString(), HandSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hand_Y.ToString(), HandSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Hand_Z.ToString(), HandSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Knee_X.ToString(), KneeSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Knee_Y.ToString(), KneeSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Knee_Z.ToString(), KneeSlider_Z.value);

                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Foot_X.ToString(), FootSlider_X.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Foot_Y.ToString(), FootSlider_Y.value);
                PlayerPrefs.SetFloat(PlayerPref.IKPositionKey.Foot_Z.ToString(), FootSlider_Z.value);

                PlayerPrefs.Save();
            });
        }
    }
}