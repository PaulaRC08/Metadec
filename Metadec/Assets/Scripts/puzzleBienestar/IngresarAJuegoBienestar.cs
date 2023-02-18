using Photon.Pun;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngresarAJuegoBienestar : MonoBehaviourPun
{
    public GameObject controlerBienestar;
    public Collider Trigger;
    public GameObject cameraPlayer;
    public GameObject canvasJuego;
    public canvasJuegoBienestar juegoBienestar;
    //public GameObject camPlayer1;
    public Transform posicionPlayer1;
    //public bool cambiaronPosicion;
    public GameObject jugador;
    public PhotonView photonjugador;

    public bool ingreso;
    private void Start()
    {
        juegoBienestar = canvasJuego.GetComponent<canvasJuegoBienestar>();
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
            //camPlayer1.SetActive(true);
            jugador.transform.rotation = Quaternion.identity;
            jugador.transform.position = posicionPlayer1.position;
            Debug.Log("Se supone cambio de posicion");
            jugador.GetComponent<ThirdPersonController>().enabled = true;
            jugador.GetComponent<CharacterController>().enabled = true;
            jugador.GetComponent<CapsuleCollider>().enabled = true;
            jugador.GetComponent<ThirdPersonController>().MoveSpeed = 2;
            jugador.GetComponent<ThirdPersonController>().SprintSpeed = 5.335f;
            jugador.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
            jugador.GetComponent<EnEscenario>().proximityVoice.GetComponent<SphereCollider>().radius = 70;
            cameraPlayer.transform.rotation = new Quaternion(0, 0, 0, 0);
            //cameraPlayer.SetActive(false);
            jugador.GetComponent<PlayerUIScene>().PanelactividadBienestar.SetActive(true);
            jugador.GetComponent<PlayerUIScene>().esperarjugadorBienestar.SetActive(false);
            jugador.GetComponent<PlayerUIScene>().expJugadorBienestar.SetActive(true);
            jugador.GetComponent<PlayerUIScene>().conversacionBienestar.SetActive(true);
            jugador.GetComponent<ThirdPersonController>().MecanicasBloqueadas = false;
            //controlerBienestar.GetComponent<AudioSource>().enabled = true;
            Invoke("desactivarExplicacion", 3f);
            //desactivarExplicacion();
        }
    }

    public void desactivarExplicacion()
    {
        jugador.GetComponent<PlayerUIScene>().expJugadorBienestar.SetActive(false);
        jugador.GetComponent<PlayerUIScene>().conversacionBienestar.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ingreso al Trigger");
        if (other.tag == "Player")
        {
            if (!juegoBienestar.juegoIniciado && !juegoBienestar.jugador1listo)
            {
                photonjugador = other.gameObject.GetComponent<EnEscenario>().photonView;
                jugador = other.gameObject;
                controlerBienestar.GetComponent<controlerAjedrez>().jugador1 = other.gameObject;
                juegoBienestar.setJugador1listo(true, photonjugador.IsMine);
                other.GetComponent<ThirdPersonController>().Grounded = true;
                other.GetComponent<ThirdPersonController>().groundedAutomatic();
                other.GetComponent<ThirdPersonController>().MoveSpeed = 0;
                other.GetComponent<ThirdPersonController>().SprintSpeed = 0;
                other.GetComponent<ThirdPersonController>().JumpHeight = 0;
                other.GetComponent<ThirdPersonController>().MecanicasBloqueadas = true;
                //other.GetComponent<ThirdPersonController>().enabled = false;
                controlerBienestar.GetComponent<controlerAjedrez>().player1 = other.gameObject.GetComponent<PhotonView>().Controller;
                if (photonjugador.IsMine)
                {
                    other.GetComponent<PlayerUIScene>().PanelactividadBienestar.SetActive(true);
                    other.GetComponent<PlayerUIScene>().esperarjugadorBienestar.SetActive(true);
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
                juegoBienestar.setJugador1listo(false, true);
                
                jugador.GetComponent<ThirdPersonController>().MoveSpeed = 2;
                jugador.GetComponent<ThirdPersonController>().SprintSpeed = 5.335f;
                jugador.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
                controlerBienestar.GetComponent<controlerAjedrez>().player1 = null;
                jugador.GetComponent<PlayerUIScene>().PanelactividadBienestar.SetActive(false);
                jugador.GetComponent<PlayerUIScene>().esperarjugadorBienestar.SetActive(false);
                jugador.GetComponent<ThirdPersonController>().MecanicasBloqueadas = false;
                //jugador.GetComponent<ThirdPersonController>().enabled = true;
                ingreso = false;
            }
        }
    }

}