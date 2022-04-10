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
        }
    }
}