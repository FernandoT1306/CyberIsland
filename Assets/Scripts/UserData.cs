using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

[System.Serializable]
public class ListaNiveles
{
    public string SceneName;
    public string NombreNivel;
    public string TextoHistoria;
    public int Nivel;
    public int Estrellas;
    public bool MostrarIntro;
    public bool Desbloqueado;
    public bool Completado;
}

public class UserData : MonoBehaviour
{
    public static UserData Instance = null;

    public string Dificultad_User;
    public int Experiencia;
    public int Monedas;
    public int IdFoto;
    public int Corazones;
    public string NombreUsuario;
    public int NivelJugando;

    public List<Sprite> FotosPerfil = new List<Sprite>();

    public List<ListaNiveles> Niveles = new List<ListaNiveles>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetSiguienteNivel()
    {
        ListaNiveles ultimoDesbloqueado = Niveles.Where(nivel => nivel.Desbloqueado).LastOrDefault();

        if (ultimoDesbloqueado != null)
        {
            return ultimoDesbloqueado.Nivel;
        }

        //Si no encuentra ninguno se marca como default el nivel 1
        return 1;
    }

    public void SumarMonedas(int Cantidad)
    {
        Monedas += Cantidad;
        PlayerPrefs.SetInt("PrefMonedas", Monedas);
        PlayerPrefs.Save();
    }

    public void SetExperiencia(int CantidadXP)
    {
        Experiencia += CantidadXP;
        PlayerPrefs.SetInt("PrefXp", Experiencia);
        PlayerPrefs.Save();
    }

    public void SumarEstrellas(int NivelJugado, int Cantidad)
    {
        Niveles.Find(IdBuscar => IdBuscar.Nivel == NivelJugado).Estrellas = Cantidad;
        GuardarListaNiveles();
    }

    public void CompletarNivel(int NivelJugado)
    {
        Niveles.Find(IdBuscar => IdBuscar.Nivel == NivelJugado).Completado = true;
        int NivelAnterior = NivelJugado;
        int NivelSiguiente = NivelAnterior++;
        Niveles.Find(IdBuscar => IdBuscar.Nivel == NivelSiguiente).Desbloqueado = true;
        GuardarListaNiveles();
    }

    public void GuardarListaNiveles()
    {
        string listaNivelesString = JsonConvert.SerializeObject(Niveles);
        PlayerPrefs.SetString("PrefListaNiveles", listaNivelesString);
        PlayerPrefs.Save();
    }

    public void CargarListaNiveles()
    {
        if (PlayerPrefs.HasKey("PrefListaNiveles"))
        {
            string listaNivelesString = PlayerPrefs.GetString("PrefListaNiveles");
            Niveles = JsonConvert.DeserializeObject<List<ListaNiveles>>(listaNivelesString);
        }
        else
        {
            GuardarListaNiveles();
        }
    }

    public void ResetNivelActual(int Lvl)
    {
        Niveles.Find(IdBuscar => IdBuscar.Nivel == Lvl).Estrellas = 0;
        GuardarListaNiveles();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
