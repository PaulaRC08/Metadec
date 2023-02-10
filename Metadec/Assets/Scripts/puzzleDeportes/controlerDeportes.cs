using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlerDeportes : MonoBehaviour
{
    public Player player1;
    public Player player2;

    public Camera cameraMain;
    public Camera cameraplayer1;
    public Camera cameraplayer2;
    public Camera cameraplayer3;

    public GameObject ingresoPlayer1;
    public GameObject ingresoPlayer2;
    public IngresarAJuego codeIngresoPlayer1;
    public IngresarAJuego2 codeIngresoPlayer2;


    public bool player1Eligio;
    public string tagElegidoP1;
    public GirarCubo elementoElegidoP1;
    public bool player2Eligio;
    public string tagElegidoP2;
    public GirarCubo2 elementoElegidoP2;
    public int contadorAciertos = 8;
    public GameObject canvasJuego;
        public canvasJuegoDeporte juegoDeportes;

    public float Tiempo;

    public Transform posicionPlayer1;
    public Transform posicionPlayer2;

    public GameObject[] todosCubos1;
    public GameObject[] todosCubos2;

    private void Start()
    {
        juegoDeportes = canvasJuego.GetComponent<canvasJuegoDeporte>();
        //codeIngresoPlayer1 = ingresoPlayer1.GetComponent<IngresarAJuego>();
        //codeIngresoPlayer2 = ingresoPlayer2.GetComponent<IngresarAJuego2>();
    }

    private void Update()
    {
        if (juegoDeportes.juegoIniciado)
        {
            if (player1Eligio && player2Eligio)
            {
                Debug.Log("ELIGIERON AMBOS OPCIONES");
                
                if (tagElegidoP1 == tagElegidoP2)
                {
                    Debug.Log("Tags Iguales");
                    if (elementoElegidoP1!=null)
                    {
                        elementoElegidoP1.particulas.GetComponent<ParticleSystem>().Play();
                    }
                    else
                    {
                        elementoElegidoP2.particulas.GetComponent<ParticleSystem>().Play();
                    }
                    StartCoroutine(activarCubos(true, 3f));
                    contadorAciertos++;

                }
                else
                {
                    StartCoroutine(activarCubos(false, 4f));

                }
                player1Eligio = false;
                player2Eligio = false;
            }
        }

        Tiempo += Time.deltaTime;

    }


    IEnumerator activarCubos(bool sonIguales, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (contadorAciertos == 9)
        {
            juegoDeportes.juegoIniciado = false;
            contadorAciertos = 0;
            foreach (var item in todosCubos2)
            {
                GirarCubo2 gc = item.GetComponent<GirarCubo2>();
                gc.collider.enabled = true;
                gc.transform.position = new Vector3(gc.transform.position.x,0f, gc.transform.position.z);
                gc.rend.material.SetColor("_Color", Color.black);
                gc.animacion.SetBool("pulsar", false);
                item.gameObject.SetActive(true);
            }
            foreach (var item in todosCubos1)
            {
                GirarCubo gc = item.GetComponent<GirarCubo>();
                gc.collider.enabled = true;
                gc.transform.position = new Vector3(gc.transform.position.x, 0f, gc.transform.position.z);
                gc.rend.material.SetColor("_Color", Color.black);
                gc.animacion.SetBool("pulsar", false);
                item.gameObject.SetActive(true);
            }
            cameraplayer1.gameObject.SetActive(false);
            cameraplayer2.gameObject.SetActive(false);
            cameraplayer3.gameObject.SetActive(true);
            //cameraMain.gameObject.SetActive(true);
            if (elementoElegidoP1 != null)
            {
                elementoElegidoP1.activarHablar();
            }
            else
            {
                elementoElegidoP2.activarHablar();
            }
            tagElegidoP1 = null;
            tagElegidoP2 = null;
            elementoElegidoP1=null;
            elementoElegidoP2 = null;

            juegoDeportes.juegoLiberado();
        }
        else
        {
            if (sonIguales)
            {
                if (elementoElegidoP1 != null)
                {
                    elementoElegidoP1.activarOtrosCubos(sonIguales);
                }
                else
                {
                    elementoElegidoP2.activarOtrosCubos(sonIguales);
                }
            }
            else
            {
                if (elementoElegidoP1 != null)
                {
                    elementoElegidoP1.activarOtrosCubos(sonIguales);
                    elementoElegidoP1.collider.enabled = true;
                    elementoElegidoP1.animacion.SetBool("pulsar", false);
                }
                else
                {
                    elementoElegidoP2.activarOtrosCubos(sonIguales);
                    elementoElegidoP2.collider.enabled = true;
                    elementoElegidoP2.animacion.SetBool("pulsar", false);
                }
            }
        }
    }
}
