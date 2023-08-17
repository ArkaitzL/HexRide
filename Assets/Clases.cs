using System;
using UnityEngine;
using BaboOnLite;

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