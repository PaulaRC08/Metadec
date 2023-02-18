using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlerRestaurante : MonoBehaviourPun
{
    public AudioSource audio;
        public AudioClip clipCorrecto;
        public AudioClip clipIncorrecto;
        public AudioClip clipGanador;

    public GameObject canva;
        public GameObject fallo;
        public GameObject reinicioJuego;

    public Player player1;
    public Player player2;
    public GameObject jugador1;
    public GameObject jugador2;

    public Transform posicionPlayer1Ganar;
    public Transform posicionPlayer2Ganar;

    public Camera camer;
    public Camera camer2;
    public Animator animCamer;

    public GameObject MainCamera;

    public GameObject infCafetera;
    public GameObject esperaCafetera;

    public GameObject particleTerminado;

    public bool enCafetera;
    public bool mostroSecuencia;
    public bool falloSecuencia;
    public bool añadircolor=true;
    public int contadorSecuencia;

    public List<GameObject> botonesColores;


    public List<int> secuenciaColoresMostrada;
    public List<int> secuenciaColoresCodificada;
    public List<int> secuenciaColoresElegida;

    public int contadorfallos;
    public int[] sinfallos = new int[] { 2, 4, 3, 1};
    public int[] unfallo = new int[] { 4, 3, 1, 2 };
    public int[] dosfallos = new int[] { 1, 4, 2,3 };

    public GameObject pagina1;
    public GameObject pagina2;

    public Material pag1Español;
    public Material pag2Español;
    public Material pag1Ingles;
    public Material pag2Ingles;

    public GameObject canvasJuego;
    public canvasJuegoRestaurante juegoRestaurante;

    private void Start()
    {
        juegoRestaurante = canvasJuego.GetComponent<canvasJuegoRestaurante>();
        if (PlayerPrefs.GetString("Languaje") == "Español")
        {
            pagina1.GetComponent<Renderer>().material = pag1Español;
            pagina2.GetComponent<Renderer>().material = pag2Español;
        }
        else
        { 
            pagina1.GetComponent<Renderer>().material = pag1Ingles;
            pagina2.GetComponent<Renderer>().material = pag2Ingles;
        }

        canva.SetActive(false);
        fallo.SetActive(false);
        reinicioJuego.SetActive(false);
        animCamer = camer.GetComponent<Animator>();
        
    }

    public void activarCamara()
    {
        animCamer.SetBool("cafetera", true);
    }


    void Update()
    {
        if (PhotonNetwork.LocalPlayer == player1)
        {

            if (Input.GetMouseButtonDown(0))
            {
                SeleccionarElementos();
                Debug.Log("Hola0");
                
            }

            if (enCafetera)
            {
                if (contadorSecuencia == 5)
                {
                    Debug.Log("Ganooooooooooooooooo");
                    
                    animCamer.SetBool("cafetera", false);
                    audio.clip = clipGanador;
                    audio.Play();
                    particleTerminado.GetComponent<ParticleSystem>().Play();
                    enCafetera = false;
                    Invoke("terminarJuego", 3f);
                }
                else
                {
                    if (!mostroSecuencia)
                    {
                        esperaCafetera.SetActive(true);
                        iluminarSecuencia();
                    }
                }
            }

        }
    }

    public void terminarJuego()
    {
        
        jugador1.GetComponent<ThirdPersonController>().Grounded = true;
        jugador1.GetComponent<ThirdPersonController>().groundedAutomatic();
        jugador1.GetComponent<CapsuleCollider>().enabled = false;
        jugador1.GetComponent<ThirdPersonController>().enabled = false;
        jugador1.GetComponent<ThirdPersonController>().MoveSpeed = 0;
        jugador1.GetComponent<ThirdPersonController>().JumpHeight = 0;
        jugador1.transform.position = posicionPlayer2Ganar.position;
        jugador1.transform.position = posicionPlayer2Ganar.position;
        jugador1.GetComponent<ThirdPersonController>().enabled = true;
        jugador1.GetComponent<CapsuleCollider>().enabled = true;
        jugador1.GetComponent<CharacterController>().enabled = true;
        jugador1.GetComponent<ThirdPersonController>().MoveSpeed = 2;
        jugador1.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
        jugador1.GetComponent<PlayerUIScene>().PanelactividadRestaurante.SetActive(false);
        jugador1.GetComponent<PlayerUIScene>().conversacionRestaurante.SetActive(false);
        juegoRestaurante.juegoLiberado();
        jugador1.GetComponent<EnEscenario>().proximityVoice.GetComponent<SphereCollider>().radius = 0.74f;
        particleTerminado.GetComponent<ParticleSystem>().Stop();
        this.photonView.RPC("sacarJugadores", player2, false, "123");
        camer.gameObject.SetActive(false);
        MainCamera.SetActive(true);
    }


    [PunRPC]
    public void sacarJugadores(bool cond, string opc, PhotonMessageInfo info)
    {
        if (player2 == PhotonNetwork.LocalPlayer)
        {
            jugador2.GetComponent<ThirdPersonController>().Grounded = true;
            jugador2.GetComponent<ThirdPersonController>().groundedAutomatic();
            jugador2.GetComponent<CapsuleCollider>().enabled = false;
            jugador2.GetComponent<ThirdPersonController>().enabled = false;
            jugador2.GetComponent<ThirdPersonController>().MoveSpeed = 0;
            jugador2.GetComponent<ThirdPersonController>().JumpHeight = 0;
            jugador2.transform.position = posicionPlayer1Ganar.position;
            secuenciaColoresMostrada.Clear();
            secuenciaColoresElegida.Clear();
            secuenciaColoresCodificada.Clear();
            jugador2.GetComponent<ThirdPersonController>().enabled = true;
            jugador2.GetComponent<CapsuleCollider>().enabled = true;
            jugador2.GetComponent<CharacterController>().enabled = true;
            jugador2.GetComponent<ThirdPersonController>().MoveSpeed = 2;
            jugador2.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
            jugador2.GetComponent<PlayerUIScene>().PanelactividadRestaurante.SetActive(false);
            jugador2.GetComponent<PlayerUIScene>().conversacionRestaurante.SetActive(false);
            jugador2.GetComponent<EnEscenario>().proximityVoice.GetComponent<SphereCollider>().radius = 0.74f;
            camer2.gameObject.SetActive(false);
            MainCamera.SetActive(true);
        }

    }

    public void SeleccionarElementos()
    {

        RaycastHit hit;
        Ray ray = camer.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Colisiono : " + hit.collider.gameObject.name);
            Debug.Log(hit.collider.gameObject.tag);

            if (hit.collider.gameObject.tag == "ficha")
            {

                if (enCafetera)
                {

                    if (mostroSecuencia)
                    {
                        secuenciaColoresElegida.Add(Int32.Parse(hit.collider.gameObject.name));
                        if (secuenciaColoresMostrada.Count == secuenciaColoresElegida.Count)
                        {
                            secuenciaColoresCodificada.Clear();
                            switch (contadorfallos)
                            {
                                case 0:
                                    foreach (var item in secuenciaColoresMostrada)
                                    {
                                        secuenciaColoresCodificada.Add(sinfallos[item]);
                                    }
                                    break;
                                case 1:
                                    foreach (var item in secuenciaColoresMostrada)
                                    {
                                        secuenciaColoresCodificada.Add(unfallo[item]);
                                    }
                                    break;
                                case 2:
                                    foreach (var item in secuenciaColoresMostrada)
                                    {
                                        secuenciaColoresCodificada.Add(dosfallos[item]);
                                    }
                                    break;
                            }
                            for (int i = 0; i < secuenciaColoresMostrada.Count; i++)
                            {
                                if (secuenciaColoresCodificada[i] != secuenciaColoresElegida[i])
                                {
                                    falloSecuencia = true;
                                }
                            }
                            if (falloSecuencia)
                            {
                                audio.clip = clipIncorrecto;
                                audio.Play();
                                if (contadorfallos == 2)
                                {
                                    secuenciaColoresMostrada.Clear();
                                    contadorfallos = 0;
                                    contadorSecuencia = 0;
                                    canva.SetActive(true);
                                    reinicioJuego.SetActive(true);
                                    enCafetera = false;
                                    Invoke("apagarCanva",1f);
                                }
                                else
                                {
                                    fallo.SetActive(true);
                                    Invoke("apagarCanva", 1f);
                                    contadorfallos += 1;
                                    if (contadorSecuencia != 4)
                                    {
                                        contadorSecuencia += 1;
                                    }
                                    else
                                    {
                                        añadircolor = false;
                                    }
                                    //Panel Mal eleccion
                                }
                                secuenciaColoresElegida.Clear();
                                secuenciaColoresCodificada.Clear();
                                mostroSecuencia = false;

                            }
                            else
                            {
                                audio.clip = clipCorrecto;
                                audio.Play();
                                mostroSecuencia = false;
                                secuenciaColoresElegida.Clear();
                                secuenciaColoresCodificada.Clear();
                                contadorSecuencia += 1;
                            }
                                falloSecuencia = false;
                        }
                    }
                    else
                    {
                        esperaCafetera.SetActive(true);
                        iluminarSecuencia();
                    }
                    
                }
            }
        }
    }


    public void apagarCanva()
    {
        canva.SetActive(false);
        fallo.SetActive(false);
        reinicioJuego.SetActive(false);
        Invoke("activarenCafetera", 0.8f);
    }

    public void activarenCafetera()
    {
        enCafetera = true;
    }

    public void iniciarPuzzleCafetera()
    {
        infCafetera.SetActive(false);
        enCafetera = true;
    }

    public void iluminarSecuencia()
    {
        if (secuenciaColoresMostrada.Count == 0)
        {
            int ValorColor = UnityEngine.Random.Range(0, 3);
            secuenciaColoresMostrada.Add(ValorColor);
            botonesColores[ValorColor].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            mostroSecuencia = true;
            StartCoroutine(apagarBoton(ValorColor, true, 1f));
        }
        else
        {
            if (añadircolor)
            {
                int ValorColor = UnityEngine.Random.Range(0, 3);
                secuenciaColoresMostrada.Add(ValorColor);
            }
            mostroSecuencia = true;
            StartCoroutine(secuenciaBotones(secuenciaColoresMostrada, 1f));

        }
    }

    IEnumerator apagarBoton(int indice, bool terminoSecuencia,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        botonesColores[indice].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        mostroSecuencia = terminoSecuencia;
        esperaCafetera.SetActive(false);
    }

    IEnumerator secuenciaBotones(List<int> secuenciaMostrada,float delayTime)
    {
        foreach (int indice in secuenciaMostrada)
        { 
            botonesColores[indice].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(delayTime);
            botonesColores[indice].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(delayTime/2);
        }
        esperaCafetera.SetActive(false);
    }

}