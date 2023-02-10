using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GirarCubo2 : MonoBehaviourPun
{
    int min = 0, seg = 30;
    float restante;
    bool enmarcha;
    Text contador;

    public Camera cameraMain;
    public Camera cameraHablar3;
    public Transform posicionPlayerGanando;
    public Transform posicionPlayerHablar;
    public Transform posicionPlayer;
    public Animator animacion;
    public bool estaElegido;
    public GameObject controlerJuego;
    controlerDeportes controler;
    public GameObject[] otrosCubos;
    public ParticleSystem particulas;
    public Collider collider;
    public Renderer rend;
    public GameObject jugador;
    private void Start()
    {
        collider = GetComponent<Collider>();
        controler = controlerJuego.GetComponent<controlerDeportes>();
        rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.black);
        particulas.Stop();
        restante = (min * 60) + seg;
        enmarcha = false;
    }

    private void Update()
    {
        if (enmarcha)
        {
            restante -= Time.deltaTime;
            if (restante < 1)
            {
                enmarcha = false;
                GanarSacarJugador();
            }
            int tempMin = Mathf.FloorToInt(restante / 60);
            int tempSeg = Mathf.FloorToInt(restante % 60);
            contador.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            jugador = other.gameObject;
            if (other.gameObject.GetComponent<EnEscenario>().photonView.IsMine)
            {
                controler.player2Eligio = true;
                controler.tagElegidoP2 = this.tag;
                controler.elementoElegidoP2 = this.gameObject.GetComponent<GirarCubo2>();
                this.photonView.RPC("DeportesP2", controler.player1, false, this.tag);
                collider.enabled = false;
                animacion.SetBool("pulsar", true);
                foreach (var item in otrosCubos)
                {
                    GirarCubo2 gc = item.GetComponent<GirarCubo2>();
                    gc.collider.enabled = false;
                    gc.rend.material.SetColor("_Color", Color.gray);
                }
                other.GetComponent<ThirdPersonController>().Grounded = true;
                other.GetComponent<ThirdPersonController>().groundedAutomatic();
                other.GetComponent<ThirdPersonController>().enabled = false;
                other.GetComponent<CharacterController>().enabled = false;
                other.GetComponent<CapsuleCollider>().enabled = false;
                other.transform.position = posicionPlayer.position;

            }
        }
    }

    public void activarOtrosCubos(bool sonIguales)
    {
        foreach (var item in otrosCubos)
        {
            GirarCubo2 gc = item.GetComponent<GirarCubo2>();
            gc.collider.enabled = true;
            gc.rend.material.SetColor("_Color", Color.black);
        }
        jugador.GetComponent<ThirdPersonController>().enabled = true;
        jugador.GetComponent<CharacterController>().enabled = true;
        jugador.GetComponent<CapsuleCollider>().enabled = true;

        if (sonIguales)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void activarHablar()
    {
        jugador.transform.position = posicionPlayerHablar.position;
        jugador.GetComponent<ThirdPersonController>().Grounded = true;
        jugador.GetComponent<ThirdPersonController>().groundedAutomatic();
        jugador.GetComponent<CapsuleCollider>().enabled = false;
        jugador.GetComponent<ThirdPersonController>().enabled = false;
        jugador.GetComponent<ThirdPersonController>().MoveSpeed = 0;
        jugador.GetComponent<ThirdPersonController>().JumpHeight = 0;
        jugador.GetComponent<PlayerUIScene>().Panelactividad.SetActive(true);
        jugador.GetComponent<PlayerUIScene>().conversacionDeportivo.SetActive(true);
        jugador.transform.rotation = posicionPlayerHablar.rotation;
        contador = jugador.GetComponent<PlayerUIScene>().relojDeportivo;
        enmarcha = true;
    }

    public void GanarSacarJugador()
    {
        jugador.transform.position = posicionPlayerGanando.position;
        cameraMain.gameObject.SetActive(true);
        cameraHablar3.gameObject.SetActive(false);
        //jugador.GetComponent<ThirdPersonController>().Grounded = true;
        //jugador.GetComponent<ThirdPersonController>().groundedAutomatic();
        jugador.GetComponent<ThirdPersonController>().enabled = true;
        jugador.GetComponent<CharacterController>().enabled = true;
        jugador.GetComponent<CapsuleCollider>().enabled = true;
        jugador.GetComponent<ThirdPersonController>().MoveSpeed = 10;
        jugador.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
        jugador.GetComponent<PlayerUIScene>().Panelactividad.SetActive(false);
        jugador.GetComponent<PlayerUIScene>().conversacionDeportivo.SetActive(false);
        jugador.GetComponent<EnEscenario>().proximityVoice.GetComponent<SphereCollider>().radius = 0.74f;
    }

    [PunRPC]
    public void DeportesP2(bool cond, string eligio, PhotonMessageInfo info)
    {
        controler.player2Eligio = true;
        controler.tagElegidoP2 = eligio;
    }
}
