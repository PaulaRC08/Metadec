using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentEstudianteFicha : MonoBehaviour
{

    public Text TextNombre;
    public Text TextPais;
    public string nombre;
    public string pais;

    void Start()
    {
        
    }
    public void cambiarPrefab(string nombrep, string paisp)
    {
        pais = paisp;
        nombre = nombrep;
        TextNombre.text = nombrep;
        TextPais.text = paisp;
    }
}
