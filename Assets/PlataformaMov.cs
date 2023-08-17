using UnityEngine;
using BaboOnLite;

public partial class PlataformaMov {
    [SerializeField] float recorridoMax, recorridoMin, velocidad;
    [SerializeField] bool x, y, z;
    private int direccion = 1;
}

public partial class PlataformaMov : MonoBehaviour {
    private void Update()
    {

        float posicion = 0;
        if (x) posicion = transform.localPosition.x;
        if (y) posicion = transform.localPosition.y;
        if (z) posicion = transform.localPosition.z;

        if (posicion < recorridoMin){
            direccion *= -1;
            MoverPos(recorridoMin);
        }
        if (posicion > recorridoMax) {
            direccion *= -1;
            MoverPos(recorridoMax);
        }


        float movimiento = direccion * velocidad;
        transform.Translate(new Vector3(
            x ? movimiento : 0,
            y ? movimiento : 0,
            z ? movimiento : 0
        ) * Time.deltaTime);
    }
}
public partial class PlataformaMov {
    private void MoverPos(float pos) {
        if (x) transform.localPosition = new Vector3(pos, transform.localPosition.y, transform.localPosition.z);
        if (y) transform.localPosition = new Vector3(transform.localPosition.x, pos, transform.localPosition.z);
        if (z) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, pos);
    }
}
