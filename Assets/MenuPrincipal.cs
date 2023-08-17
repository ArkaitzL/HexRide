using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BaboOnLite;

public partial class MenuPrincipal {
    private GameObject personaje;
}
public partial class MenuPrincipal : MonoBehaviour {
    private void Start() {
        personaje = GameObject.FindGameObjectWithTag("Player");
    }
}
public partial class MenuPrincipal {
    public void Comenzar() {
        personaje.GetComponent<Personaje>().enabled = true;
    }
}
