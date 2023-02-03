using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using Photon.Pun;

namespace ReadyPlayerMe
{
    public class RPMRuntimeLb : MonoBehaviourPunCallbacks
    {

        #region Main Settings
        [Space]
        [Header("Base Settings")]
        [Space]
        [Tooltip("Starter Asset basemodel you can change to other model too")]
        [SerializeField] private string avatarUrl = "";
        public bool loadOnStart;
        [Space]
        public GameObject baseModel;
        public GameObject parentReference;
        private const string PARENT = "ParentRef";
        private GameObject avatar;
        //public object[] Data;
        public string hola = "asfasdgfsdgasd";
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
            AvatarCache.Clear();
            avatarUrl = PhotonNetwork.LocalPlayer.CustomProperties["avatar"].ToString();

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
            }

            LoadAvatar(avatarUrl);
            /*
                    var avatarLoader2 = new AvatarLoader();
                    baseModel.SetActive(false);
                    avatarLoader2.OnCompleted += (_, args) =>
                    {
                        avatar = args.Avatar;
                        parentReference.name = PARENT;
                    };
                    avatarLoader2.LoadAvatar(avatarUrl);
                    parentReference.name = avatarUrl;
                    if (usingEvent)
                    {
                        eventToCallOnLoadAvatar.Invoke();
                    }

             */
        }

        private void OnDestroy()
        {
            if (avatar != null) Destroy(avatar);
            CancelInvoke();
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


                if (usingEvent)
                {
                    eventToCallOnLoadCompleted.Invoke();
                }
                RPMLoadAvatarUI.SetActive(false);
                baseModel.SetActive(false);
                AvatarCache.Clear();

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
            AvatarCache.Clear();
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
        }
        private IEnumerator ErrorShow(string errorMessage)
        {
            RPMErrorUI.GetComponentInChildren<TMP_Text>().text = errorMessage;
            RPMErrorUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowErrorMessage);
            RPMErrorUI.SetActive(false);
        }


        /*public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            //Data = info.photonView.InstantiationData;
            //avatarUrl = Data[0].ToString();

            /*var avatarLoader = new AvatarLoader();

            baseModel.SetActive(false);
            avatarLoader.OnCompleted += (_, args) =>
            {
                avatar = args.Avatar;
                parentReference.name = PARENT;
            };
            avatarLoader.LoadAvatar(avatarUrl);
            parentReference.name = avatarUrl;
            if (usingEvent)
            {
                eventToCallOnLoadAvatar.Invoke();
            }
        }*/

    }
}