using UnityEngine;
using BaboOnLite;

public partial class PlataformaPuerta {
    [SerializeField] private Puerta[] puertas;
}
public partial class PlataformaPuerta : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Info de que los pulsado
        Renderer render = GetComponent<Renderer>();
        float factor = .3f;
        render.material.color = new Color(render.material.color.r * factor, render.material.color.g * factor, render.material.color.b * factor, render.material.color.a);

        //Aplicar el movimiento a las puertas
        puertas.ForEach((p) =>
        {
            Controlador.Mover(p.puerta, p.movimiento);
        });

        Destroy(this);
    }
}
public partial class PlataformaPuerta
{

}
