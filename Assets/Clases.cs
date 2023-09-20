using System;
using UnityEngine;
using BaboOnLite;
using UnityEngine.UI;

[Serializable]
public class Dispositivos { //Controla los diferentes tipos de controles
    public float velocidad;
    public bool activo;
}
[Serializable]
public class Mundo { //Almacena la informacion relacionada con los mundos
    public Transform[] niveles;
    public DictionarySerializable<Material> texturas;
}
[Serializable]
public class Puerta { //Movimiento de la puerta
    public Transform puerta;
    public Movimiento movimiento;
}
[Serializable]
public class Mutear { //Se usa para cambiar la imagen de mute
    public Image objeto;
    public Sprite si, no;
}
