using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlerAjedrez : MonoBehaviourPun
{
    public AudioSource audio;
        public AudioClip clipCorrecto;
        public AudioClip clipIncorrecto;
        public AudioClip clipGanador;

    public Player player1;
    public Player player2;
    public GameObject jugador1;
    public GameObject jugador2;

    public Transform posicionPlayer1Ganar;
    public Transform posicionPlayer2Ganar;

    public GameObject MainCamera;

    public Camera camer;
    float distanciaRayo;
    GameObject pieza;
    Vector3 piezaposisioninicial;
    float positionypieza;
    float positionypiezaalta;
    bool estadopulsado;
    int ultimaFichaOrden;
    int contadorSolucionesCorrectas;


    public GameObject canvasJuego;
    public canvasJuegoBienestar juegoBienestar;

    public List<GameObject> Moviles;
    public List<GameObject> Solucion;

    public List<GameObject> chile;
    public List<GameObject> peru;
    public List<GameObject> españa;
    public List<GameObject> india;

    public List<Transform> chilePosition;
    public List<Transform> peruPosition;
    public List<Transform> españaPosition;
    public List<Transform> indiaPosition;

    public List<GameObject> particleGame;

    public GameObject chileTrigger;
    public GameObject indiaTrigger;
    public GameObject españaTrigger;
    public GameObject peruTrigger;

    public string pais;

    private void Start()
    {
        juegoBienestar = canvasJuego.GetComponent<canvasJuegoBienestar>();
    
        foreach (var item in particleGame)
        {
            item.GetComponent<ParticleSystem>().Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SeleccionarPieza();
            Debug.Log("Hola0");
        }
        if (Input.GetMouseButtonUp(0))
        {
            SoltarPieza();
        }
        if (estadopulsado)
        {
            moverPieza();
        }
    }

    public void SeleccionarPieza()
    {
        //Ray rayo = camer.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Input.mousePosition, camer.transform.up*50, Color.red, 5f);

        //Ray rayo = new Ray(transform.position, Vector3.down);       
        RaycastHit hit;
        Ray ray = camer.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Colisiono : " + hit.collider.gameObject.name);
            Debug.Log(hit.collider.gameObject.tag);
            if (hit.collider.gameObject.tag == "piezapuzzle")
            {
                ultimaFichaOrden++;
                estadopulsado = true;
                Debug.Log("Hola2");
                distanciaRayo = hit.distance;
                pieza = hit.collider.gameObject;
                piezaposisioninicial = pieza.transform.position;
                positionypieza = pieza.transform.position.y;
                positionypiezaalta = positionypieza + 0.1f;
                //pieza.GetComponent<Image>() = ultimaFichaOrden;
                Debug.Log(pieza);
            }
        }

        /*
        Ray ray = camer.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            Debug.Log("Hola2");
            Debug.Log("Colisiono : "+ hit.collider.gameObject.name);

            // Do something with the object that was hit by the raycast.
        }*/
    }
    void moverPieza()
    {
        //Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Vector3 limiteRayo = rayo.GetPoint(distanciaRayo);
        //limiteRayo = new Vector3(limiteRayo.x, limiteRayo.y, 0);
        RaycastHit hit;
        Ray ray = camer.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
            pieza.transform.position = new Vector3(hit.point.x, positionypiezaalta, hit.point.z);
        }

        //Debug.Log("Posicionvector:("+Input.mousePosition.x+","+ Input.mousePosition.y+","+ Input.mousePosition.z+")");

        //pieza.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,pieza.transform.position.z);
    }
    void SoltarPieza()
    {
        if (estadopulsado)
        {
            ComprobarDrop();
            estadopulsado = false;
            pieza = null;
        }
    }
    public void ComprobarDrop()
    {
        for (int i = 0; i < Solucion.Count; i++)
        {
            Debug.Log("Hola3");
            if (pieza.name == Solucion[i].name)
            {
                Debug.Log("Hola4");
                if (Vector3.Distance(pieza.transform.position, Solucion[i].transform.position) < 0.5f)
                {
                    if (this.pais == pieza.name)
                    {
                        pieza.transform.position = new Vector3(Solucion[i].transform.position.x, positionypieza, Solucion[i].transform.position.z);
                        audio.clip = clipCorrecto;
                        audio.Play();
                        this.photonView.RPC("tirarFichas", player1, false, pieza.name);
                        tirarFichasMe(pieza.name);
                        contadorSolucionesCorrectas++;
                        Debug.Log(contadorSolucionesCorrectas);
                        Debug.Log("Hola5");
                        if (contadorSolucionesCorrectas == Moviles.Count)
                        {
                            foreach (var item in particleGame)
                            {
                                item.GetComponent<ParticleSystem>().Play();
                            }
                            Debug.Log("Ganaste");
                            Debug.Log(contadorSolucionesCorrectas);
                            audio.clip = clipGanador;
                            audio.Play();
                            Invoke("GanarSacarJugadoresActividad", 3f);

                        }
                        pieza.GetComponent<Collider>().enabled = false;
                    }
                    else {
                        pieza.transform.position = piezaposisioninicial;
                        audio.clip = clipIncorrecto;
                        audio.Play();
                    }
                    break;
                }
                else
                {
                    pieza.transform.position = piezaposisioninicial;
                    audio.clip = clipIncorrecto;
                    audio.Play();
                }
            }
        }
    }

    public void resetearPosicionficha()
    {
        foreach (var ficha in chile)
        {
            ficha.GetComponent<Rigidbody>().isKinematic = true;
        }
        foreach (var ficha in india)
        {
            ficha.GetComponent<Rigidbody>().isKinematic = true;
        }
        foreach (var ficha in españa)
        {
            ficha.GetComponent<Rigidbody>().isKinematic = true;
        }
        foreach (var ficha in peru)
        {
            ficha.GetComponent<Rigidbody>().isKinematic = true;
        }
        for (int i = 0; i < chile.Count; i++)
        {
            chile[i].transform.rotation = chilePosition[i].rotation;
            chile[i].transform.position = chilePosition[i].position;
            chileTrigger.GetComponent<BoxCollider>().enabled = true;
        }
        for (int i = 0; i < india.Count; i++)
        {
            india[i].transform.rotation = indiaPosition[i].rotation;
            india[i].transform.position = indiaPosition[i].position;
            indiaTrigger.GetComponent<BoxCollider>().enabled = true;
        }
        for (int i = 0; i < españa.Count; i++)
        {
            españa[i].transform.rotation = españaPosition[i].rotation;
            españa[i].transform.position = españaPosition[i].position;
            españaTrigger.GetComponent<BoxCollider>().enabled = true;
        }
        for (int i = 0; i < peru.Count; i++)
        {
            peru[i].transform.rotation = peruPosition[i].rotation;
            peru[i].transform.position = peruPosition[i].position;
            peruTrigger.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void GanarSacarJugadoresActividad()
    {
        chileTrigger.GetComponent<BoxCollider>().enabled = true;
        indiaTrigger.GetComponent<BoxCollider>().enabled = true;
        españaTrigger.GetComponent<BoxCollider>().enabled = true;
        peruTrigger.GetComponent<BoxCollider>().enabled = true;
        jugador2.GetComponent<ThirdPersonController>().Grounded = true;
        jugador2.GetComponent<ThirdPersonController>().groundedAutomatic();
        jugador2.GetComponent<CapsuleCollider>().enabled = false;
        jugador2.GetComponent<ThirdPersonController>().enabled = false;
        jugador2.GetComponent<ThirdPersonController>().MoveSpeed = 0;
        jugador2.GetComponent<ThirdPersonController>().JumpHeight = 0;
        jugador2.transform.position = posicionPlayer2Ganar.position;
        jugador2.transform.position = posicionPlayer2Ganar.position;
        //jugador.GetComponent<ThirdPersonController>().Grounded = true;
        //jugador.GetComponent<ThirdPersonController>().groundedAutomatic();
        jugador2.GetComponent<ThirdPersonController>().enabled = true;
        jugador2.GetComponent<CapsuleCollider>().enabled = true;
        jugador2.GetComponent<CharacterController>().enabled = true;
        jugador2.GetComponent<ThirdPersonController>().MoveSpeed = 2;
        jugador2.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
        jugador2.GetComponent<PlayerUIScene>().PanelactividadBienestar.SetActive(false);
        jugador2.GetComponent<PlayerUIScene>().conversacionBienestar.SetActive(false);
        juegoBienestar.juegoLiberado();
        jugador2.GetComponent<EnEscenario>().proximityVoice.GetComponent<SphereCollider>().radius = 0.74f;
        this.photonView.RPC("sacarJugadores", player1, false, "123");
        camer.gameObject.SetActive(false);
        MainCamera.SetActive(true);
    }


    [PunRPC]
    public void sacarJugadores(bool cond, string opc, PhotonMessageInfo info)
    {
        if (player1 == PhotonNetwork.LocalPlayer)
        {
            jugador1.GetComponent<ThirdPersonController>().Grounded = true;
            jugador1.GetComponent<ThirdPersonController>().groundedAutomatic();
            jugador1.GetComponent<CapsuleCollider>().enabled = false;
            jugador1.GetComponent<ThirdPersonController>().enabled = false;
            jugador1.GetComponent<ThirdPersonController>().MoveSpeed = 0;
            jugador1.GetComponent<ThirdPersonController>().JumpHeight = 0;
            jugador1.transform.position = posicionPlayer1Ganar.position;
            resetearPosicionficha();
            //jugador.GetComponent<ThirdPersonController>().Grounded = true;
            //jugador.GetComponent<ThirdPersonController>().groundedAutomatic();
            jugador1.GetComponent<ThirdPersonController>().enabled = true;
            jugador1.GetComponent<CapsuleCollider>().enabled = true;
            jugador1.GetComponent<CharacterController>().enabled = true;
            jugador1.GetComponent<ThirdPersonController>().MoveSpeed = 2;
            jugador1.GetComponent<ThirdPersonController>().JumpHeight = 1.2f;
            jugador1.GetComponent<PlayerUIScene>().PanelactividadBienestar.SetActive(false);
            jugador1.GetComponent<PlayerUIScene>().conversacionBienestar.SetActive(false);
            jugador1.GetComponent<EnEscenario>().proximityVoice.GetComponent<SphereCollider>().radius = 0.74f;
        }

    }


    [PunRPC]
    public void tirarFichas(bool cond, string opc, PhotonMessageInfo info)
    {
        switch (opc)
        {
            case "chile":
                foreach (var ficha in chile)
                {
                    ficha.GetComponent<Rigidbody>().isKinematic = false;
                }
                chileTrigger.GetComponent<Collider>().enabled = false;
                break;
            case "india":
                foreach (var ficha in india)
                {
                    ficha.GetComponent<Rigidbody>().isKinematic = false;
                }
                indiaTrigger.GetComponent<Collider>().enabled = false;
                break;
            case "españa":
                foreach (var ficha in españa)
                {
                    ficha.GetComponent<Rigidbody>().isKinematic = false;
                }
                españaTrigger.GetComponent<Collider>().enabled = false;
                break;
            case "peru":
                foreach (var ficha in peru)
                {
                    ficha.GetComponent<Rigidbody>().isKinematic = false;
                }
                peruTrigger.GetComponent<Collider>().enabled = false;
                break;
        }
        audio.clip = clipCorrecto;
        audio.Play();
    }

    public void tirarFichasMe(string opc)
    {
        switch (opc)
        {
            case "chile":
                chileTrigger.GetComponent<DesbloquearBandera>().bloquearFichas();
                chileTrigger.GetComponent<Collider>().enabled = false;
                break;
            case "india":
                indiaTrigger.GetComponent<DesbloquearBandera>().bloquearFichas();
                indiaTrigger.GetComponent<Collider>().enabled = false;
                break;
            case "españa":
                españaTrigger.GetComponent<DesbloquearBandera>().bloquearFichas();
                españaTrigger.GetComponent<Collider>().enabled = false;
                break;
            case "peru":
                peruTrigger.GetComponent<DesbloquearBandera>().bloquearFichas();
                peruTrigger.GetComponent<Collider>().enabled = false;
                break;
        }
    }

}
