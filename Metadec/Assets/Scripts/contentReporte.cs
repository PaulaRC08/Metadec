using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class contentReporte : MonoBehaviourPun
{
    public Hashtable Reporteview = new Hashtable();
    public Text TextNombre;
    public Text TextPais;
    Player reportado;

    string[] paisPlayer;
    public Button btnverReporte;
    public GameObject PlayerUI;

    void Start()
    {
        reportado  = Reporteview["Reportado"] as Player;
        PlayerUI = this.transform.parent.gameObject.GetComponent<contentReport>().PlayerUI;
        TextNombre.text = reportado.NickName;
        paisPlayer = reportado.CustomProperties["pais"].ToString().Split(',');
        if (PlayerPrefs.GetString("Languaje") == "Español")
        {
            TextPais.text = paisPlayer[0];
        }
        else
        {
            TextPais.text = paisPlayer[1];
        }
        btnverReporte.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        PlayerUI.GetComponent<PlayerUIScene>().llenarVerReporte(Reporteview);
    }
}
