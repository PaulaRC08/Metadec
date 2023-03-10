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
    public string contraseņa;

    //Datos prueba traidos de servidor externo
    string usuarioReal = "user";
    string contraseņaReal = "123";

    public string nombreJugador = "Pepito Perez";
    public string paisJugador = "EU - Estados Unidos,EU - United States";
    public string rolJugador = "Docente";
    public string avaterCode = "https://api.readyplayer.me/v1/avatars/63d2ccd323fe23d34bf8069f.glb";
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
        if (usuario.Equals("paula"))
        {
            nombreJugador = "Paula";
            paisJugador = "EU - Estados Unidos,EU - United States";
            rolJugador = "Docente";
            avaterCode = "https://models.readyplayer.me/63d2ccd323fe23d34bf8069f.glb";
        }else if (usuario.Equals("juan"))
        {
            nombreJugador = "Juan";
            paisJugador = "CO - Colombia,CO - Colombia";
            rolJugador = "Docente";
            avaterCode = "https://models.readyplayer.me/63d4a314bc1bcc3f933ba207.glb";
        }else if (usuario.Equals("user"))
        {
            nombreJugador = "user";
            paisJugador = "GER - Alemania,GER - Germany";
            rolJugador = "Estudiante";
            avaterCode = "https://models.readyplayer.me/63d317802b5cfca19e1f4358.glb";
        }

            if (!(string.IsNullOrEmpty(usuario)) && !(string.IsNullOrEmpty(contraseņa)))
        {
            if (usuario.Equals("paula") || usuario.Equals("juan") || usuario.Equals("user"))
            {
                if (contraseņa.Equals(contraseņaReal))
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
                    if (PlayerPrefs.GetString("Languaje") == "Espaņol"){
                        txErrorInicioSesion.text = "CONTRASEŅA INCORRECTA";
                    }else{
                        txErrorInicioSesion.text = "INCORRECT PASSWORD";
                    }
                        
                    Debug.Log("Contraseņa Incorrecta");
                }
            }
            else{
                errorInicioSesion.SetActive(true);
                if (PlayerPrefs.GetString("Languaje") == "Espaņol"){
                    txErrorInicioSesion.text = "USUARIO NO EXISTE";
                }else{
                    txErrorInicioSesion.text = "USER DOESN'T EXIST";
                }
                Debug.Log("Usuario Incorrecto");
            }
        }
        else
        {
            errorInicioSesion.SetActive(true);
            if (PlayerPrefs.GetString("Languaje") == "Espaņol"){
                txErrorInicioSesion.text = "CAMPOS VACIOS";
            }else{
                txErrorInicioSesion.text = "EMPTY FIELDS";
            }
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

    public void setContraseņa(string txcontraseņa)
    {
        if (!(string.IsNullOrEmpty(txcontraseņa)))
        {
            contraseņa = txcontraseņa;
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
