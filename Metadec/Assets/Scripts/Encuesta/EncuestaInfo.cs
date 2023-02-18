using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EncuestaInfo : MonoBehaviour
{

    #region Pregunta1

    public GameObject Pregunta1;
    public ToggleGroup pregunta1Group;
    public int respuesta;

    #endregion
    #region Pregunta2

    public GameObject Pregunta2;
    public ToggleGroup pregunta2Group;
    public int respuesta2;

    #endregion
    #region Pregunta3

    public GameObject Pregunta3;
    public ToggleGroup pregunta3Group;
    public int respuesta3;

    #endregion
    #region Pregunta4

    public GameObject Pregunta4;
    public ToggleGroup pregunta4Group;
    public int respuesta4;

    #endregion
    #region Pregunta5

    public GameObject Pregunta5;
    public ToggleGroup pregunta5Group;
    public int respuesta5;

    #endregion

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Pregunta1.SetActive(true);
        Pregunta2.SetActive(false);
        Pregunta3.SetActive(false);
        Pregunta4.SetActive(false);
        Pregunta5.SetActive(false);
    }

    public void cambiarPregunta1()
    {
        respuesta = Int32.Parse(pregunta1Group.ActiveToggles().FirstOrDefault().gameObject.tag);
        Pregunta1.SetActive(false);
        Pregunta2.SetActive(true);
    }
    public void cambiarPregunta2()
    {
        respuesta2 = Int32.Parse(pregunta2Group.ActiveToggles().FirstOrDefault().gameObject.tag);
        Pregunta2.SetActive(false);
        Pregunta3.SetActive(true);
    }
    public void cambiarPregunta3()
    {
        respuesta3 = Int32.Parse(pregunta3Group.ActiveToggles().FirstOrDefault().gameObject.tag);
        Pregunta3.SetActive(false);
        Pregunta4.SetActive(true);
    }
    public void cambiarPregunta4()
    {
        respuesta4 = Int32.Parse(pregunta4Group.ActiveToggles().FirstOrDefault().gameObject.tag);
        Pregunta4.SetActive(false);
        Pregunta5.SetActive(true);
    }

    public void cambiarPregunta5()
    {
        respuesta5 = Int32.Parse(pregunta5Group.ActiveToggles().FirstOrDefault().gameObject.tag);
        //Cerrar Guardar Salir
        SceneManager.LoadScene("LoginRoom");
    }

}
