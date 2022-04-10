using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Sunmax
{
    public class LightSettingPresenter : MonoBehaviour
    {
        [SerializeField] private Light Light;
        [SerializeField] private ColorReactiveProperty LightColor;
        [SerializeField] private Slider XRotateSlider;
        [SerializeField] private Slider YRotateSlider;
        [SerializeField] private Slider IntensitySlider;
        [SerializeField] private Slider RedSlider;
        [SerializeField] private Slider GreenSlider;
        [SerializeField] private Slider BlueSlider;
        [SerializeField] private Button RegisterButton;
        [SerializeField] private Button LoadButton;
        [SerializeField] private Button InitButton;
        void Start()
        {
            XRotateSlider.OnValueChangedAsObservable().Subscribe(x =>
            {
                var localeulerAngles = Light.transform.localEulerAngles;
                localeulerAngles.x = x;
                Light.transform.localEulerAngles = localeulerAngles;
            });

            YRotateSlider.OnValueChangedAsObservable().Subscribe(x =>
            {
                var localeulerAngles = Light.transform.localEulerAngles;
                localeulerAngles.y = x;
                Light.transform.localEulerAngles = localeulerAngles;
            });

            IntensitySlider.OnValueChangedAsObservable().Subscribe(x =>
            {
                Light.intensity = x;
            });

            Observable.CombineLatest(
                RedSlider.OnValueChangedAsObservable(),
                GreenSlider.OnValueChangedAsObservable(),
                BlueSlider.OnValueChangedAsObservable()
                )
                .Select(colorList => new Color(colorList[0], colorList[1], colorList[2]))
                .Subscribe(color => LightColor.Value = color)
                .AddTo(this);

            LightColor.Subscribe(color => Light.color = color);

            RegisterButton.onClick.AsObservable().Subscribe(_ =>
            {
                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.XRotate.ToString(), XRotateSlider.value);
                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.YRotate.ToString(), YRotateSlider.value);
                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.Intensity.ToString(), IntensitySlider.value);

                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.Color_Red.ToString(), RedSlider.value);
                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.Color_Green.ToString(), GreenSlider.value);
                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.Color_Blue.ToString(), BlueSlider.value);

                PlayerPrefs.Save();
            });

            LoadButton.onClick.AsObservable().Subscribe(_ =>
            {
                XRotateSlider.value = PlayerPrefs.GetFloat(PlayerPref.LightParameterKey.XRotate.ToString(), 0f);
                YRotateSlider.value = PlayerPrefs.GetFloat(PlayerPref.LightParameterKey.YRotate.ToString(), 0f);
                IntensitySlider.value = PlayerPrefs.GetFloat(PlayerPref.LightParameterKey.Intensity.ToString(), 0f);

                RedSlider.value = PlayerPrefs.GetFloat(PlayerPref.LightParameterKey.Color_Red.ToString(), 0f);
                GreenSlider.value = PlayerPrefs.GetFloat(PlayerPref.LightParameterKey.Color_Green.ToString(), 0f);
                BlueSlider.value = PlayerPrefs.GetFloat(PlayerPref.LightParameterKey.Color_Blue.ToString(), 0f);
            });

            InitButton.onClick.AsObservable().Subscribe(_ =>
            {
                XRotateSlider.value = 50f;
                YRotateSlider.value = -30f;
                IntensitySlider.value = 1f;

                RedSlider.value = 1f;
                GreenSlider.value = 1f;
                BlueSlider.value = 1f;

                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.XRotate.ToString(), XRotateSlider.value);
                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.YRotate.ToString(), YRotateSlider.value);
                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.Intensity.ToString(), IntensitySlider.value);

                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.Color_Red.ToString(), RedSlider.value);
                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.Color_Green.ToString(), GreenSlider.value);
                PlayerPrefs.SetFloat(PlayerPref.LightParameterKey.Color_Blue.ToString(), BlueSlider.value);

                PlayerPrefs.Save();
            });
        }
    }
}