using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NivelesManager : MonoBehaviour
{
    private UserData DatosUsuario;

    public TextMeshProUGUI TxtSiguienteNivel;
    public GameObject VentanaCargando;
    public GameObject PrefabCardNivel;
    public GameObject ContenedorNiveles;

  

    // Start is called before the first frame update
    void Start()
    {
        DatosUsuario = FindObjectOfType<UserData>();
        int NumeroNivel = DatosUsuario.GetSiguienteNivel();
        TxtSiguienteNivel.text = "Nivel " + NumeroNivel.ToString("F0");
        DatosUsuario.NivelJugando = NumeroNivel;
        DatosUsuario.CargarListaNiveles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNiveles()
    {
        foreach (Transform TarjetasLevel in ContenedorNiveles.transform)
        {
            Destroy(TarjetasLevel.gameObject);
        }

        for (int i = 0; i < DatosUsuario.Niveles.Count; i++)
        {
            GameObject NivelRes = Instantiate(PrefabCardNivel, ContenedorNiveles.transform.position, Quaternion.identity);
            NivelRes.transform.SetParent(ContenedorNiveles.transform);
            NivelRes.GetComponent<CardNivel>().RecibirDatosLevel(DatosUsuario.Niveles[i].Nivel, DatosUsuario.Niveles[i].Estrellas, DatosUsuario.Niveles[i].NombreNivel, DatosUsuario.Niveles[i].Desbloqueado);
        }
    }

    public void IniciarNivel(int IdNivel)
    {
        DatosUsuario.NivelJugando = IdNivel;
        VentanaCargando.SetActive(true);
        string NameLevel = DatosUsuario.Niveles.Find(IdBuscar => IdBuscar.Nivel == IdNivel).SceneName;
        SceneManager.LoadScene(NameLevel);
    }

    public void ContinuarSiguienteNivel()
    {
        VentanaCargando.SetActive(true);
        int NumeroSiguiente = DatosUsuario.GetSiguienteNivel();
        DatosUsuario.NivelJugando = NumeroSiguiente;
        string NameSiguiente = DatosUsuario.Niveles.Find(IdBuscar => IdBuscar.Nivel == NumeroSiguiente).SceneName;
        SceneManager.LoadScene(NameSiguiente);
    }
}
