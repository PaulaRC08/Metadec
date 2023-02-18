using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IniciarHipervinculos : MonoBehaviour
{
    public List<GameObject> tableros;
    public List<hipervinculo> hipervinculos = new List<hipervinculo>();
    public int nHipervinculo;
    public bool ponerHiperviculos;

    private void Start()
    {
        hipervinculo hp1 = new hipervinculo("https://www.ucundinamarca.edu.co/","ucundi",2);
        hipervinculo hp2 = new hipervinculo("https://www.youtube.com/", "youtube", 1);
        hipervinculo hp3 = new hipervinculo("https://www.google.com/", "google", 0);
        hipervinculos.Add(hp1); 
        hipervinculos.Add(hp2); 
        hipervinculos.Add(hp3);


    }

    private void Update()
    {
        nHipervinculo = hipervinculos.Count;
        if (hipervinculos.Count != 0)
        {
            if (!ponerHiperviculos)
            {
                int contador = 0;
                foreach (var item in hipervinculos)
                {
                    tableros[contador].SetActive(true);
                    tableros[contador].GetComponent<tableroHipervinculo>().hipervinculo = item.LinkHipervinculo;
                    tableros[contador].GetComponent<tableroHipervinculo>().tituloHipervinculo = item.TituloHipervinculo;
                    tableros[contador].GetComponent<tableroHipervinculo>().tipoHipervinculo = item.TipoHipervinculo;
                    tableros[contador].GetComponent<tableroHipervinculo>().llenarHipervinculo();
                    contador += 1;
                }
                ponerHiperviculos = true;
            }
        }       
    }

}

public class hipervinculo{
    private string linkHipervinculo;
    private string tituloHipervinculo;
    private int tipoHipervinculo;

    public hipervinculo(string linkHipervinculo, string tituloHipervinculo, int tipoHipervinculo)
    {
        this.linkHipervinculo = linkHipervinculo;
        this.tituloHipervinculo = tituloHipervinculo;
        this.tipoHipervinculo = tipoHipervinculo;

    }

    public string LinkHipervinculo
    {
        get { return this.linkHipervinculo; }  
        set { this.linkHipervinculo = value; }  
    }

    public string TituloHipervinculo
    {
        get { return this.tituloHipervinculo; }
        set { this.tituloHipervinculo = value; }
    }

    public int TipoHipervinculo
    {
        get { return this.tipoHipervinculo; }
        set { this.tipoHipervinculo = value; }
    }
} 