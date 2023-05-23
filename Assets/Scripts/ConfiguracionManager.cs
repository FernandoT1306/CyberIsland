using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class ConfiguracionManager : MonoBehaviour
{
    private UserData DatosUsuario;

    [Header("Dificultad")]
    public TextMeshProUGUI TxtDificultad;
    public string[] NivelesDificultad = { "Facil", "Normal", "Dificil", "Experto" };
    private int IndiceDificultad = 0;

    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start()
    {
        DatosUsuario = FindObjectOfType<UserData>();

        if (PlayerPrefs.HasKey("PrefDificultad"))
        {
            IndiceDificultad = PlayerPrefs.GetInt("PrefDificultad");
            DatosUsuario.Dificultad_User = NivelesDificultad[IndiceDificultad];
        }
        else
        {
            IndiceDificultad = 0;
            PlayerPrefs.SetInt("PrefDificultad", IndiceDificultad);
            DatosUsuario.Dificultad_User = NivelesDificultad[IndiceDificultad];
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("PrefVolumen"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("PrefVolumen");
            CambiarVolumen(volumeSlider.value);
        }
        else
        {
            volumeSlider.value = volumeSlider.maxValue;
            PlayerPrefs.SetFloat("PrefVolumen", volumeSlider.value);
            PlayerPrefs.Save();
            CambiarVolumen(volumeSlider.value);

        }

        ActualizarDificultad();
    }



    public void SiguienteDificultad()
    {
        IndiceDificultad++;
        if (IndiceDificultad >= NivelesDificultad.Length)
        {
            IndiceDificultad = 0;
        }

        ActualizarDificultad();
    }

    public void AnteriorDificultad()
    {
        IndiceDificultad--;
        if (IndiceDificultad < 0)
        {
            IndiceDificultad = NivelesDificultad.Length - 1;
        }

        ActualizarDificultad();
    }

    private void ActualizarDificultad()
    {
        TxtDificultad.text = NivelesDificultad[IndiceDificultad];
        DatosUsuario.Dificultad_User = NivelesDificultad[IndiceDificultad];
        PlayerPrefs.SetInt("PrefDificultad", IndiceDificultad);
        PlayerPrefs.Save();
    }

    public void CambiarVolumen(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("PrefVolumen", volume);
            PlayerPrefs.Save();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
