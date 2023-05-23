using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


[System.Serializable]
public class PreguntaTrivia
{
    public string pregunta; //es la pregunta que debe ser respondida
    public string[] respuestas; //lista de respuestas
    public int respuestaCorrecta; //Se escribe el indice de la respuesta correcta 
    public string categoria; // la categoria de la pregunta (matematicas, biologia, computacion, etc..)
    public int nivel; // una seccion es un mundo o un nivel
    public string dificultad = "Facil"; // 0=facil, 1=normal, 2=dificil, 3=Experto (la pregunta se mostrara de acuerdo a lo seleccinado en el menu principal)
    public int puntos; // puntaje que otorga la pregunta si se responde correctamente
    public bool respondida; // muestra que la pregunta ya ha sido respondida
}

public class TriviaManager : MonoBehaviour
{
    List<PreguntaTrivia> preguntasFiltradas = new List<PreguntaTrivia>();
    private UserData DatosUsuario;
    private Vector2 PosInicial;
    private JohnMovement Player;
    private int VidaPlayer;
    private float CronomPregunta;
    private float CronomRevivir;
    private float CronomTemporizadorJuego;
    private float CronomAumentoJuego;
    private float CronomTemporizadorPreguntas;
    private bool RelojJuegoActivo;
    private bool RelojPreguntasActivo;
    private bool OpcionElegida;
    private bool JuegoIniciado;
    private int NivelActual;

    [Header("Level Config")]
    public bool Pausa;
    private string Dificultad;
    private int Incorrectas; // Total de respuestas incorrectas
    private int Correctas; //Total de respuestas correctas
    private int Puntos; //Total de puntos obtenidos
    private int MonedasLevel;
    private int EstrellasLevel;
    public float TiempoMostrarRespuesta;
    public float TiempoRevivir;
    public bool TemporizadorPreguntas; // Si la pregunta tiene que responderse en un tiempo maximo
    public float TiempoTemporizadorPreguntas = 4; // tiempo en el que debe responderse la pregunta
    public bool CronometroJuego;
    public float TiempoTemporizadorJuego = 120;
    public float velocidadEscritura = 0.02f;

    [Header("Panel Stats")]
    public GameObject PanelReloj;
    public GameObject ContenedorCorazones;
    public TextMeshProUGUI TxtEstrellas;
    public TextMeshProUGUI TxtMonedas;
    public TextMeshProUGUI TxtNombre;
    public TextMeshProUGUI TxtTiempo;
    public Image ImagenPerfil;
    public Slider SliderVida;
    private int CorazonesLevel;
    public GameObject PrefabCorazon;
    List<GameObject> ListaCorazones = new List<GameObject>();


    [Header("Ventana Intro")]
    public GameObject VentanaIntro;
    public TextMeshProUGUI TxtTitulo;
    public TextMeshProUGUI TxtIntro;
    public GameObject BotonIniciar;

    [Header("Ventana perdiste")]
    public GameObject VentanaPerdiste;
    public TextMeshProUGUI TxtNivel;
    public TextMeshProUGUI TxtPuntos;
    public TextMeshProUGUI TxtCorrectas;

    [Header("Ventana Ganaste")]
    public GameObject VentanaGanaste;
    public TextMeshProUGUI TxtNivelVictoria;
    public TextMeshProUGUI TxtNombreNivelVictoria;
    public TextMeshProUGUI TxtPuntosVictoria;
    public TextMeshProUGUI TxtMonedasVictoria;
    public TextMeshProUGUI TxtAciertosVictoria;
    public TextMeshProUGUI TxtTiempoVictoria;
    public TextMeshProUGUI TxtBonusVictoria;
    public Image Estrella_1;
    public Image Estrella_2;
    public Image Estrella_3;
    public Sprite EstrellaDorada;

    [Header("Ventana preguntas")]
    public GameObject VentanaPregunta;
    public TextMeshProUGUI TxtMensajePanel;
    public TextMeshProUGUI TituloPanel;
    public GameObject BotonesPanel;
    public GameObject ImgPerdiste;
    public GameObject ImgCorrecto;
    public Slider SliderTemporizador;  // Referencia al slider en el inspector
    public GameObject BotonUsarCorazon;


    [Header("Panel Preguntas")]
    public GameObject ContenedorRespuestas;
    public TextMeshProUGUI TxtCatActual;
    public TextMeshProUGUI TxtPuntosPregunta;
    public TextMeshProUGUI TxtPreguntaText;
    public GameObject RespuestasPrefab;
    private int PreguntaActual;
    private int EleccionUsuario;
    private int RespuestaCorrectaActual;
    private int PuntosPregunta;
    List<Button> BotonesRespuestas = new List<Button>();

    public PreguntaTrivia[] preguntas;

    private void Start()
    {
        DatosUsuario = FindObjectOfType<UserData>();
        Dificultad = DatosUsuario.Dificultad_User;
        TxtNombre.text = DatosUsuario.NombreUsuario;
        ImagenPerfil.sprite = DatosUsuario.FotosPerfil[DatosUsuario.IdFoto];
        CorazonesLevel = DatosUsuario.Corazones;
        NivelActual = DatosUsuario.NivelJugando;


        ResetPanelPreguntas();
        FiltrarPreguntas();

        if (CronometroJuego)
        {
            CronomTemporizadorJuego = TiempoTemporizadorJuego;
            CronomAumentoJuego = 0;
            PanelReloj.SetActive(true);
        }
        else
        {
            PanelReloj.SetActive(false);
        }

        if (DatosUsuario.Niveles.Find(IdBuscar => IdBuscar.Nivel == NivelActual).MostrarIntro)
        {
            StartCoroutine(ShowIntro());
        }
        else
        {
            IniciarJuego();
        }

        EstablecerCorazones();
    }

    IEnumerator ShowIntro()
    {
        Pausa = true;
        TxtIntro.text = "";
        VentanaIntro.SetActive(true);
        TxtTitulo.text = DatosUsuario.Niveles.Find(IdBuscar => IdBuscar.Nivel == NivelActual).NombreNivel;
        string MensajeIntro = DatosUsuario.Niveles.Find(IdBuscar => IdBuscar.Nivel == NivelActual).TextoHistoria;
        foreach (char caracter in MensajeIntro)
        {
            TxtIntro.text += caracter;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        BotonIniciar.SetActive(true);
    }

    private void Update()
    {
        if (RelojPreguntasActivo)
        {
            CronomTemporizadorPreguntas -= Time.deltaTime;
            SliderTemporizador.value = CronomTemporizadorPreguntas;

            // Comprobar si se ha agotado el tiempo
            if (CronomTemporizadorPreguntas <= 0)
            {
                StartCoroutine(RetrasoMostrarRespuesta());
            }
        }


        if (RelojJuegoActivo && JuegoIniciado)
        {
            CronomTemporizadorJuego -= Time.deltaTime;
            TxtTiempo.text = CronomTemporizadorJuego.ToString("F0");
            CronomAumentoJuego += Time.deltaTime;
            // Comprobar si se ha agotado el tiempo
            if (CronomTemporizadorJuego <= 0)
            {
                Morir();
            }
        }

        if (Player != null)
        {
            SliderVida.value = Player.Health;
        }

        if (Input.GetKey(KeyCode.KeypadEnter) && !JuegoIniciado)
        {
            IniciarJuego();
        }

    }

    public void IniciarJuego()
    {
        VentanaIntro.SetActive(false);
        JuegoIniciado = true;
        Pausa = false;

        if (CronometroJuego)
        {
            RelojJuegoActivo = true;
        }
    }

    public void FiltrarPreguntas()
    {
        foreach (PreguntaTrivia preguntaObj in preguntas)
        {
            if (preguntaObj.nivel == NivelActual && preguntaObj.dificultad == Dificultad)
            {
                preguntasFiltradas.Add(preguntaObj);
            }
        }
    }

    public void SetPlayer(GameObject PersonajeRecibido)
    {
        Player = PersonajeRecibido.GetComponent<JohnMovement>();
        SliderVida.maxValue = Player.Health;
        VidaPlayer = Player.Health;
        PosInicial = PersonajeRecibido.transform.position;
    }

    public void EstablecerCorazones()
    {
        foreach (Transform TarjetasCorazon in ContenedorCorazones.transform)
        {
            Destroy(TarjetasCorazon.gameObject);
        }

        for (int i = 0; i < CorazonesLevel; i++)
        {
            GameObject CorazonRes = Instantiate(PrefabCorazon, ContenedorCorazones.transform.position, Quaternion.identity);
            CorazonRes.transform.SetParent(ContenedorCorazones.transform);
            ListaCorazones.Add(CorazonRes);
        }
    }

    public void RestarCorazon()
    {
        if (CorazonesLevel >= 1)
        {
            int TotalCorazonesActuales = CorazonesLevel;
            int IndiceRemover = TotalCorazonesActuales -=1;
            GameObject ObjCorazon = ListaCorazones[IndiceRemover];
            ListaCorazones.Remove(ObjCorazon);

            CorazonesLevel -= 1;
            EstablecerCorazones();
            PlayerPrefs.SetInt("PrefCorazones", CorazonesLevel);
        }

        Revivir();
    }

    public void MostrarPreguntaAleatoria()
    {
        if (preguntasFiltradas.Count > 0)
        {
            PreguntaActual = Random.Range(0, preguntasFiltradas.Count);
            PreguntaTrivia pregunta = preguntasFiltradas[PreguntaActual];

            RelojJuegoActivo = false;
            Pausa = true;
            VentanaPregunta.SetActive(true);

            foreach (Transform Tarjetas in ContenedorRespuestas.transform)
            {
                Destroy(Tarjetas.gameObject);
            }

            TxtPreguntaText.text = pregunta.pregunta;
            RespuestaCorrectaActual = pregunta.respuestaCorrecta;
            TxtCatActual.text = pregunta.categoria;
            TxtPuntosPregunta.text = pregunta.puntos.ToString("F0");
            PuntosPregunta = pregunta.puntos;

            for (int j = 0; j < pregunta.respuestas.Length; j++)
            {
                GameObject Res = Instantiate(RespuestasPrefab, ContenedorRespuestas.transform.position, Quaternion.identity);
                Res.transform.SetParent(ContenedorRespuestas.transform);
                Res.GetComponent<OpcRespuesta>().SetRespuesta(j, pregunta.respuestas[j]);
                Button BotonRes = Res.GetComponent<Button>();
                BotonesRespuestas.Add(BotonRes);
            }

            if (TemporizadorPreguntas)
            {
                SliderTemporizador.gameObject.SetActive(true);
                RelojPreguntasActivo = true;
            }

        }
        else
        {
            Morir();
        }

    }

    public void Morir()
    {
        Pausa = true;
        RelojJuegoActivo = false;
        VentanaPerdiste.SetActive(true);
        TxtNivel.text = "Nivel " + NivelActual.ToString("f0");
        TxtCorrectas.text = Correctas.ToString("f0");
        TxtPuntos.text = Puntos.ToString("f0");
        DatosUsuario.ResetNivelActual(NivelActual);
    }

    public void Reiniciar()
    {
        DatosUsuario.ResetNivelActual(NivelActual);
        string nombreEscenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nombreEscenaActual);
    }

    public void RecibirEleccion(int IdCard)
    {
        foreach (Button ListaBoton in BotonesRespuestas)
        {
            ListaBoton.interactable = false;
        }

        if (TemporizadorPreguntas)
        {
            RelojPreguntasActivo = false;
        }

        OpcionElegida = true;
        EleccionUsuario = IdCard;
        StartCoroutine(RetrasoMostrarRespuesta());
        DatosUsuario.SetExperiencia(Puntos);
    }

    IEnumerator RetrasoMostrarRespuesta()
    {
        RelojPreguntasActivo = false;
        TxtMensajePanel.text = "Resultado en: " + CronomPregunta.ToString("f0");
        while (CronomPregunta > 0)
        {
            yield return new WaitForSeconds(1f);
            CronomPregunta -= 1f;
            TxtMensajePanel.text = "Resultado en: " + CronomPregunta.ToString("f0");
        }

 
        if (EleccionUsuario == RespuestaCorrectaActual && OpcionElegida)
        {
            //seccion si el usuario responde CORRECTAMENTE
            Correctas += 1;
            Puntos += PuntosPregunta;
            TituloPanel.color = Color.yellow;
            TituloPanel.text = "Respuesta Correcta";
            ImgCorrecto.SetActive(true);
            TxtMensajePanel.text = "";
            Revivir();
            RemoverPregunta();
        }
        else if (EleccionUsuario != RespuestaCorrectaActual && OpcionElegida)
        {
            //seccion si el usuario responde INCORRECTAMENTE
            Incorrectas += 1;
            TituloPanel.color = Color.red;
            TituloPanel.text = "Respuesta Incorrecta";
            ImgPerdiste.SetActive(true);
            BotonesPanel.SetActive(true);
            TxtMensajePanel.text = "";
            RelojJuegoActivo = false;
            if (CorazonesLevel >= 1)
            {
                BotonUsarCorazon.SetActive(true);
            }
            
        }
        else if (!OpcionElegida)
        {
            //seccion si el usuario NO RESPONDE
            Incorrectas += 1;
            TituloPanel.color = Color.red;
            TituloPanel.text = "No Respondiste";
            ImgPerdiste.SetActive(true);
            BotonesPanel.SetActive(true);
            TxtMensajePanel.text = "";
            RelojJuegoActivo = false;
            if (CorazonesLevel >= 1)
            {
                BotonUsarCorazon.SetActive(true);
            }
        }
    }

    public void Revivir()
    {
        DatosUsuario.Niveles.Find(IdBuscar => IdBuscar.Nivel == NivelActual).MostrarIntro = false;
        StartCoroutine(CerrarYRevivir());
    }

    public void RecogerMoneda()
    {
        MonedasLevel++;
        TxtMonedas.text = MonedasLevel.ToString("F0");
    }

    public void RecogerEstrella()
    {
        EstrellasLevel += 1;
        TxtEstrellas.text = EstrellasLevel.ToString("F0");
    }

    public void NivelCompletado()
    {
        VentanaGanaste.SetActive(true);

        TxtNivelVictoria.text = "Nivel " + NivelActual.ToString("F0");
        TxtNombreNivelVictoria.text = DatosUsuario.Niveles.Find(IdBuscar => IdBuscar.Nivel == NivelActual).NombreNivel;

        if (EstrellasLevel == 1)
        {
            Estrella_1.sprite = EstrellaDorada;
        }
        else if (EstrellasLevel == 2)
        {
            Estrella_1.sprite = EstrellaDorada;
            Estrella_2.sprite = EstrellaDorada;
        }
        else if (EstrellasLevel == 3)
        {
            Estrella_1.sprite = EstrellaDorada;
            Estrella_2.sprite = EstrellaDorada;
            Estrella_3.sprite = EstrellaDorada;
        }

        TxtPuntosVictoria.text = Puntos.ToString("F0");
        TxtMonedasVictoria.text = MonedasLevel.ToString("F0");
        TxtAciertosVictoria.text = Correctas.ToString("F0");
        TxtTiempoVictoria.text = CronomAumentoJuego.ToString("F0");

        float CantidadTime = CronomAumentoJuego;
        int CantidadEnteroBonus = Mathf.CeilToInt(CantidadTime);
        int Bonus = CantidadEnteroBonus + Correctas * 2;
        TxtBonusVictoria.text = Bonus.ToString("F0");

        Pausa = true;
        RelojJuegoActivo = false;

        DatosUsuario.SetExperiencia(Bonus);
        DatosUsuario.SumarEstrellas(NivelActual, EstrellasLevel);
        DatosUsuario.SumarMonedas(MonedasLevel);
        DatosUsuario.CompletarNivel(NivelActual);
    }

    public void IrMenu()
    {
        DatosUsuario.ResetNivelActual(NivelActual);
        SceneManager.LoadScene("Menu");
    }

    public void RemoverPregunta()
    {
        PreguntaTrivia pregunta = preguntasFiltradas[PreguntaActual];
        preguntasFiltradas.Remove(pregunta);
    }

    IEnumerator CerrarYRevivir()
    {
        BotonesPanel.SetActive(false);
        TxtMensajePanel.text = "Continuar partida en: " + CronomRevivir.ToString("f0");
        while (CronomRevivir > 0)
        {
            yield return new WaitForSeconds(1f);
            CronomRevivir -= 1f;
            TxtMensajePanel.text = "Continuar partida en: " + CronomRevivir.ToString("f0");
        }

        Player.Health = VidaPlayer;
        TituloPanel.color = Color.white;
        TituloPanel.text = "Responde correctamente!";
        ImgPerdiste.SetActive(false);
        ImgCorrecto.SetActive(false);
        VentanaPregunta.SetActive(false);
        Pausa = false;
        ResetPanelPreguntas();
    }

    public void IrSiguienteNivel()
    {
       int NivelSiguiente = NivelActual += 1;
        DatosUsuario.NivelJugando = NivelSiguiente;
       string NextLevel = DatosUsuario.Niveles.Find(IdBuscar => IdBuscar.Nivel == NivelSiguiente).SceneName;
       SceneManager.LoadScene(NextLevel);
    }

    public void ResetPanelPreguntas()
    {
        if (CronometroJuego)
        {
            RelojJuegoActivo = true;
        }

        //PANELES
        TituloPanel.color = Color.white;
        TituloPanel.text = "Responde correctamente!";
        TxtMensajePanel.text = "";
        ImgPerdiste.SetActive(false);
        ImgCorrecto.SetActive(false);
        VentanaPregunta.SetActive(false);
        BotonesPanel.SetActive(false);
        CronomRevivir = TiempoRevivir;
        CronomPregunta = TiempoMostrarRespuesta;
        CronomTemporizadorPreguntas = TiempoTemporizadorPreguntas;
        SliderTemporizador.value = TiempoTemporizadorPreguntas;
        SliderTemporizador.maxValue = TiempoTemporizadorPreguntas;
        RelojPreguntasActivo = false;
        SliderTemporizador.gameObject.SetActive(false);

        //PREGUNTAS
        OpcionElegida = false;
        TxtPreguntaText.text = "";
        RespuestaCorrectaActual = 0;
        TxtCatActual.text = "";
        TxtPuntosPregunta.text = "";
        PuntosPregunta = 0;
    }
}
