using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasJuegoBienestar : MonoBehaviourPun
{
    public GameObject cuboJugador1;
    public GameObject cuboJugador2;
    public IngresarAJuegoBienestar codeJugador1;
    public IngresarAJuegoBienestar2 codeJugador2;

    public bool jugador1listo;
    public bool jugador2listo;
    public bool juegoIniciado;

    public GameObject libre;
    public GameObject esperando;
    public GameObject ocupado;
    private void Start()
    {
        codeJugador1 = cuboJugador1.GetComponent<IngresarAJuegoBienestar>();
        codeJugador2 = cuboJugador2.GetComponent<IngresarAJuegoBienestar2>();
        esperando.SetActive(false);
        ocupado.SetActive(false);
    }


    public void setJugador1listo(bool value, bool ismine)
    {
        jugador1listo = value;
        if (ismine)
        {
            if (value)
            {
                if (jugador2listo)
                {
                    this.photonView.RPC("cambiarEstadoJuegoBienestar", RpcTarget.All, ismine, 3);
                    juegoIniciado = true;
                    Invoke("iniciarJuego", 1.5f);

                }
                else
                {
                    this.photonView.RPC("cambiarEstadoJuegoBienestar", RpcTarget.All, ismine, 2);
                }
            }
            else if (jugador2listo)
            {
                this.photonView.RPC("cambiarEstadoJuegoBienestar", RpcTarget.All, ismine, 2);
                this.photonView.RPC("cambiarListoJugadorBienestar", RpcTarget.All, false);
            }
            else
            {
                this.photonView.RPC("cambiarEstadoJuegoBienestar", RpcTarget.All, ismine, 1);
                this.photonView.RPC("cambiarListoJugadorBienestar", RpcTarget.All, false);
            }
        }
        else
        {
            if (value)
            {
                if (jugador2listo)
                {
                    juegoIniciado = true;
                    Invoke("iniciarJuego", 1.5f);
                }

            }

        }

    }


    public void setJugador2listo(bool value, bool ismine)
    {
        jugador2listo = value;
        if (ismine)
        {
            if (value)
            {
                if (jugador1listo)
                {
                    this.photonView.RPC("cambiarEstadoJuegoBienestar", RpcTarget.All, ismine, 3);
                    juegoIniciado = true;
                    Invoke("iniciarJuego", 1.5f);
                }
                else
                {
                    this.photonView.RPC("cambiarEstadoJuegoBienestar", RpcTarget.All, ismine, 2);
                }
            }
            else if (jugador1listo)
            {
                this.photonView.RPC("cambiarEstadoJuegoBienestar", RpcTarget.All, ismine, 2);
                this.photonView.RPC("cambiarListoJugadorBienestar2", RpcTarget.All, false);
            }
            else
            {
                this.photonView.RPC("cambiarEstadoJuegoBienestar", RpcTarget.All, ismine, 1);
                this.photonView.RPC("cambiarListoJugadorBienestar2", RpcTarget.All, false);
            }
        }
        else
        {
            if (value)
            {
                if (jugador1listo)
                {
                    juegoIniciado = true;
                    Invoke("iniciarJuego", 1.5f);
                }
            }
        }

    }

    public void iniciarJuego()
    {
        codeJugador1.tranportarjugador();
        codeJugador2.tranportarjugador();
    }

    public void juegoLiberado()
    {
        this.photonView.RPC("cambiarEstadoJuegoBienestar", RpcTarget.All, false, 1);
        jugador1listo = false;
        jugador2listo = false;
    }

    [PunRPC]
    public void cambiarEstadoJuegoBienestar(bool cond, int opc, PhotonMessageInfo info)
    {
        switch (opc)
        {
            case 1:
                codeJugador1.Trigger.enabled = true;
                codeJugador2.Trigger.enabled = true;
                jugador1listo = false;
                jugador2listo = false;
                juegoIniciado = false;
                libre.SetActive(true);
                esperando.SetActive(false);
                ocupado.SetActive(false);
                break;
            case 2:
                libre.SetActive(false);
                esperando.SetActive(true);
                ocupado.SetActive(false);
                break;
            case 3:
                libre.SetActive(false);
                esperando.SetActive(false);
                ocupado.SetActive(true);
                if (!cond)
                {
                    codeJugador1.Trigger.enabled = false;
                    codeJugador2.Trigger.enabled = false;
                }
                break;
        }

    }

    [PunRPC]
    public void cambiarListoJugadorBienestar(bool cond, PhotonMessageInfo info)
    {
        jugador1listo = false;
    }

    [PunRPC]
    public void cambiarListoJugadorBienestar2(bool cond, PhotonMessageInfo info)
    {
        jugador2listo = false;
    }
}
