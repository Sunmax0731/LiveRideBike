using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
namespace Sunmax
{
    public class BackgroundPresenter : MonoBehaviour
    {
        [SerializeField] private CameraSettings CameraSettings;
        [SerializeField] private ColorReactiveProperty backgroundColor = new ColorReactiveProperty(new Color(255, 0, 255, 255));
        [SerializeField] private Slider RedSlider;
        [SerializeField] private Slider GreenSlider;
        [SerializeField] private Slider BlueSlider;
        [SerializeField] private Button GreenButton;
        [SerializeField] private Button MagentaButton;
        [SerializeField] private Button BlueButton;
        [SerializeField] private Button RegisterButton;
        [SerializeField] private Button LoadButton;
        void Start()
        {
            backgroundColor.Subscribe(color => CameraSettings.CameraBackgroundColor.Value = color);

            Observable.CombineLatest(
                RedSlider.OnValueChangedAsObservable(),
                GreenSlider.OnValueChangedAsObservable(),
                BlueSlider.OnValueChangedAsObservable()
            )
            .Select(colorList => new Color(colorList[0], colorList[1], colorList[2]))
            .Subscribe(color => backgroundColor.Value = color)
            .AddTo(this);

            GreenButton.onClick.AsObservable().Subscribe(_ =>
            {
                backgroundColor.Value = new Color(0f, 1f, 0f);
                SetSliderValue(backgroundColor.Value);
            });

            MagentaButton.onClick.AsObservable().Subscribe(_ =>
            {
                backgroundColor.Value = new Color(1f, 0f, 1f);
                SetSliderValue(backgroundColor.Value);
            });

            BlueButton.onClick.AsObservable().Subscribe(_ =>
            {
                backgroundColor.Value = new Color(0f, 0f, 1f);
                SetSliderValue(backgroundColor.Value);
            });

            RegisterButton.onClick.AsObservable().Subscribe(_ =>
            {
                PlayerPrefs.SetFloat(PlayerPref.BackgroundColorKey.CameraBackground_Red.ToString(), backgroundColor.Value.r);
                PlayerPrefs.SetFloat(PlayerPref.BackgroundColorKey.CameraBackground_Green.ToString(), backgroundColor.Value.g);
                PlayerPrefs.SetFloat(PlayerPref.BackgroundColorKey.CameraBackground_Blue.ToString(), backgroundColor.Value.b);
                PlayerPrefs.Save();
            });

            LoadButton.onClick.AsObservable().Subscribe(_ =>
            {
                var color = new Color(
                        PlayerPrefs.GetFloat(PlayerPref.BackgroundColorKey.CameraBackground_Red.ToString(), 0f),
                        PlayerPrefs.GetFloat(PlayerPref.BackgroundColorKey.CameraBackground_Green.ToString(), 0f),
                        PlayerPrefs.GetFloat(PlayerPref.BackgroundColorKey.CameraBackground_Blue.ToString(), 0f)
                    );

                backgroundColor.Value = color;
            });
        }
        private void SetSliderValue(Color color)
        {
            RedSlider.value = color.r;
            GreenSlider.value = color.g;
            BlueSlider.value = color.b;
        }
    }
}