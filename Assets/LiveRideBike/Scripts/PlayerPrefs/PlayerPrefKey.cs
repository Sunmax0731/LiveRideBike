using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunmax
{
    static public class PlayerPref
    {
        static public string UseCameraName = "UseCameraName";
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
        public enum IKPositionKey
        {
            Head_X,
            Head_Y,
            Head_Z,
            Waist_X,
            Waist_Y,
            Waist_Z,
            Hip_X,
            Hip_Y,
            Hip_Z,
            Hand_X,
            Hand_Y,
            Hand_Z,
            Knee_X,
            Knee_Y,
            Knee_Z,
            Foot_X,
            Foot_Y,
            Foot_Z,
        }
    }
}