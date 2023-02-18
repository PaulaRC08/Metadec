using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.Unity;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerUIScene : MonoBehaviourPunCallbacks
{
    Hashtable hashPlayer = new Hashtable();

    #region Componentes Jugador
    public ThirdPersonController controladorPersonaje;
    public EnEscenario enEscenario;
    #endregion

    #region Carga
    public GameObject pantallaCarga;
        public GameObject Expulsado;
        public GameObject sinDocente;
    #endregion

    #region Pantalla Principal
    public GameObject pantallaPrincipal;
    public GameObject abrirHipervinculo;
    public GameObject sinMicrofono;
        public Text sinMicrofonoDescripcion;
    public Text relojGeneral;
    #endregion

    #region Opciones Menu
    public GameObject menu;
    public GameObject botonCerrarSala;
    public List<string> unityMic = new List<string>();
    public string[] unitydevice;
    public Dropdown Microfono;
    public bool sinMicrofonos;
    public Slider volumen;
    public Slider Brillo;
    public Image panelBrillo;
    public Dropdown Calidad;
    public List<string> calidadOpctions = new List<string>();
    public int ncalidad;
    #endregion

    #region Reportar
    public GameObject ReporteListaUsuarios;
        public GameObject prefabUsuario;
        public Transform ScrollViewcontentListPlayers;
    public GameObject Reporte;
        public Toggle TcomunicacionOfensiva;
        public Toggle TnombreOfensivo;
        public Toggle TcomportamientoIrrespetuoso;
        public Toggle TAmenaza;
        public InputField DDescricion;
    public Hashtable ReporteUsuario = new Hashtable();
    PhotonView photonViewMasterClient;
    #endregion

    #region Ver Reportes
    public List<Hashtable> listaReportes = new List<Hashtable>();
    public Hashtable verReporte = new Hashtable();
    public GameObject btVerReportes;
    public GameObject pnlListaReportes;
        public GameObject prefabReporte;
        public Transform ScrollViewcontentreport;
    public GameObject vistaReporte;
        public Text Rreportado;
        public Text RquienReporto;
        public Toggle RcomunicacionOfensiva;
        public Toggle RnombreOfensivo;
        public Toggle RcomportamientoIrrespetuoso;
        public Toggle RAmenaza;
        public InputField RDescricion;
        public Dropdown penalizacion;
    public List<string> penalizacionOpctions = new List<string>();
    PhotonView photonViewReportado;
    public bool estaReportadoMicro;

    #endregion

    #region Panel Deportivo
    public GameObject Panelactividad;
        public GameObject esperarjugador;
        public GameObject expJugadorDeportivo;
        public GameObject conversacionDeportivo;
            public Text relojDeportivo;
    #endregion

    #region Panel Bienestar
    public bool isPlayer2;

    public GameObject PanelactividadBienestar;
        public GameObject esperarjugadorBienestar;
        public GameObject expJugadorBienestar;
        public GameObject conversacionBienestar;
    #endregion

    #region Panel Restaurante

    public GameObject PanelactividadRestaurante;
    public GameObject esperarjugadorRestaurante;
    public GameObject expJugadorRestaurante;
    public GameObject conversacionRestaurante;
    #endregion

    #region Puzzles
    public GameObject notReportes;
        public Text txtReportes;
    public GameObject notPuzzles;
    public GameObject listasPuzzles;
        public GameObject prefabPuzzle;
        public Transform ScrollViewcontentListPuzzle;
    public List<Hashtable> listaPuzzles = new List<Hashtable>();
    #endregion

    private void Start()
    {

        hashPlayer = PhotonNetwork.LocalPlayer.CustomProperties;
        hashPlayer.Add("photonViewID", photonView.ViewID);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashPlayer);

        

        Cursor.visible = false;
        pantallaPrincipal.SetActive(true);
        sinMicrofono.SetActive(false);
        abrirHipervinculo.SetActive(false);
        menu.SetActive(false);
        ReporteListaUsuarios.SetActive(false);
        Reporte.SetActive(false);
        pnlListaReportes.SetActive(false);
        vistaReporte.SetActive(false);
        pantallaCarga.SetActive(false);
        Expulsado.SetActive(false);
        sinDocente.SetActive(false);
        notReportes.SetActive(false);
        notPuzzles.SetActive(false);
        listasPuzzles.SetActive(false);

        Panelactividad.SetActive(false);
        esperarjugador.SetActive(false);
        expJugadorDeportivo.SetActive(false);
        conversacionDeportivo.SetActive(false);

        PanelactividadBienestar.SetActive(false);
        esperarjugadorBienestar.SetActive(false);
        expJugadorBienestar.SetActive(false);
        conversacionBienestar.SetActive(false);


        PanelactividadRestaurante.SetActive(false);
        esperarjugadorRestaurante.SetActive(false);
        expJugadorRestaurante.SetActive(false);
        conversacionRestaurante.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            botonCerrarSala.SetActive(true);
            btVerReportes.SetActive(true);
        }
        else
        {
            botonCerrarSala.SetActive(false);
            btVerReportes.SetActive(false);
        }

        if (Microphone.devices.Length == 0)
        {
            enEscenario.photonvoiceRecoder.enabled = false;
        }
        unitydevice = Microphone.devices;
        foreach (var item in Microphone.devices)
        {
            unityMic.Add(item);
        }
        Microfono.ClearOptions();
        Microfono.AddOptions(unityMic);

        volumen.value = PlayerPrefs.GetFloat("volumenAudio", 1f);
        AudioListener.volume = volumen.value;

        Brillo.value = PlayerPrefs.GetFloat("brillo", 1f);
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, 1 - Brillo.value);

        if (PlayerPrefs.GetString("Languaje")=="Español")
        {
            calidadOpctions.Add("Muy Bajo"); calidadOpctions.Add("Bajo"); calidadOpctions.Add("Medio");
            calidadOpctions.Add("Alto"); calidadOpctions.Add("Muy Alto");
        }
        else
        {
            calidadOpctions.Add("Very low"); calidadOpctions.Add("Low"); calidadOpctions.Add("Medium");
            calidadOpctions.Add("High"); calidadOpctions.Add("Very high");
        }
        calidadOpctions.Add("Ultra");
        Calidad.ClearOptions();
        Calidad.AddOptions(calidadOpctions);

        ncalidad = PlayerPrefs.GetInt("numeroDeCalidad", 4);
        Calidad.value = ncalidad;

        if (PlayerPrefs.GetString("Languaje") == "Español")
        {
            penalizacionOpctions.Add("Bloquear Microfono en la sesion"); penalizacionOpctions.Add("Expulsar de la Sala");
            penalizacionOpctions.Add("Expulsar y Bloquear 1 Semana"); penalizacionOpctions.Add("Expulsar y Bloquear 4 semanas");
        }
        else
        {
            penalizacionOpctions.Add("Block microphone in session"); penalizacionOpctions.Add("kick out of room");
            penalizacionOpctions.Add("Kick out and block 1 Week"); penalizacionOpctions.Add("kick out and block 4 Week");
        }
        penalizacion.ClearOptions();
        penalizacion.AddOptions(penalizacionOpctions);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.active || ReporteListaUsuarios.active || Reporte.active || pnlListaReportes.active || vistaReporte.active)
            {
                if (!isPlayer2)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }

                sinMicrofono.SetActive(false);
                menu.SetActive(false);
                ReporteListaUsuarios.SetActive(false);
                pnlListaReportes.SetActive(false);
                Reporte.SetActive(false);
                vistaReporte.SetActive(false);
                controladorPersonaje.enabled = true;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                menu.SetActive(true);
                controladorPersonaje.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (listaPuzzles.Count!=0)
            {
                listasPuzzles.SetActive(true);
            }
        }
        else if(listasPuzzles.active)
        {
            listasPuzzles.SetActive(false);
        }

        if (!estaReportadoMicro)
        {
            if (Microphone.devices.Length != 0)
            {
                Microfono.interactable = true;
                if (sinMicrofonos)
                {
                    unityMic.Clear();
                    sinMicrofono.SetActive(false);
                    foreach (var item in Microphone.devices)
                    {
                        unityMic.Add(item);
                    }
                    Microfono.ClearOptions();
                    Microfono.AddOptions(unityMic);
                    enEscenario.photonvoiceRecoder.enabled = true;
                    enEscenario.photonvoiceRecoder.RestartRecording();
                    sinMicrofonos = false;
                }
                else if (unityMic.Count != Microphone.devices.Length)
                {
                    unityMic.Clear();
                    foreach (var item in Microphone.devices)
                    {
                        unityMic.Add(item);
                    }
                    Microfono.ClearOptions();
                    Microfono.AddOptions(unityMic);
                }
            }
            else
            {
                unityMic.Clear();
                if (PlayerPrefs.GetString("Languaje") == "Español")
                {
                    unityMic.Add("Sin Dispositivos");
                }
                else
                {
                    unityMic.Add("No devices");
                }

                Microfono.ClearOptions();
                Microfono.AddOptions(unityMic);
                sinMicrofono.SetActive(true);
                enEscenario.photonvoiceRecoder.enabled = false;
                Microfono.interactable = false;
                sinMicrofonos = true;
            }
        }
        else
        {
            sinMicrofono.SetActive(true);
            if (PlayerPrefs.GetString("Languaje") == "Español")
            {
                sinMicrofonoDescripcion.text = "Microfono Bloqueado por reporte";
            }
            else
            {
                sinMicrofonoDescripcion.text = "Microphone Blocked by report"; 
            }
               

        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (listaReportes.Count!=0)
            {
                notReportes.SetActive(true);
                txtReportes.text = listaReportes.Count.ToString();
            }
            else
            {
                notReportes.SetActive(false);
            }
        }

        if (listaPuzzles.Count != 0)
        {
            notPuzzles.SetActive(true);
        }
        else
        {
            notPuzzles.SetActive(false);
        }

    }

    #region ReportarUsuario
    public void listUsuarios()
    {
        for (int i = 0; i < ScrollViewcontentListPlayers.childCount; i++)
        {
            Destroy(ScrollViewcontentListPlayers.GetChild(i).gameObject);
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.IsMasterClient && player != PhotonNetwork.LocalPlayer)
            {
                GameObject panelEstudiante = Instantiate(prefabUsuario, ScrollViewcontentListPlayers);
                panelEstudiante.GetComponent<ContentUseraReportar>().playerReportado = player;
            }
        }
    }
    public void volverMenu()
    {
        menu.SetActive(true);
        ReporteListaUsuarios.SetActive(false);
        pnlListaReportes.SetActive(false);
        Reporte.SetActive(false);
    }

    public void abrirReporte()
    {
        Reporte.SetActive(true);
    }

    public void cerrarReporte()
    {
        Reporte.SetActive(false);
    }

    public void enviarReporte()
    {
        ReporteUsuario.Add("Reporto", PhotonNetwork.LocalPlayer);
        ReporteUsuario.Add("cOfensiva", TcomunicacionOfensiva.isOn);
        ReporteUsuario.Add("nOfensivo", TnombreOfensivo.isOn);
        ReporteUsuario.Add("cIrrespetuoso", TcomportamientoIrrespetuoso.isOn);
        ReporteUsuario.Add("amenaza", TAmenaza.isOn);
        ReporteUsuario.Add("descripcion", DDescricion.text);
        if (PhotonNetwork.IsMasterClient)
        {
            listaReportes.Add(ReporteUsuario);
        }
        else
        {
            photonViewMasterClient = PhotonView.Find((int)PhotonNetwork.MasterClient.CustomProperties["photonViewID"]);
            photonViewMasterClient.RPC("enviarReporteMasterClient", RpcTarget.All,false, "Paula", ReporteUsuario);
        }
        listUsuarios();
        Reporte.SetActive(false);
    }

    #endregion

    #region Penalizar Reportes
    public void listReportes()
    {
        for (int i = 0; i < ScrollViewcontentreport.childCount; i++)
        {
            Destroy(ScrollViewcontentreport.GetChild(i).gameObject);
        }
        foreach (Hashtable reporte in listaReportes)
        {
            GameObject panelReporte = Instantiate(prefabReporte, ScrollViewcontentreport);
            panelReporte.GetComponent<contentReporte>().Reporteview = reporte;
        }
    }

    public void llenarVerReporte(Hashtable reporte)
    {
        verReporte = reporte;
        Rreportado.text += (reporte["Reportado"] as Player).NickName;
        RquienReporto.text += (reporte["Reporto"] as Player).NickName;
        RcomunicacionOfensiva.isOn = (bool)reporte["cOfensiva"];
        RnombreOfensivo.isOn = (bool)reporte["nOfensivo"];
        RcomportamientoIrrespetuoso.isOn = (bool)reporte["cIrrespetuoso"];
        RAmenaza.isOn = (bool)reporte["amenaza"];
        RDescricion.text = reporte["descripcion"].ToString();
        vistaReporte.SetActive(true);
    }

    public void penalizar()
    {
        photonViewReportado = PhotonView.Find((int)(verReporte["Reportado"] as Player).CustomProperties["photonViewID"]);
        photonViewReportado.RPC("penalizarReporteaJugador", verReporte["Reportado"] as Player, false, penalizacion.value);
        listaReportes.Remove(verReporte);
        listReportes();
        vistaReporte.SetActive(false);

    }

    public void cerrarVerReporte()
    {
        vistaReporte.SetActive(false);
    }
    #endregion


    #region Opciones de Menu 
    public void changeMicrofono()
    {

        enEscenario.photonvoiceRecoder.MicrophoneDevice = new Photon.Voice.DeviceInfo(unityMic[Microfono.value]);

    }

    public void ChangeSliderVolumen(float valor)
    {
        volumen.value = valor;
        PlayerPrefs.SetFloat("volumenAudio", volumen.value);
        AudioListener.volume = volumen.value;
    }

    public void ChangeSliderBrillo(float valor)
    {
        Brillo.value = valor;
        PlayerPrefs.GetFloat("brillo", 1 - Brillo.value);
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, 1 - Brillo.value);
    }

    public void AjustarCalidad()
    {
        QualitySettings.SetQualityLevel(Calidad.value);
        PlayerPrefs.SetInt("numeroDeCalidad", Calidad.value);
        ncalidad = Calidad.value;
    }

    public void botonReporte()
    {
        listUsuarios();
        ReporteListaUsuarios.SetActive(true);
        menu.SetActive(false);
    }

    public void botonVerReportes()
    {
        listReportes();
        pnlListaReportes.SetActive(true);
        menu.SetActive(false);
    }

    public void cerrarSala()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void salirmenu()
    {
        if (!isPlayer2)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        menu.SetActive(false);
        ReporteListaUsuarios.SetActive(false);
        pnlListaReportes.SetActive(false);
        vistaReporte.SetActive(false);
        Reporte.SetActive(false);
        controladorPersonaje.enabled = true;
    }
    #endregion


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        pantallaCarga.SetActive(true);
        sinDocente.SetActive(true);
        Invoke("LeftRoomMenssage", 3f);
    }

    [PunRPC]
    public void enviarReporteMasterClient(bool cond,string name, Hashtable reporte, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("RCP RECIBIDO");
            this.listaReportes.Add(reporte);
            Debug.Log(name);
        }      
    }
    [PunRPC]
    public void  penalizarReporteaJugador(bool cond, int penalizacion, PhotonMessageInfo info)
    {
        Debug.Log("RCP PENALIZACION RECIBIDO");
        switch (penalizacion)
        {
            case 0:
                Debug.Log("bloquear Micro");
                enEscenario.photonvoiceRecoder.enabled = false;
                estaReportadoMicro = true;
                break;
            case 1:
                Debug.Log("Expulsar de Sala");
                pantallaCarga.SetActive(true);
                Expulsado.SetActive(true);
                Invoke("LeftRoomMenssage", 3f);
                break;
            case 2:
                Debug.Log("Expulsar y bloquear 1 semana");
                pantallaCarga.SetActive(true);
                Expulsado.SetActive(true);
                Invoke("LeftRoomMenssage", 3f);
                break;
            case 3:
                Debug.Log("Expulsar y bloquear 4 semana");
                pantallaCarga.SetActive(true);
                Expulsado.SetActive(true);
                Invoke("LeftRoomMenssage", 3f);
                break;
            default:
                break;
        }
    }

    private void LeftRoomMenssage()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("LoginRoom");
    }
}
