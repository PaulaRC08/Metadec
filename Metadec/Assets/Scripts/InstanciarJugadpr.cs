using Photon.Pun;
using Photon.Realtime;
using ReadyPlayerMe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstanciarJugadpr : MonoBehaviourPunCallbacks
{
    int min = 1, seg = 30;
    float restante;
    bool enmarcha = true;

    bool sesionTerminada;

    public GameObject cargarAvatars;
    public GameObject carga;
    public GameObject avatar;
    public GameObject playerAvater;
    //public GameObject prefMainCamera;
    //public GameObject prefPlayerFollow;
    public bool cargandoJugadores = true;
    public int nJugadores;
    public int nJugadoresEnEscena = 0;

    public GameObject mainCamera;
    public GameObject playerFollow;

    public List<string> avatarUrls = new List<string>();
    public List<string> namesPlayersInstantiate = new List<string>();

    void Start()
    {
        restante = (min * 60) + seg;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
        carga.SetActive(true);
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("LoginRoom");

            return;
        }

        //Instantiate(prefMainCamera, Vector2.zero, Quaternion.identity);
        //Instantiate(prefPlayerFollow, Vector2.zero, Quaternion.identity);
        avatar = PhotonNetwork.Instantiate(playerAvater.name, new Vector3(Random.Range(30f, 58.52f), 2.3f, Random.Range(43.8f, 50f)), Quaternion.identity, 0) as GameObject;
        //avatar = PhotonNetwork.Instantiate(playerAvater.name, new Vector3(25.1830006f, 7.1500001f, 22.6369991f), Quaternion.identity, 0) as GameObject;
        avatar.GetComponent<EnEscenario>().control = this.gameObject;
        nJugadores = (int)PhotonNetwork.PlayerList.Length;
    }

    private void Update()
    {
        if (cargandoJugadores)
        {
            nJugadores = (int)PhotonNetwork.PlayerList.Length;
            if (nJugadores == nJugadoresEnEscena)
            {
                carga.SetActive(false);
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    avatarUrls.Add(player.CustomProperties["avatar"].ToString());
                }
                cargarAvatars.GetComponent<RuntimeExampleMultiple>().avatarUrls.AddRange(avatarUrls);
                cargarAvatars.GetComponent<ReadyPlayerMe.RuntimeExampleMultiple>().cargarAvatarsEscena();
                cargandoJugadores = false;
            }
        }
        if (avatar != null)
        {
            if (enmarcha)
            {
                restante -= Time.deltaTime;
                if (restante < 1)
                {
                    enmarcha = false;
                    PhotonNetwork.LeaveRoom();
                    sesionTerminada = true;
                    avatar.GetComponent<PlayerUIScene>().relojGeneral.gameObject.SetActive(false);
                    carga.SetActive(true);

                }
                int tempMin = Mathf.FloorToInt(restante / 60);
                int tempSeg = Mathf.FloorToInt(restante % 60);
                avatar.GetComponent<PlayerUIScene>().relojGeneral.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
            }
        }


    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene("LoginRoom");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        foreach (string name in namesPlayersInstantiate)
        {
            if (name == otherPlayer.NickName)
            {
                nJugadoresEnEscena -= 1;
                namesPlayersInstantiate.Remove(name);
                break;
            }
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if (sesionTerminada)
        {
            carga.SetActive(false);
            SceneManager.LoadScene("EncuestaFinal");
        }
        else
        {
            SceneManager.LoadScene("LoginRoom");
        }

    }
}
