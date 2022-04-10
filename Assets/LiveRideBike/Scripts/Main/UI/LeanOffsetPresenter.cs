using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Sunmax
{
    public class LeanOffsetPresenter : MonoBehaviour
    {
        [SerializeField] BikeBehaviour BikeBehaviour;
        [SerializeField] private Slider LeanSlider;
        [SerializeField] private Button RegisterButton;
        [SerializeField] private Button LoadButton;
        [SerializeField] private Button InitButton;
        private
        void Start()
        {
            LeanSlider.OnValueChangedAsObservable().Subscribe(x => BikeBehaviour.OffsetLeanAngle = x);

            RegisterButton.onClick.AsObservable().Subscribe(_ =>
            {
                var value = LeanSlider.value;
                PlayerPrefs.SetFloat(PlayerPref.LeanOffsetKey, value);
                PlayerPrefs.Save();
            });

            LoadButton.onClick.AsObservable().Subscribe(_ =>
            {
                LeanSlider.value = PlayerPrefs.GetFloat(PlayerPref.LeanOffsetKey, 0f);
            });

            InitButton.onClick.AsObservable().Subscribe(_ =>
            {
                var value = 0f;
                LeanSlider.value = value;
                PlayerPrefs.SetFloat(PlayerPref.LeanOffsetKey, value);
                PlayerPrefs.Save();
            });
        }
    }
}