using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPerfil : MonoBehaviour
{
    private PerfilManager PerfilUsuario;
    public GameObject ObjSelect;
    public Image IconoPerfil;
    private int IndiceFoto;

    private void Start()
    {
        PerfilUsuario = FindObjectOfType<PerfilManager>();
    }

    public void DatosCard(int Indice, Sprite Img)
    {
        IndiceFoto = Indice;
        IconoPerfil.sprite = Img;
    }

    public void SeleccionarImagen()
    {
        PerfilUsuario.SetImagen(IndiceFoto);
    }
}
