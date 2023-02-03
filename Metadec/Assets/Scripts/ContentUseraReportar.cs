using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentUseraReportar : MonoBehaviourPunCallbacks
{

    public Player playerReportado;
    public Text TextNombre;
    public Text TextPais;

    string[] paisPlayer;
    public GameObject PlayerReport;

    void Start()
    {
        PlayerReport = this.transform.parent.gameObject.GetComponent<contentReport>().PlayerUI;
        TextNombre.text = playerReportado.NickName;
        paisPlayer = playerReportado.CustomProperties["pais"].ToString().Split(',');
        if (PlayerPrefs.GetString("Languaje") == "Español")
        {
            TextPais.text = paisPlayer[0];
        }
        else
        {
            TextPais.text = paisPlayer[1];
        }
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        PlayerReport.GetComponent<PlayerUIScene>().ReporteUsuario.Clear();
        PlayerReport.GetComponent<PlayerUIScene>().ReporteUsuario.Add("Reportado", playerReportado);
        PlayerReport.GetComponent<PlayerUIScene>().Reporte.SetActive(true);
    }

}
