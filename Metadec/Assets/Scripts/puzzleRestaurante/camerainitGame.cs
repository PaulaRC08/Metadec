using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerainitGame : MonoBehaviour
{
    public GameObject controlerRestaurante;
        public controlerRestaurante controler;
    private void Start()
    {
        controler = controlerRestaurante.GetComponent<controlerRestaurante>();
    }
    public void iniciarPuzzleCafetera()
    {
        controler.iniciarPuzzleCafetera();
    }
}
