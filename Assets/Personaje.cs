using BaboOnLite;
using UnityEngine;

public partial class Personaje {
    [SerializeField] private float velocidad, limiteVelocidad;
    private Rigidbody rb;
}
public partial class Personaje : MonoBehaviour {
    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate() {
        //Añade velocidad a la caida
        Mover();

        // Limita la velocidad máxima
        LimitarVelocidadMaxima();

        //Movimiento PC
        Controles.data.RotarPC(rb, KeyCode.D, 1);
        Controles.data.RotarPC(rb, KeyCode.A, -1);

        //Moviemiento ANDROID1
        Controles.data.RotarAndroid1(rb);

        //Moviemiento ANDROID2
        Controles.data.RotarAndroid2(rb);
    }

    //Muerte
    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Muerte")){
            MenuJuego.data.Muerte(true);

            if (Controlador.Esperando("Impacto"))  {
                Sonidos.get.Audio("Impacto", 1f);
                Controlador.IniciarEspera("Impacto", .5f);
            }
               
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Caida")) {
            MenuJuego.data.Muerte(false);

            if (Controlador.Esperando("Caida")) {
                Sonidos.get.Audio("Caida", .3f);
                Controlador.IniciarEspera("Caida", .5f);
            }
         
        }
        //Detecta la colicion contra el detector
        //(El encargado de saber si pasas de fragmento)
        if (other.transform.CompareTag("Detector")) {
            Generador.data.Siguiente();
            Destroy(other.gameObject);
        }
        //(El encargado de saber donde reaparecer)
        if (other.transform.CompareTag("Reaparecer"))  {
            MenuJuego.data.ultimaPos = new Vector3(0, transform.position.y, transform.position.z);
            Destroy(other.gameObject);
        }
    }
}

public partial class Personaje {
    public void Mover(int multiplicador = 1) {
        rb.AddForce(
            new Vector3(0, -1, 1) * (velocidad * multiplicador),
            ForceMode.Force
        );
    }

    private void LimitarVelocidadMaxima()
    {

        // Calcula la velocidad actual y su magnitud
        Vector3 velocidadActual = rb.velocity;
        float magnitudVelocidadActual = velocidadActual.magnitude;

        // Si la magnitud de la velocidad supera la velocidad máxima limita la velocidad
        if (magnitudVelocidadActual > limiteVelocidad)
        {
            Vector3 velocidadLimitada = velocidadActual.normalized * limiteVelocidad;
            rb.velocity = velocidadLimitada;
        }
    }
}
