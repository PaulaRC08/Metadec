using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnEscenario : MonoBehaviourPunCallbacks
{
    public Player jugadorPhoton;
    public GameObject proximityVoice;
    public GameObject canvaPlayer;
    private const string CAMERA = "FollowCamera";
    public ReadyPlayerMe.RPMRuntime RpmRuntime;
    public Text namePlayer;
    public GameObject carga;
    public GameObject playerCameraRoot;
    public GameObject parentRef;
    public PhotonView photonView;
    public GameObject control;
    public Recorder photonvoiceRecoder;
    public bool iscargado = true;
    public bool avatarCargado = false;
    public string avatarUrl;
    public string idavatar;

    public GameObject camera;
    public GameObject playerFollow;


    //public GameObject prefMainCamera;
    //public GameObject prefPlayerFollow;
    private void Start()
    {
        canvaPlayer.SetActive(false);
        avatarUrl = photonView.Owner.CustomProperties["avatar"].ToString();
        jugadorPhoton = photonView.Controller;
        parentRef.name = avatarUrl;
        namePlayer.text = photonView.Owner.NickName;
        if (photonView.IsMine)
        {
            //camera = Instantiate(prefMainCamera, Vector2.zero, Quaternion.identity);
            //playerFollow = Instantiate(prefPlayerFollow, Vector2.zero, Quaternion.identity);
            canvaPlayer.SetActive(true);
            this.GetComponent<PlayerUIScene>().enabled = true;

            //playerFollow.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot.transform;
            GameObject.FindGameObjectWithTag(CAMERA).GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot.transform;

        }
        else
        {
            this.GetComponent<ThirdPersonController>().enabled = false;
        }


    }
        // Update is called once per frame
    void Update()
    {
        carga.SetActive(false);
        if (control != null && iscargado)
        {
            control.GetComponent<InstanciarJugadpr>().nJugadoresEnEscena += 1;
            control.GetComponent<InstanciarJugadpr>().namesPlayersInstantiate.Add(photonView.Owner.NickName);
            camera = control.GetComponent<InstanciarJugadpr>().mainCamera;
            playerFollow = control.GetComponent<InstanciarJugadpr>().playerFollow;
            iscargado = false;
        }
        else
        {
            control = GameObject.Find("ControladorEscenario");
        }

        if (photonvoiceRecoder == null)
        {
            photonvoiceRecoder = GameObject.Find("PhotonVoiceControler").GetComponent<Recorder>();
        }

        if (photonView.IsMine)
        {
            idavatar = avatarUrl.Replace("https://models.readyplayer.me/", "");
            if (GameObject.Find(idavatar.Replace(".glb","")) != null && !avatarCargado)
            {
                carga.SetActive(false);
                RpmRuntime.cargarRender(avatarUrl);
                avatarCargado = true;
            }
        }
        else
        {
            carga.SetActive(false);
        }
    }
}
