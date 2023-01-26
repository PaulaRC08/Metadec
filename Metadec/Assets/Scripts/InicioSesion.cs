using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class InicioSesion : MonoBehaviourPunCallbacks
{
    public GameObject LobbyControl;
    public GameObject iniciodeSesion;
    public GameObject ingresoSalaEstudiante;
    public GameObject ingresoSalaDocente;
    public GameObject Carga;
    public GameObject errorEmergente;
    public Text txErrorEmergente;
    public GameObject errorInicioSesion;
    public Text txErrorInicioSesion;

    public string usuario;
    public string contrase�a;

    //Datos prueba traidos de servidor externo
    string usuarioReal = "user";
    string contrase�aReal = "123";

    public string nombreJugador = "Pepito Perez";
    public string paisJugador = "CO - Colombia";
    public string rolJugador = "Docente";
    public string avaterCode = "https://api.readyplayer.me/v1/avatars/6375d034152ef07e24257bd3.glb";
    Hashtable hashPlayer = new Hashtable();


    void Start()
    {
        errorInicioSesion.SetActive(false);
    }
    void Update()
    {
       
    }

    public void ingresar()
    {
        if (!(string.IsNullOrEmpty(usuario)) && !(string.IsNullOrEmpty(contrase�a)))
        {
            if (usuario.Equals(usuarioReal))
            {
                if (contrase�a.Equals(contrase�aReal))
                {
                    PlayerPrefs.SetString("NickName", nombreJugador);
                    PhotonNetwork.NickName = nombreJugador;
                    hashPlayer.Clear();
                    hashPlayer.Add("pais", paisJugador);
                    hashPlayer.Add("rol", rolJugador);
                    hashPlayer.Add("avatar", avaterCode);
                    PhotonNetwork.SetPlayerCustomProperties(hashPlayer);
                    if (rolJugador == "Docente")
                    {
                        ingresoSalaDocente.SetActive(true);
                    }
                    else{
                        ingresoSalaEstudiante.SetActive(true);
                    }
                    iniciodeSesion.SetActive(false);
                    Debug.Log("inicio de seion CORRECTO");
                }
                else{
                    errorInicioSesion.SetActive(true);
                    txErrorInicioSesion.text = "CONTRASE�A INCORRECTA";
                    Debug.Log("Contrase�a Incorrecta");
                }
            }
            else{
                errorInicioSesion.SetActive(true);
                txErrorInicioSesion.text = "USUARIO NO EXISTE";
                Debug.Log("Usuario Incorrecto");
            }
        }
        else
        {
            errorInicioSesion.SetActive(true);
            txErrorInicioSesion.text = "CAMPOS VACIOS";
            Debug.Log("Campos Vacios");
        }
    }

    public void setUsuario(string txusuario)
    {
        if (!(string.IsNullOrEmpty(txusuario)))
        {
            usuario = txusuario;
        }
    }

    public void setContrase�a(string txcontrase�a)
    {
        if (!(string.IsNullOrEmpty(txcontrase�a)))
        {
            contrase�a = txcontrase�a;
        }
    }

    public void paginaRegistrarse()
    {
        Application.OpenURL("http://unity3d.com/");
    }

    public void cerrarAplicacion()
    {
        Application.Quit();
    }

}
