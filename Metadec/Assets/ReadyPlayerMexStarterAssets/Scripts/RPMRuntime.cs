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
    public class RPMRuntime : MonoBehaviourPunCallbacks
    {

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

        public void cargarRender(string avatarUrls)
        {
            StartLoadAvatarRenderer(avatarUrls);
        }
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
            AvatarCache.Clear();
        }
        private void UpdateSprite(Texture2D render)
        {
            var sprite = Sprite.Create(render, new Rect(0, 0, render.width, render.height), new Vector2(.5f, .5f));
            spriteRenderer.sprite = sprite;
            loadingPanel.SetActive(false);

            if(sprite != null)
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