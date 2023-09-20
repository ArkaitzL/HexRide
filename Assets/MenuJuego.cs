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
    private int puntos = 0;
    private Transform personaje;

    //Muerte
    [HideInInspector] public Vector3 ultimaPos = new Vector3(0,0,0);
    private bool muerto;
    private bool reaparecer = true;

    //Otros
    [SerializeField] private TextMeshProUGUI tutorial;

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

        //Quita el tutorial
        Controlador.Rutina(2.5f, () => {
            Controlador.ColorText(
                tutorial,
                new Color(tutorial.color.r, tutorial.color.g, tutorial.color.b, 0f),
                0.5f
            );
        });

        //Activar sonido
        Sonidos.get.Audio("Musica", .3f, true, Vector3.zero);
    }

    private void Update()
    {
        if (!muerto) {
            if (personaje.position.y < ultimaPosicionY - velocidaPuntos)
            {
                puntuacion.text = (++puntos).ToString("D3") + "m";
                ultimaPosicionY = personaje.position.y;
            }

            if (puntos > Save.Data.record)
            {
                Save.Data.record = puntos;
            }
        }
    }
}

public partial class MenuJuego {
    //Muerte del personaje
    public void Muerte(bool muerte) {
        //Evita que siga haciendo acciones importantes
        Controlador.Rutina(1f, () => {
            if (!reaparecer) {
                menuMuerte.transform.GetChild(0).gameObject.SetActive(false);
            }
            reaparecer = false;
            menuMuerte.SetActive(true);

            if (personaje != null)  {
                Destroy(personaje.gameObject);
                personaje = null;
            }
        });

        Camera.main.gameObject.GetComponent<CamaraContr>().enabled = false;
        muerto = true;

        //Reaparecer
        if (reaparecer) {
            personaje.GetComponent<Controles>().enabled = false;
            personaje.GetComponent<Personaje>().enabled = false;
            Controles.data = null;
        }

        if (muerte) {
            //Lo destruye de ser necesario
            GameObject padre = new GameObject("Muerte");
            for (int i = 0; i < Random.Range(10, 20); i++) {
                Transform objeto = Instantiate(particulasMuerte, personaje.position, Quaternion.identity).transform;
                objeto.SetParent(padre.transform);

                float tamaño = Random.Range(.1f, .6f);
                objeto.localScale = new Vector3(tamaño, tamaño, tamaño);
            }
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

        //Destruye sonidos al reaparecer
        Sonidos.get.Destruir();

        //Tiempo de espera
        personaje.GetComponent<Rigidbody>().isKinematic = true;

        Sonidos.get.Audio("Contador");
        StartCoroutine(Espera(2));
    }

    private IEnumerator Espera(int i) {
        if (i == 0) {
            contadorTiempo.text = $"";
            yield return new WaitForSeconds(.5f);
            personaje.GetComponent<Rigidbody>().isKinematic = false;

            personaje.GetComponent<Rigidbody>().velocity = new Vector3(0, -1, 1) * 1000;
            personaje.GetComponent<Personaje>().Mover(150);

            //Musica
            Sonidos.get.Audio("Musica", .3f, true, Vector3.zero);
        }
        else {
            contadorTiempo.text = $"{i}";

            yield return new WaitForSeconds(1f);
            StartCoroutine(Espera(--i));
        }
    }

    public void Menu() {
        MenuUI.ReiniciarEscena();
        Sonidos.get.Destruir();
    }
}
