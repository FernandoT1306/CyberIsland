using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardNivel : MonoBehaviour
{
    private NivelesManager NivelManager;
    public GameObject GreyStars;
    private int Nivel_op;
    public TextMeshProUGUI NombreNivel;
    public TextMeshProUGUI Nivel;
    public GameObject ImgCandado;
    private bool Desbloqueado;

    [Header("Estrellas Grises")]
    public Image Estrella_1;
    public Image Estrella_2;
    public Image Estrella_3;

    [Header("Estrella Doradas")]
    public Sprite Estrella_Dorada;


    // Start is called before the first frame update
    void Start()
    {
        NivelManager = FindObjectOfType<NivelesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecibirDatosLevel(int Lvl, int Stars, string NameLvl, bool Estado)
    {
        Nivel.text = "Nivel " + Lvl.ToString("F0");
        NombreNivel.text = NameLvl;
        Nivel_op = Lvl;
        Desbloqueado = Estado;

        if (Stars == 1)
        {
            Estrella_1.sprite = Estrella_Dorada;
        }
        else if (Stars == 2)
        {
            Estrella_1.sprite = Estrella_Dorada;
            Estrella_2.sprite = Estrella_Dorada;
        }
        else if (Stars == 3)
        {
            Estrella_1.sprite = Estrella_Dorada;
            Estrella_2.sprite = Estrella_Dorada;
            Estrella_3.sprite = Estrella_Dorada;
        }

        if (Desbloqueado)
        {
            ImgCandado.SetActive(false);
        }
        else
        {
            ImgCandado.SetActive(true);
            GreyStars.SetActive(false);
        }

    }

    public void ElegirNivel()
    {
        if (Desbloqueado)
        {
            NivelManager.IniciarNivel(Nivel_op);
        }   
    }
}
