using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TiendaManager : MonoBehaviour
{
    private UserData DatosUsuario;
    public int PrecioCorazones;
    public TextMeshProUGUI TxtPrecioCorazones;
    public Button BotonComprar;
    public Sprite CorazonRojo;
    public List<Image> ListaCorazonesTienda = new List<Image>();
    private int CorazonesActuales;
    private int MaxCorazones = 5;

    // Start is called before the first frame update
    void Start()
    {
       DatosUsuario = FindObjectOfType<UserData>();

        if (PlayerPrefs.HasKey("PrefCorazones"))
        {
            CorazonesActuales = PlayerPrefs.GetInt("PrefCorazones");
            DatosUsuario.Corazones = CorazonesActuales;
        }
        else
        {
            CorazonesActuales = 3;
            PlayerPrefs.SetInt("PrefCorazones", CorazonesActuales);
        }

        SetCorazones();
    }

    public void SetCorazones()
    {
        for (int i = 0; i < CorazonesActuales; i++)
        {
            ListaCorazonesTienda[i].sprite = CorazonRojo;
        }

        if (CorazonesActuales == MaxCorazones && DatosUsuario.Monedas >= PrecioCorazones)
        {
            TxtPrecioCorazones.text = "No Disponible";
            BotonComprar.interactable = false;
        }
        else if (CorazonesActuales < MaxCorazones && DatosUsuario.Monedas < PrecioCorazones)
        {
            TxtPrecioCorazones.text = "Insuficiente";
            BotonComprar.interactable = false;
        }
        else if (CorazonesActuales < MaxCorazones && DatosUsuario.Monedas >= PrecioCorazones)
        {
            TxtPrecioCorazones.text = PrecioCorazones.ToString("F0");
            BotonComprar.interactable = true;
        }
    }

    public void ComprarCorazon()
    {
        if (CorazonesActuales < MaxCorazones)
        {
            CorazonesActuales += 1;
            DatosUsuario.Corazones = CorazonesActuales;
            PlayerPrefs.SetInt("PrefCorazones", CorazonesActuales);
            SetCorazones();
        }
        else
        {
            TxtPrecioCorazones.text = "No Disponible";
            BotonComprar.interactable = false;
        }
    }
}
