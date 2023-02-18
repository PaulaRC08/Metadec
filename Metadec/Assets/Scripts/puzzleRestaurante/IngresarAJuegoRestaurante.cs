using Photon.Pun;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngresarAJuegoRestaurante : MonoBehaviourPun
{
    public GameObject controlerRestaurante;
    public Collider Trigger;
    public GameObject cameraPlayer;
    public GameObject canvasJuego;
        public canvasJuegoRestaurante juegoRestaurante;
    public GameObject camPlayerGame;
    public Transform posicionPlayer;
    //public bool cambiaronPosicion;
    public GameObject jugador;
    public PhotonView photonjugador;
    public bool ingreso;
    public GameObject gameSound;

    private void Start()
    {
        juegoRestaurante = canvasJuego.GetComponent<canvasJuegoRestaurante>();
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
            camPlayerGame.SetActive(true);
            jugador.transform.rotation = Quaternion.identity;
            jugador.transform.position = posicionPlayer.position;
            jugador.GetComponent<ThirdPersonController>().enabled = false;
            jugador.GetComponent<CharacterController>().enabled = true;
            jugador.GetComponent<CapsuleCollider>().enabled = true;
            jugador.GetComponent<ThirdPersonController>().MoveSpeed = 2;
            jugador.GetComponent<ThirdPersonController>().SprintSpeed = 5.335f;
            jugador.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
            jugador.GetComponent<EnEscenario>().proximityVoice.GetComponent<SphereCollider>().radius = 70;
            cameraPlayer.transform.rotation = new Quaternion(0, 0, 0, 0);
            cameraPlayer.SetActive(false);
            jugador.GetComponent<PlayerUIScene>().PanelactividadRestaurante.SetActive(true);
            jugador.GetComponent<PlayerUIScene>().esperarjugadorRestaurante.SetActive(false);
            jugador.GetComponent<PlayerUIScene>().expJugadorRestaurante.SetActive(true);
            jugador.GetComponent<PlayerUIScene>().conversacionRestaurante.SetActive(true);
            jugador.GetComponent<ThirdPersonController>().MecanicasBloqueadas = false;
            jugador.GetComponent<PlayerUIScene>().isPlayer2 = true;
            //gameSound.GetComponent<AudioSource>().enabled = true;
            Invoke("desactivarExplicacion", 3f);
        }
    }

    public void desactivarExplicacion()
    {
        jugador.GetComponent<PlayerUIScene>().expJugadorRestaurante.SetActive(false);
        jugador.GetComponent<PlayerUIScene>().conversacionRestaurante.SetActive(true);
        camPlayerGame.GetComponent<Animator>().SetBool("cafetera", true);
        controlerRestaurante.GetComponent<controlerRestaurante>().activarCamara();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ingreso al Trigger");
        if (other.tag == "Player")
        {
            if (!juegoRestaurante.juegoIniciado && !juegoRestaurante.jugador1listo)
            {
                photonjugador = other.gameObject.GetComponent<EnEscenario>().photonView;
                jugador = other.gameObject;
                controlerRestaurante.GetComponent<controlerRestaurante>().jugador1 = other.gameObject;
                juegoRestaurante.setJugador1listo(true, photonjugador.IsMine);
                other.GetComponent<ThirdPersonController>().Grounded = true;
                other.GetComponent<ThirdPersonController>().groundedAutomatic();
                other.GetComponent<ThirdPersonController>().MoveSpeed = 0;
                other.GetComponent<ThirdPersonController>().SprintSpeed = 0;
                other.GetComponent<ThirdPersonController>().JumpHeight = 0;
                other.GetComponent<ThirdPersonController>().MecanicasBloqueadas = true;
                controlerRestaurante.GetComponent<controlerRestaurante>().player1 = other.gameObject.GetComponent<PhotonView>().Controller;
                if (photonjugador.IsMine)
                {
                    other.GetComponent<PlayerUIScene>().PanelactividadRestaurante.SetActive(true);
                    other.GetComponent<PlayerUIScene>().esperarjugadorRestaurante.SetActive(true);
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
                juegoRestaurante.setJugador1listo(false, true);

                jugador.GetComponent<ThirdPersonController>().MoveSpeed = 2;
                jugador.GetComponent<ThirdPersonController>().SprintSpeed = 5.335f;
                jugador.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
                controlerRestaurante.GetComponent<controlerRestaurante>().player1 = null;
                jugador.GetComponent<PlayerUIScene>().PanelactividadRestaurante.SetActive(false);
                jugador.GetComponent<PlayerUIScene>().esperarjugadorRestaurante.SetActive(false);
                jugador.GetComponent<ThirdPersonController>().MecanicasBloqueadas = false;
                ingreso = false;
            }
        }
    }


}
