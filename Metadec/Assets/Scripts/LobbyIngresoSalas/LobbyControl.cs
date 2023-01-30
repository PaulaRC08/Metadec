using System.Collections;
using Assets.SimpleLocalization;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyControl : MonoBehaviourPunCallbacks
{
    public GameObject prefabPlayer;
    public GameObject Lenguaje;
        public Dropdown opcionLenguaje;
        public int LenguajePlayer = 0;
        public string LenguajestPlayer;
    public GameObject iniciodeSesion;
    public GameObject ingresoSalaEstudiante;
        public string nombreSala;
        public string contraseñaSala;
    public GameObject ingresoSalaDocente;
        public Transform ScrollViewcontentSalasProgramadas;
        public GameObject prefabSala;
        public GameObject prefabSalaDesh;
    public GameObject sala;
        public Text TxNombreSala;
        public Text TxClase;
        public Text TxNumeroJugadores;
        public GameObject Docente1;
            public Text NombreDocente1;
            public Text PaisDocente1;
        public GameObject Docente2;
            public Text NombreDocente2;
            public Text PaisDocente2;
        public GameObject prefabEstudiante;
        public Transform ScrollViewcontentListPlayers;
    public Button BtnInicio;
    public GameObject Carga;
    public GameObject errorEmergente;
        public Text txErrorEmergente;

    bool isload = false;

    //Datos prueba traidos de servidor externo
    /*public string nombreJugador = "Pepito Perez";
    public string paisJugador = "CO - Colombia";
    public string rolJugador = "Docente";
    public string avaterCode= "https://api.readyplayer.me/v1/avatars/6375d034152ef07e24257bd3.glb";
    */




    void Start()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log(PlayerPrefs.GetString("Languaje"));
        if (PlayerPrefs.GetString("Languaje") == ""){
            Lenguaje.SetActive(true);
        } else { 
            Lenguaje.SetActive(false);
            LocalizationManager.Read();
            LenguajestPlayer = PlayerPrefs.GetString("Languaje");
            LocalizationManager.Language = PlayerPrefs.GetString("Languaje");
        }
        iniciodeSesion.SetActive(true);
        ingresoSalaEstudiante.SetActive(false);
        ingresoSalaDocente.SetActive(false);
        sala.SetActive(false);
            BtnInicio.gameObject.SetActive(false);
            Docente1.SetActive(false);
            Docente2.SetActive(false);
        Carga.SetActive(true);
        errorEmergente.SetActive(false);
    }
    void Update()
    {
        if (isload)
        {
            Carga.SetActive(true);
        }
    }

    public void changeLanguage()
    {
        if (opcionLenguaje.value!=0)
        {
            PlayerPrefs.SetString("Languaje", opcionLenguaje.captionText.text);
            Lenguaje.SetActive(false);
            LocalizationManager.Read();
            LocalizationManager.Language = PlayerPrefs.GetString("Languaje");
            LenguajestPlayer = opcionLenguaje.captionText.text;
            if (opcionLenguaje.value != 1)
            {
                LenguajePlayer = 0;
            }
            else
            {
                LenguajePlayer = 1;
            }
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Carga.SetActive(false);
        errorEmergente.SetActive(true);
        txErrorEmergente.text = message;
    }

    public void joinRoom()
    {
        if (LenguajestPlayer=="Español")
        {
            if (nombreSala != "" && contraseñaSala != "")
            {
                if (contraseñaSala == "123")
                {
                    PhotonNetwork.JoinRoom(nombreSala);
                    Debug.Log("Ingreso a la Sala Correctamente");
                    Carga.SetActive(true);
                }
                else
                {
                    errorEmergente.SetActive(true);
                    txErrorEmergente.text = "Contraseña Incorrecta";
                }
            }
            else
            {
                errorEmergente.SetActive(true);
                txErrorEmergente.text = "Escribe el codigo de la sala para ingresar";
            }
        }
        else
        {
            if (nombreSala != "" && contraseñaSala != "")
            {
                if (contraseñaSala == "123")
                {
                    PhotonNetwork.JoinRoom(nombreSala);
                    Debug.Log("Ingreso a la Sala Correctamente");
                    Carga.SetActive(true);
                }
                else
                {
                    errorEmergente.SetActive(true);
                    txErrorEmergente.text = "Incorrect password";
                }
            }
            else
            {
                errorEmergente.SetActive(true);
                txErrorEmergente.text = "Empty code or password fields";
            }
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Carga.SetActive(false);
        errorEmergente.SetActive(true);
        if (LenguajestPlayer == "Español")
        {
            if (message == "Game does not exist")
            {
                txErrorEmergente.text = "La sala no existe o no ha sido abierta";
            }
            else if (message == "Game closed")
            {
                txErrorEmergente.text = "La Sala ya fue iniciada";
            }
            else
            {
                txErrorEmergente.text = message;
            }
        }
        else
        {

            if (message == "Game does not exist")
            {
                txErrorEmergente.text = "The room does not exist or has not been opened";
            }
            else if (message == "Game closed")
            {
                txErrorEmergente.text = "The room has already started";
            }
            else
            {
                txErrorEmergente.text = message;
            }
        }
    }

    public override void OnJoinedRoom()
    {
        Carga.SetActive(false);
        ingresoSalaEstudiante.SetActive(false);
        ingresoSalaDocente.SetActive(false);
        sala.SetActive(true);
        TxNombreSala.text = PhotonNetwork.CurrentRoom.Name;
        TxClase.text = PhotonNetwork.CurrentRoom.CustomProperties["Clase"].ToString();
        if (PhotonNetwork.IsMasterClient)
        {
            BtnInicio.gameObject.SetActive(true);
            Debug.Log("SE UNIO A LA SALA");
        }
        eliminarListaJugadores();
        listaJugadores();
        Instantiate(prefabPlayer, new Vector3(7.17000008f, 0, 26.2299995f), Quaternion.Euler(0, 194.386f, 0));
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        eliminarListaJugadores();
        listaJugadores();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        eliminarListaJugadores();
        listaJugadores();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        exitRoom();
        errorEmergente.SetActive(true);
        if (LenguajestPlayer == "Español")
        {
            txErrorEmergente.text = "El Docente encargado a salido de la sala";
        }
        else
        {
            txErrorEmergente.text = "The teacher in charge left the room";
        }
    }

    void listaJugadores()
    {
        string[] paisPlayer;
        TxNumeroJugadores.text = PhotonNetwork.PlayerList.Length + "/20";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            paisPlayer = player.CustomProperties["pais"].ToString().Split(',');
            if (player.CustomProperties["rol"].ToString() == "Docente")
            {
                if (NombreDocente1.text == "NOMBRE Y APELLIDO")
                {
                    Docente1.SetActive(true);
                    NombreDocente1.text = player.NickName;
                    if (LenguajestPlayer == "Español"){
                        PaisDocente1.text = paisPlayer[0];
                    }else{
                        PaisDocente1.text = paisPlayer[1];}

                    
                }
                else{
                    Docente2.SetActive(true);
                    NombreDocente2.text = player.NickName;
                    if (LenguajestPlayer == "Español")
                    {
                        PaisDocente2.text = paisPlayer[0];
                    }
                    else
                    {
                        PaisDocente2.text = paisPlayer[1];
                    }
                }
            }
            else{
                Debug.Log("Estudiante");
                GameObject panelEstudiante = Instantiate(prefabEstudiante, ScrollViewcontentListPlayers);
                if (LenguajestPlayer == "Español")
                {
                    panelEstudiante.GetComponent<ContentEstudianteFicha>().cambiarPrefab(player.NickName, paisPlayer[0]);
                }
                else
                {
                    panelEstudiante.GetComponent<ContentEstudianteFicha>().cambiarPrefab(player.NickName, paisPlayer[1]);
                }
                
            }
            
        }
    }

    void eliminarListaJugadores()
    {
        NombreDocente1.text = "NOMBRE Y APELLIDO";
        Docente1.SetActive(false);
        Docente2.SetActive(false);
        for (int i = ScrollViewcontentListPlayers.childCount - 1; i >= 0; i--)
        {
            Destroy(ScrollViewcontentListPlayers.GetChild(i).gameObject);
        }
    }

    public void startMetadec()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isload = true;
            PhotonNetwork.CurrentRoom.IsOpen=false;
            PhotonNetwork.LoadLevel("UdeC");
        }
        //view.RPC("loadlevel", RpcTarget.Others, false, isload);
    }

    public void exitRoom()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["rol"].ToString() == "Docente")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;
                PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;

            }
                ingresoSalaDocente.SetActive(true);
        }
        else
        {
            ingresoSalaEstudiante.SetActive(true);
        }
        sala.SetActive(false);
        PhotonNetwork.LeaveRoom();
        Debug.Log("Ha dejado la Sala");

    }


    public void setCodigoSala(string txsala)
    {
        if (!(string.IsNullOrEmpty(txsala)))
        {
            nombreSala = txsala.ToUpper();
        }
    }
    public void setContraseñaSala(string txcontraseñasala)
    {
        if (!(string.IsNullOrEmpty(txcontraseñasala)))
        {
            contraseñaSala = txcontraseñasala;
        }
    }
    public void cerrarSesion()
    {
        iniciodeSesion.SetActive(true);
        ingresoSalaDocente.SetActive(false);
        ingresoSalaEstudiante.SetActive(false);
    }
    public void cerrarErrorEmergente()
    {
        errorEmergente.SetActive(false);
    }

    [PunRPC]
    public void loadlevel(bool boolean, bool isLoadRPC)
    {
        if (isLoadRPC)
        {
            isload = isLoadRPC;
        }
    }
}
