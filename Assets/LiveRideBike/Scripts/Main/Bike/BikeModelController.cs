using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunmax
{
    public class BikeModelController : MonoBehaviour
    {
        [Header("GameObject")]
        [SerializeField] private GameObject BikeModel;
        [Header("Parameter")]
        [SerializeField] private float OffsetLeanAngle = 0f;

        public void LeanBikeModel(float leanAngle)
        {
            if (BikeModel == null) return;
            var angle = BikeModel.transform.localEulerAngles;
            angle.z = leanAngle + OffsetLeanAngle;
            BikeModel.transform.rotation = Quaternion.Lerp(BikeModel.transform.rotation, Quaternion.Euler(angle), 0.25f);
        }
    }
}