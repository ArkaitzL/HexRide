using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BaboOnLite;
using System.Collections;

public partial class MenuJuego {
    [SerializeField] private TextMeshProUGUI puntuacion;
    [SerializeField] private float velocidaPuntos = 50f;
    [SerializeField] private Transform particulasMuerte;
    [SerializeField] private GameObject menuMuerte;
    [SerializeField] private GameObject personajePref;
    [SerializeField] private TextMeshProUGUI contadorTiempo;

    //Puntuacion
    private float ultimaPosicionY;
    private int puntos = 100;
    private Transform personaje;

    //Muerte
    [HideInInspector] public Vector3 ultimaPos = new Vector3(0,0,0);
    private bool muerto;
    private int espera;


    public static MenuJuego data;
}
public partial class MenuJuego : MonoBehaviour {

    void Instance(bool escena = false)
    {
        if (data != null)
        {
            Destroy(gameObject);
            Debug.LogWarning(Errores.get["Global"][0]);
            return;
        }

        data = this;
        if (escena) DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Instancia la clase
        Instance();
        personaje = GameObject.FindGameObjectWithTag("Player").transform;
        ultimaPosicionY = personaje.position.y;
    }

    private void Update()
    {
        if (!muerto) {
            if (personaje.position.y < ultimaPosicionY - velocidaPuntos)
            {
                puntuacion.text = $"{--puntos} m";
                ultimaPosicionY = personaje.position.y;
            }
        }
    }
}

public partial class MenuJuego {
    //Muerte del personaje
    public void Muerte(bool muerte) {
        //Evita que siga haciendo acciones importantes
        Controlador.Rutina(1f, () => {
            menuMuerte.SetActive(true);

            if (personaje != null)  {
                Destroy(personaje.gameObject);
                personaje = null;
            }
        });
        Camera.main.gameObject.GetComponent<CamaraContr>().enabled = false;
        muerto = true;

        //Reaparecer
        personaje.GetComponent<Controles>().enabled = false;
        personaje.GetComponent<Personaje>().enabled = false;
        Controles.data = null;

        if (muerte) {
            //Lo destruye de ser necesario
            Instantiate(particulasMuerte, personaje.position, Quaternion.identity);
            Destroy(personaje.gameObject);
            personaje = null;
        }
    }

    //Reaparecer despues de morir
    public void Reaparecer() {
    
        //Se instancia y se ajustan todos los componentes
        Instantiate(personajePref, ultimaPos, Quaternion.identity);

        personaje = GameObject.FindGameObjectWithTag("Player").transform;
        personaje.GetComponent<Personaje>().enabled = true;

        Camera.main.gameObject.GetComponent<CamaraContr>().enabled = true;
        muerto = false;

        //Tiempo de espera
        personaje.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(Espera(2));
    }

    private IEnumerator Espera(int i) {
        if (i == 0) {
            contadorTiempo.text = $"";
            yield return new WaitForSeconds(.5f);
            personaje.GetComponent<Rigidbody>().isKinematic = false;

            personaje.GetComponent<Rigidbody>().velocity = new Vector3(0, -1, 1) * 1000;
            personaje.GetComponent<Personaje>().Mover(150);
        }
        else {
            contadorTiempo.text = $"{i}";

            yield return new WaitForSeconds(1f);
            StartCoroutine(Espera(--i));
        }
    }
}
