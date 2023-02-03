using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentEstudianteFicha : MonoBehaviourPunCallbacks
{

    public Text TextNombre;
    public Text TextPais;
    public Text TextRol;
    public string nombre;
    public string pais;
    string[] paisPlayer;

    public void cambiarPrefab(string nombrep, string paisp,string rol)
    {
        pais = paisp;
        nombre = nombrep;
        TextNombre.text = nombrep;
        TextRol.text = rol;
        paisPlayer = paisp.Split(',');
        if (PlayerPrefs.GetString("Languaje") == "Español")
        {
            TextPais.text = paisPlayer[0];
        }
        else
        {
            TextPais.text = paisPlayer[1];
        }
    }

}
