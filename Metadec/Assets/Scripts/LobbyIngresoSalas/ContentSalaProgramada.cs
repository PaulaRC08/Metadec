using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ContentSalaProgramada : MonoBehaviourPunCallbacks
{
    public GameObject PantallaCarga;
    public Text TextCodigo;
    public Text TextContraseña;
    public Text TextMateria;
    public Text TextFecha;
    string codigoP;
    string contraseñaP;
    string materiaP;
    string fechaP;
    public Button AbrirSala;

    Hashtable hashRoom = new Hashtable();
    

    private void Start()
    {
        PantallaCarga = GameObject.Find("DocIngresoSala");
        cambiarPrefab("QWE123", "123", "CLASS", "30/30/30");
    }

    private void Update()
    {
        if (PantallaCarga == null)
        {
            PantallaCarga = GameObject.Find("DocIngresoSala");
        }
    }

    public void cambiarPrefab(string codigo, string contraseña, string materia, string fecha)
    {
        codigoP = codigo;
        contraseñaP = contraseña;
        materiaP = materia;
        fechaP = fecha;
        TextCodigo.text += codigo;
        TextContraseña.text += contraseña;
        TextMateria.text += materia;
        TextFecha.text += fecha;
        hashRoom.Clear();
        hashRoom.Add("Clase", materia);
        hashRoom.Add("docentesenSala", false);
    }

    public void CreateRoom()
    {
        
        RoomOptions roomOptions = new RoomOptions(){MaxPlayers = 20};
        roomOptions.CustomRoomProperties = hashRoom;
        //roomOptions.CustomRoomPropertiesForLobby = new string[] {"docentesenSala"};
        //PhotonNetwork.CreateRoom(codigoP, roomOptions);
        PhotonNetwork.CreateRoom("123", roomOptions);
        PantallaCarga.GetComponent<DocIngresoSala>().carga.SetActive(true);
        Debug.Log("La sala " + "123" + " a sido creada");
        ;
    }

}
