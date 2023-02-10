using Photon.Pun;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngresarAJuego2 : MonoBehaviourPun
{
    public GameObject controlerDeportes;
    public Collider Trigger;
    public GameObject cameraPlayer;
    public GameObject canvasJuego;
    public canvasJuegoDeporte juegoDeportes;
    public GameObject camPlayer2;
    public Transform posicionPlayer2;
    //public bool cambiaronPosicion;
    public GameObject jugador;
    public PhotonView photonjugador;
    public bool ingreso;

    private void Start()
    {
        juegoDeportes = canvasJuego.GetComponent<canvasJuegoDeporte>();
    }

    public void tranportarjugador()
    {
        Debug.Log("TRANSPORTAR JUGADOR");
        if (photonjugador.IsMine)
        {
            Debug.Log("Jugador No Null");
            jugador.GetComponent<CharacterController>().enabled = false;
            jugador.GetComponent<CapsuleCollider>().enabled = false;
            jugador.GetComponent<ThirdPersonController>().enabled = false;
            Debug.Log("jugador Controlador enabled");
            camPlayer2.SetActive(true);
            jugador.transform.rotation = Quaternion.identity;
            jugador.transform.position = posicionPlayer2.position;
            jugador.GetComponent<ThirdPersonController>().enabled = true;
            jugador.GetComponent<CharacterController>().enabled = true;
            jugador.GetComponent<CapsuleCollider>().enabled = true;
            jugador.GetComponent<ThirdPersonController>().MoveSpeed = 10;
            jugador.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
            jugador.GetComponent<EnEscenario>().proximityVoice.GetComponent<SphereCollider>().radius = 50;
            cameraPlayer.transform.rotation = new Quaternion(0, 0, 0, 0);
            cameraPlayer.SetActive(false);
            jugador.GetComponent<PlayerUIScene>().Panelactividad.SetActive(true);
            jugador.GetComponent<PlayerUIScene>().esperarjugador.SetActive(false);
            jugador.GetComponent<PlayerUIScene>().expJugadorDeportivo.SetActive(true);
            Invoke("desactivarExplicacion", 3f);
        }
    }

    public void desactivarExplicacion()
    {
        jugador.GetComponent<PlayerUIScene>().Panelactividad.SetActive(true);
        jugador.GetComponent<PlayerUIScene>().expJugadorDeportivo.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ingreso al Trigger");
        if (other.tag == "Player")
        {
            if (!juegoDeportes.juegoIniciado)
            {
                photonjugador = other.gameObject.GetComponent<EnEscenario>().photonView;
                jugador = other.gameObject;
                juegoDeportes.setJugador2listo(true, photonjugador.IsMine);
                other.GetComponent<ThirdPersonController>().MoveSpeed = 0;
                other.GetComponent<ThirdPersonController>().JumpHeight = 0;
                controlerDeportes.GetComponent<controlerDeportes>().player2 = other.gameObject.GetComponent<PhotonView>().Controller;
                if (photonjugador.IsMine)
                {
                    other.GetComponent<PlayerUIScene>().Panelactividad.SetActive(true);
                    other.GetComponent<PlayerUIScene>().esperarjugador.SetActive(true);
                    ingreso = true;
                }
            }
        }

    }

    private void Update()
    {
        if (ingreso)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                juegoDeportes.setJugador2listo(false, true);
                jugador.GetComponent<ThirdPersonController>().MoveSpeed = 10;
                jugador.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
                controlerDeportes.GetComponent<controlerDeportes>().player2 = null;
                jugador.GetComponent<PlayerUIScene>().Panelactividad.SetActive(false);
                jugador.GetComponent<PlayerUIScene>().esperarjugador.SetActive(false);
                ingreso = false;
            }
        }
    }


}
