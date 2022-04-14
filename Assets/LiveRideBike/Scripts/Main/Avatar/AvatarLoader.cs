using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RootMotion.FinalIK;
using UniGLTF;
using UniHumanoid;
using UnityEngine;
using UnityEngine.UI;
using VRM;
using VRMShaders;
namespace Sunmax
{
    public class AvatarLoader : MonoBehaviour
    {

        [SerializeField] private bool DebugDefaultVRMLoad = true;
        [SerializeField] private string DefaultVRMFile = "";
        [SerializeField] private string LoadVRMFilePath = "";
        [SerializeField] private AvatarBehaviour AvatarBehaviour;
        HumanPoseTransfer m_src = default;
        [SerializeField] GameObject m_target;
        [SerializeField] private RuntimeAnimatorController Animator;
        class Loaded : IDisposable
        {
            RuntimeGltfInstance _instance;
            HumanPoseTransfer _pose;
            VRMBlendShapeProxy m_proxy;

            Blinker m_blink;
            bool m_enableBlinkValue;
            public bool EnableBlinkValue
            {
                set
                {
                    if (m_blink == value) return;
                    m_enableBlinkValue = value;
                    if (m_blink != null)
                    {
                        m_blink.enabled = m_enableBlinkValue;
                    }
                }
            }


            public Loaded(RuntimeGltfInstance instance, HumanPoseTransfer src, Transform lookAtTarget)
            {
                _instance = instance;

                var lookAt = instance.GetComponent<VRMLookAtHead>();
                if (lookAt != null)
                {
                    // vrm
                    _pose = _instance.gameObject.AddComponent<HumanPoseTransfer>();
                    _pose.Source = src;
                    _pose.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;

                    m_blink = instance.gameObject.AddComponent<Blinker>();

                    lookAt.Target = lookAtTarget;
                    lookAt.UpdateType = UpdateType.LateUpdate; // after HumanPoseTransfer's setPose

                    m_proxy = instance.GetComponent<VRMBlendShapeProxy>();
                }

                // not vrm
                var animation = instance.GetComponent<Animation>();
                if (animation && animation.clip != null)
                {
                    animation.Play(animation.clip.name);
                }
            }

            public void Dispose()
            {
                // Destroy game object. not RuntimeGltfInstance
                GameObject.Destroy(_instance.gameObject);
            }

            public void EnableBvh(HumanPoseTransfer src)
            {
                if (_pose != null)
                {
                    _pose.Source = src;
                    _pose.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;
                }
            }

            public void EnableTPose(HumanPoseClip pose)
            {
                if (_pose != null)
                {
                    _pose.PoseClip = pose;
                    _pose.SourceType = HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseClip;
                }
            }

            public void OnResetClicked()
            {
                if (_pose != null)
                {
                    foreach (var spring in _pose.GetComponentsInChildren<VRMSpringBone>())
                    {
                        spring.Setup();
                    }
                }
            }

            public void Update()
            {
                if (m_proxy != null)
                {
                    m_proxy.Apply();
                }
            }
        }
        Loaded m_loaded;

        [Header("IKTargetGameObject")]
        [SerializeField] private List<Transform> Targets;

        void Start()
        {
            LoadVRMFilePath = DebugDefaultVRMLoad
            ? Application.streamingAssetsPath + "/VRM/" + DefaultVRMFile
            : Application.streamingAssetsPath + "/VRM/" + PlayerPref.VRMFileName;

            LoadModelAsync(LoadVRMFilePath);
        }

        async void LoadModelAsync(string path)
        {
            if (Path.GetExtension(path).ToLower() == ".vrm")
            {
                var instance = await VrmUtility.LoadAsync(path, GetIAwaitCaller(true));
                SetModel(instance);
            }
        }
        static IAwaitCaller GetIAwaitCaller(bool useAsync)
        {
            if (useAsync)
            {
                return new RuntimeOnlyAwaitCaller();
            }
            else
            {
                return new ImmediateCaller();
            }
        }
        void SetModel(RuntimeGltfInstance instance)
        {
            instance.EnableUpdateWhenOffscreen();
            instance.ShowMeshes();

            instance.gameObject.transform.SetParent(transform);
            var comp = instance.gameObject.AddComponent<VRIK>();
            comp.AutoDetectReferences();
            AvatarBehaviour.SetVRIKComponent(comp);
            AvatarBehaviour.SetSolverTarget(Targets);

            instance.GetComponent<Animator>().applyRootMotion = true;
            instance.GetComponent<Animator>().runtimeAnimatorController = Animator;

            m_loaded = new Loaded(instance, m_src, m_target.transform);
        }
    }
}
