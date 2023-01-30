using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnEscenario : MonoBehaviourPunCallbacks
{

    private const string CAMERA = "FollowCamera";
    public ReadyPlayerMe.RPMRuntime RpmRuntime;
    public Text namePlayer;
    public GameObject carga;
    public GameObject playerCameraRoot;
    public GameObject parentRef;
    public PhotonView photonView;
    public GameObject control;
    public bool iscargado = true;
    public bool avatarCargado = false;
    public string avatarUrl;
    public string idavatar;
    private void Start()
    {
        avatarUrl = photonView.Owner.CustomProperties["avatar"].ToString();
        parentRef.name = avatarUrl;
        namePlayer.text = photonView.Owner.NickName;
        if (photonView.IsMine)
        {
            
            GameObject.FindGameObjectWithTag(CAMERA).GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot.transform;

        }
    }
        // Update is called once per frame
    void Update()
    {
        if (control != null && iscargado)
        {
            control.GetComponent<InstanciarJugadpr>().nJugadoresEnEscena += 1;
            control.GetComponent<InstanciarJugadpr>().namesPlayersInstantiate.Add(photonView.Owner.NickName);
            iscargado = false;
        }
        else
        {
            control = GameObject.Find("ControladorEscenario");
        }

        if (photonView.IsMine)
        {
            idavatar = avatarUrl.Replace("https://api.readyplayer.me/v1/avatars/", "");
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
