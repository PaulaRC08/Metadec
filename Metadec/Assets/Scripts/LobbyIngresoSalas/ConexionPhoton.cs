using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class ConexionPhoton : MonoBehaviourPunCallbacks
{

    public GameObject error;
    public GameObject btnerror;
    public Text txError;
    public GameObject Carga;
    bool Desconectado = false;


    void Start()
    {
        btnerror.SetActive(true);
        error.SetActive(false);
        conexionServidorPhoton();
    }

    void Update()
    {
        if (!(PhotonNetwork.IsConnected))
        {
            conexionServidorPhoton();
        }
    }

    void conexionServidorPhoton()
    {
        if (!(PhotonNetwork.IsConnected))
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        if (Desconectado)
        {
            btnerror.SetActive(true);
            error.SetActive(false);
            txError.text = "";
        }
        Desconectado = false;
        Carga.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Se ha conectado al servidor de Photon");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Desconectado = true;
        Debug.Log("Desconetado"+cause);
        error.SetActive(true);
        if (cause.ToString() == "DnsExceptionOnConnect")
        {
            btnerror.SetActive(false);
            txError.text = "SIN INTERNET\nconectese a una red\nReconectando..."; 
        }
        else {
            txError.text = cause.ToString();
        }
        PhotonNetwork.Reconnect();
    }

    public override void OnConnected()
    {
        base.OnConnected();
        if (Desconectado)
        {
            btnerror.SetActive(true);
            error.SetActive(false);
            txError.text = "";
        }
        Debug.Log("Con Internet");
    }
}
