using Photon.Pun;
using Photon.Realtime;
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
    public GameObject mainCamera;
    public GameObject playerFollow;
    public bool cargandoJugadores = true;
    public int nJugadores;
    public int nJugadoresEnEscena = 0;

    public List<string> avatarUrls = new List<string>();
    public List<string> namesPlayersInstantiate = new List<string>();

    void Start()
    {
        carga.SetActive(true);
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("LoginRoom");

            return;
        }

        Instantiate(mainCamera, Vector2.zero, Quaternion.identity);
        Instantiate(playerFollow, Vector2.zero, Quaternion.identity);
        avatar = PhotonNetwork.Instantiate(playerAvater.name, new Vector3(Random.Range(30f, 58.52f), 2.3f, Random.Range(43.8f, 50f)), Quaternion.identity, 0) as GameObject;
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
                cargarAvatars.GetComponent<ReadyPlayerMe.RuntimeExampleMultiple>().avatarUrls.AddRange(avatarUrls);
                cargarAvatars.GetComponent<ReadyPlayerMe.RuntimeExampleMultiple>().cargarAvatarsEscena();
                cargandoJugadores = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene("LoginRoom");
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
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

}
