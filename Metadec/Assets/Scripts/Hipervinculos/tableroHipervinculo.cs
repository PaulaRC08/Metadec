using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tableroHipervinculo : MonoBehaviour
{
    public string hipervinculo;
    public string tituloHipervinculo;
    public int tipoHipervinculo;
    //0 Herramienta
    //1 WebInf
    //2 PlataformaEducativa
    public GameObject canva;
    public Text Titulo;
    public Text tipHipervinculo;

    public Material herramienta;
    public Material webInfo;
    public Material PlataformaEducativa;

    bool abrirHipervinculo;

    public void llenarHipervinculo()
    {
        canva.SetActive(true);
        Titulo.text = tituloHipervinculo;
        switch (tipoHipervinculo)
        {
            case 0:
                if (PlayerPrefs.GetString("Languaje") == "Español")
                {
                    tipHipervinculo.text = "HERRAMIENTA";
                }
                else
                {
                    tipHipervinculo.text = "TOOLS";
                }
                this.gameObject.GetComponent<Renderer>().material = herramienta;
                break;
            case 1:
                if (PlayerPrefs.GetString("Languaje") == "Español")
                {
                    tipHipervinculo.text = "INFO EN LA WEB";
                }
                else
                {
                    tipHipervinculo.text = "INFO ON THE WEB";
                }
                
                this.gameObject.GetComponent<Renderer>().material = webInfo;
                break;
            case 2:
                if (PlayerPrefs.GetString("Languaje") == "Español")
                {
                    tipHipervinculo.text = "PLATAFORMA EDUCATIVA";
                }
                else
                {
                    tipHipervinculo.text = "EDUCATIONAL PLATFORM";
                }
                
                this.gameObject.GetComponent<Renderer>().material = PlataformaEducativa;
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!abrirHipervinculo)
                {
                    Application.OpenURL(hipervinculo);
                    abrirHipervinculo = true;
                }
                
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                abrirHipervinculo = false;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerUIScene>().abrirHipervinculo.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerUIScene>().abrirHipervinculo.SetActive(false);
        }
    }
}
