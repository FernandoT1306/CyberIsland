using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerfilManager : MonoBehaviour
{
    private int experienciaActual = 0;
    private int experienciaMaxima = 100;
    private bool ActivarCirculoCard;
    private int IndiceFoto;
    private int IndicePerfilFoto;
    private UserData DatosUsuario;


    public int Rank = 1;
    public Slider SliderXP;
    public int Monedas;
    public TextMeshProUGUI TxtMonedas;
    public Image ImagenPerfil;
    public string Nombre;
    public TextMeshProUGUI NombreUsuario;
    public InputField InputNombre;
    public GameObject ContenedorFotos;
    public GameObject PrefabCardFoto;
    List<GameObject> TarjetasFotos = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        DatosUsuario = FindObjectOfType<UserData>();

        if (PlayerPrefs.HasKey("PrefNombre"))
        {
            Nombre = PlayerPrefs.GetString("PrefNombre");
            InputNombre.text = Nombre;
            NombreUsuario.text = Nombre;
            DatosUsuario.NombreUsuario = Nombre;
        }
        else
        {
            Nombre = "Player_1";
            PlayerPrefs.SetString("PrefNombre", Nombre);
            NombreUsuario.text = Nombre;
            DatosUsuario.NombreUsuario = Nombre;
        }

        if (PlayerPrefs.HasKey("PrefFoto"))
        {
            IndicePerfilFoto = PlayerPrefs.GetInt("PrefFoto");
            ImagenPerfil.sprite = DatosUsuario.FotosPerfil[IndicePerfilFoto];
            DatosUsuario.IdFoto = IndicePerfilFoto;
        }
        else
        {
            IndicePerfilFoto = 0;
            PlayerPrefs.SetInt("PrefFoto", IndicePerfilFoto);
        }

        if (PlayerPrefs.HasKey("PrefRank"))
        {
            Rank = PlayerPrefs.GetInt("PrefRank");
        }

        if (PlayerPrefs.HasKey("PrefXp"))
        {
            experienciaActual = PlayerPrefs.GetInt("PrefXp");
            SliderXP.value = experienciaActual;
            DatosUsuario.Experiencia = experienciaActual;
        }

        if (PlayerPrefs.HasKey("PrefMonedas"))
        {
            Monedas = PlayerPrefs.GetInt("PrefMonedas");
            TxtMonedas.text = Monedas.ToString("F0");
            DatosUsuario.Monedas = Monedas;
        }
        else
        {
            Monedas = 0;
            TxtMonedas.text = Monedas.ToString("F0");
            DatosUsuario.Monedas = Monedas;
        }

        LoadExp();
    }

    public void LoadExp()
    {
        SliderXP.maxValue = experienciaMaxima;
        experienciaActual += DatosUsuario.Experiencia;
        SliderXP.value = experienciaActual;

        while (experienciaActual >= experienciaMaxima)
        {
            experienciaActual -= experienciaMaxima;
            Rank++;
            experienciaMaxima += 100;
        }

        PlayerPrefs.SetInt("PrefXp", experienciaActual);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNombre(string NomValue)
    {
        Nombre = NomValue;
        NombreUsuario.text = Nombre;
        DatosUsuario.NombreUsuario = Nombre;
        PlayerPrefs.SetString("PrefNombre", NomValue);
    }

    public void SetImagen(int IndiceFotoRecibido)
    {
        ImagenPerfil.sprite = DatosUsuario.FotosPerfil[IndiceFotoRecibido];
        DatosUsuario.IdFoto = IndiceFotoRecibido;
        IndicePerfilFoto = IndiceFotoRecibido;
        PlayerPrefs.SetInt("PrefFoto", IndiceFotoRecibido);

        for (int i = 0; i < TarjetasFotos.Count; i++)
        {
            if (IndiceFotoRecibido == i)
            {
                TarjetasFotos[i].GetComponent<CardPerfil>().ObjSelect.SetActive(true);
            }
            else
            {
                TarjetasFotos[i].GetComponent<CardPerfil>().ObjSelect.SetActive(false);
            } 
        }
    }

    public void ShowPictures()
    {
        foreach (Transform TarjetasFot in ContenedorFotos.transform)
        {
            Destroy(TarjetasFot.gameObject);
        }

        for (int i = 0; i < DatosUsuario.FotosPerfil.Count; i++)
        {
            if (i == IndicePerfilFoto)
            {
                ActivarCirculoCard = true;
            }
            else
            {
                ActivarCirculoCard = false;
            }

            GameObject FotoRes = Instantiate(PrefabCardFoto, ContenedorFotos.transform.position, Quaternion.identity);
            FotoRes.transform.SetParent(ContenedorFotos.transform);
            FotoRes.GetComponent<CardPerfil>().DatosCard(i, DatosUsuario.FotosPerfil[i]);
            FotoRes.GetComponent<CardPerfil>().ObjSelect.SetActive(ActivarCirculoCard);
            TarjetasFotos.Add(FotoRes);
        }
    }


}
