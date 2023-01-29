using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

namespace ReadyPlayerMe
{
    public class RPM : MonoBehaviour
    {
        #region Main Settings
        [Space]
        [Header("Base Settings")]
        [Space]
        [Tooltip("Starter Asset basemodel you can change to other model too")]
        [SerializeField] private string avatarUrl = "https://api.readyplayer.me/v1/avatars/63d2ccd323fe23d34bf8069f.glb";
        public bool loadOnStart;
        [Space]
        public GameObject baseModel;
        public GameObject parentReference;
        private const string PARENT = "ParentRef";
        private GameObject avatar;
        #endregion

        #region Camera Settings
        [Space]
        [Tooltip("Assigning Player Follow Camera at runtime")]
        [Header("Camera Settings")]
        [Space]
        public Transform playerCameraRoot;
        private const string CAMERA = "FollowCamera";
        #endregion

        #region UI Setting
        [Space]
        [Tooltip("Setting for UI")]
        [Header("UI Objects")]
        [Space]
        public GameObject RPMAvatarMenu;
        public GameObject RPMLoadAvatarUI;
        public GameObject RPMErrorUI;
        [HideInInspector] public bool avatarSelection;

        [Space]
        [Tooltip("UI Message Setting")]
        [Header("UI Message")]
        [Space]
        public string loadAvatarText = "Load Ready Player Me avatar. Please wait...";
        public string loadErrorText = "Timeout after 2000ms, avatar failed to load. Please try again";
        public string urlErrorText = "Given url is invalid or is not Ready Player Me avatar. Please check again";
        public float timeToShowErrorMessage = 3f;
        #endregion

        #region Eye Animator
        [Space]
        [Tooltip("Eye Animation Setting")]
        [Header("Eye Animation Handler")]
        [Space]
        public bool usingEyeAnimation;
        [Range(0, 1)] public float BlinkSpeed = 0.1f;
        [Range(1, 10)] public float BlinkInterval = 3f;
        #endregion

        #region Voice Handler
        [Header("Voice Handler")]
        [Space]
        [Tooltip("Voice Handler Setting")]
        public bool usingVoiceHandler;
        public AudioClip AudioClip;
        public AudioSource AudioSource;
        public AudioProviderType AudioProvider = AudioProviderType.Microphone;
        #endregion

        #region Avatar Renderer
        [Header("Avatar Renderer UI")]
        [Space]
        [Tooltip("Avatar Renderer Settings")]
        public bool usingAvatarUI;
        [SerializeField] private AvatarRenderScene scene = AvatarRenderScene.FullBodyPostureTransparent;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private Image avatarUIPanel;

        private readonly string blendShapeMesh = "Wolf3D_Avatar";
        private readonly Dictionary<string, float> blendShapes = new Dictionary<string, float>
        {
            { "mouthSmile", 0.7f },
            { "viseme_aa", 0.5f },
            { "jawOpen", 0.3f }
        };

        private const string TAG = nameof(AvatarRenderExample);
        #endregion

        #region Events
        [Space]
        [Header("Event Setting")]
        public bool usingEvent;
        [Space]
        public UnityEvent eventToCallOnLoadAvatar = new UnityEvent();
        public UnityEvent eventToCallOnLoadCompleted = new UnityEvent();
        public UnityEvent eventToCallOnLoadFailed = new UnityEvent();
        public UnityEvent eventToCallOnUrlError = new UnityEvent();
        #endregion

        #region DebugLog
        [Space]
        [Header("Debug Log Setting")]
        public GameObject DebugLog;
        public bool enableDebugLog = false;
        #endregion

        private void Start()
        {
            ApplicationData.Log();

            GameObject.FindGameObjectWithTag(CAMERA).GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot;

            if (enableDebugLog)
            {
                if (DebugLog == null)
                {
                    Debug.LogWarning("Please assign Debug Log Panel Game Object", DebugLog);
                }
                else
                {
                    DebugLog.SetActive(true);
                }
            }

            if (loadOnStart)
            {
                bool checkURL = avatarUrl.Contains(".glb");

                if (avatarUrl == null || !checkURL)
                {
                    UrlError(avatarUrl);
                }
                else
                {
                    LoadAvatar(avatarUrl);
                }
            }
            else
            {
                avatarSelection = true;
                AvatarSelection();
            }

        }

        public void AvatarLoaderinScene(string avatarUrlsend)
        {
            avatarUrl = avatarUrlsend;
            LoadAvatar(avatarUrl);
        }

        private void OnDestroy()
        {
            if (avatar != null) Destroy(avatar);
            CancelInvoke();
        }

        #region Load Avatar
        private void AvatarSelection()
        {
            if (avatarSelection || enableDebugLog)
            {
                RPMLoadAvatarUI.GetComponentInChildren<TMP_Text>().text = loadAvatarText;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                RPMAvatarMenu.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void LoadAvatar(string avatarUrls)
        {
            var avatarLoader = new AvatarLoader();
            avatarLoader.OnCompleted += (_, args) =>
            {
                avatar = args.Avatar;
                parentReference.name = PARENT;

                if (usingEyeAnimation)
                {
                    var eyeAnimator = avatar.AddComponent<EyeAnimationHandler>();
                    eyeAnimator.BlinkSpeed = BlinkSpeed;
                    eyeAnimator.BlinkInterval = BlinkInterval;
                }

                if (usingVoiceHandler)
                {
                    VoiceHandler voiceHandler = avatar.AddComponent<VoiceHandler>();
                    voiceHandler.AudioSource = AudioSource;
                    voiceHandler.AudioClip = AudioClip;
                    voiceHandler.AudioProvider = AudioProvider;
                }

                if (usingAvatarUI)
                {
                    StartLoadAvatarRenderer(avatarUrls);
                }

                if (usingEvent)
                {
                    eventToCallOnLoadCompleted.Invoke();
                }

                RPMLoadAvatarUI.SetActive(false);
                avatarSelection = false;
                AvatarSelection();
                baseModel.SetActive(false);
            };
            avatarLoader.OnFailed += (_, args) =>
            {
                RPMLoadAvatarUI.SetActive(false);

                if (usingEvent)
                {
                    eventToCallOnLoadFailed.Invoke();
                }

                if (enableDebugLog)
                {
                    SDKLogger.Log(tag, loadErrorText);
                }

                StartCoroutine(ErrorShow(loadErrorText));
            };
            avatarLoader.LoadAvatar(avatarUrls);

            parentReference.name = avatarUrls;

            if (usingEvent)
            {
                eventToCallOnLoadAvatar.Invoke();
            }

            RPMLoadAvatarUI.SetActive(true);
        }
        private void UrlError(String ErrorField)
        {
            if (usingEvent)
            {
                eventToCallOnUrlError.Invoke();
            }

            StartCoroutine(ErrorShow(urlErrorText));

            if (enableDebugLog)
            {
                SDKLogger.Log(tag, ErrorField + " = " + urlErrorText);
            }
            if (loadOnStart)
            {
                avatarSelection = true;
                AvatarSelection();
            }
        }
        private IEnumerator ErrorShow(string errorMessage)
        {
            RPMErrorUI.GetComponentInChildren<TMP_Text>().text = errorMessage;
            RPMErrorUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowErrorMessage);
            RPMErrorUI.SetActive(false);
        }
        #endregion

        #region Avatar Renderer
        private void StartLoadAvatarRenderer(string _loadAvatarRenderer)
        {
            var avatarRenderer = new AvatarRenderLoader
            {
                OnCompleted = UpdateSprite,
                OnFailed = Fail
            };
            avatarRenderer.LoadRender(_loadAvatarRenderer, scene, blendShapeMesh, blendShapes);
            loadingPanel.SetActive(true);

            SDKLogger.Log(TAG, "Start Load Avatar Renderer");
        }
        private void UpdateSprite(Texture2D render)
        {
            var sprite = Sprite.Create(render, new Rect(0, 0, render.width, render.height), new Vector2(.5f, .5f));
            spriteRenderer.sprite = sprite;
            loadingPanel.SetActive(false);

            if (sprite != null)
            {
                AssignThumbnail(spriteRenderer, sprite);
            }

            SDKLogger.Log(TAG, "Sprite Updated ");
        }
        private void Fail(FailureType type, string message)
        {
            SDKLogger.Log(TAG, $"Failed with error type: {type} and message: {message}");
        }
        private void AssignThumbnail(SpriteRenderer spriteRenderer, Sprite sprite)
        {
            avatarUIPanel.gameObject.SetActive(true);
            bool avatarUIPanelActive = avatarUIPanel.IsActive();

            if (sprite && avatarUIPanelActive)
            {
                avatarUIPanel.sprite = sprite;
                avatarUIPanel.material.mainTexture = spriteRenderer.sharedMaterial.mainTexture;

                spriteRenderer.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}