using Photon.Pun;
using Photon.Realtime;
using ReadyPlayerMe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstanciarJugadpr : MonoBehaviourPunCallbacks
{
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
        //avatar = PhotonNetwork.Instantiate(playerAvater.name, new Vector3(441.399994f, 0.0583543777f, 246.270004f), Quaternion.identity, 0) as GameObject;
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
                //cargarAvatars.GetComponent<ReadyPlayerMe.RuntimeExampleMultiple>().cargarAvatarsEscena();
                cargandoJugadores = false;
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
        SceneManager.LoadScene("LoginRoom");
    }
}
