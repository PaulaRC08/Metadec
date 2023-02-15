using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesbloquearBandera : MonoBehaviourPun
{
    public GameObject controlerBienestar;
        public controlerAjedrez controlerAjedrez;

    public GameObject chile;
    public GameObject india;
    public GameObject espa�a;
    public GameObject peru;

    public int pais;

    public GameObject jugadorEnCollider;

    private void Start()
    {
        controlerAjedrez = controlerBienestar.GetComponent<controlerAjedrez>();
        chile.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84,255));
        chile.GetComponent<Collider>().enabled = false;
        india.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84,255));
        india.GetComponent<Collider>().enabled = false;
        espa�a.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84,255));
        espa�a.GetComponent<Collider>().enabled = false;
        peru.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84,255));
        peru.GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {


            if (!other.GetComponent<EnEscenario>().photonView.IsMine)
            {

                if (controlerAjedrez.player1 != null && other.GetComponent<EnEscenario>().photonView.Controller == controlerAjedrez.player1)
                {

                    jugadorEnCollider = other.gameObject;
                    controlerAjedrez.pais = this.name;
                    chile.GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 255));
                    chile.GetComponent<Collider>().enabled = true;
                    india.GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 255));
                    india.GetComponent<Collider>().enabled = true;
                    espa�a.GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 255));
                    espa�a.GetComponent<Collider>().enabled = true;
                    peru.GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 255));
                    peru.GetComponent<Collider>().enabled = true;

                    
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {


            if (!other.GetComponent<EnEscenario>().photonView.IsMine)
            {

                if (controlerAjedrez.player1 != null && other.GetComponent<EnEscenario>().photonView.Controller == controlerAjedrez.player1)
                {
                    controlerAjedrez.pais = this.name;
                    chile.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84, 255));
                    chile.GetComponent<Collider>().enabled = false;
                    india.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84, 255));
                    india.GetComponent<Collider>().enabled = false;
                    espa�a.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84, 255));
                    espa�a.GetComponent<Collider>().enabled = false;
                    peru.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84, 255));
                    peru.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }

    public void bloquearFichas()
    {
        controlerAjedrez.pais = this.name;
        chile.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84, 255));
        chile.GetComponent<Collider>().enabled = false;
        india.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84, 255));
        india.GetComponent<Collider>().enabled = false;
        espa�a.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84, 255));
        espa�a.GetComponent<Collider>().enabled = false;
        peru.GetComponent<Renderer>().material.SetColor("_Color", new Color32(84, 84, 84, 255));
        peru.GetComponent<Collider>().enabled = false;
    }

}
