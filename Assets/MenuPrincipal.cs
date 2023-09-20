using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BaboOnLite;

public partial class MenuPrincipal {
    [SerializeField] private TextMeshProUGUI record;
    [SerializeField] private Mutear mutear;
    private GameObject personaje;
}
public partial class MenuPrincipal : MonoBehaviour {
    private void Start() {
        personaje = GameObject.FindGameObjectWithTag("Player");

        //Pone la puntuacion maxima
        record.text = "RECORD: " + Save.Data.record.ToString("D3") + " M"; ;

        //Activar sonido
        Sonidos.get.Boton("Boton");

        //Cambia icono de Mute
        mutear.objeto.sprite = (Save.Data.mute)
            ? mutear.no
            : mutear.si;
    }
}
public partial class MenuPrincipal {
    public void Comenzar() {
        personaje.GetComponent<Personaje>().enabled = true;
    }

    public void Mutear() {
        Save.Data.mute = !Save.Data.mute;
        mutear.objeto.sprite = (Save.Data.mute)
            ? mutear.no 
            : mutear.si;
    }
}
