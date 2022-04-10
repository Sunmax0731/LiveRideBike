using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunmax
{
    static public class PlayerPref
    {
        public enum BackgroundColorKey
        {
            CameraBackground_Red,
            CameraBackground_Green,
            CameraBackground_Blue,
        }
        static public string LeanOffsetKey = "LeanOffset";

        public enum LightParameterKey
        {
            XRotate,
            YRotate,
            Intensity,
            Color_Red,
            Color_Green,
            Color_Blue
        }
    }
}

