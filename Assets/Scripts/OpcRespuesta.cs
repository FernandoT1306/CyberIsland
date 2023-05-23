using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpcRespuesta : MonoBehaviour
{
    public TextMeshProUGUI TxtRespuesta;
    private int IdRespuesta;
    private TriviaManager Manager;

    // Start is called before the first frame update
    void Start()
    {
        Manager = FindObjectOfType<TriviaManager>();
    }

    public void SetRespuesta(int id, string Texto)
    {
        IdRespuesta = id;
        TxtRespuesta.text = Texto;
    }

    public void SelectCard()
    {
        Manager.RecibirEleccion(IdRespuesta);
    }
}
