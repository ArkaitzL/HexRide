using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public partial class Generador {
    [SerializeField] private int longNivel = 240; //Tamaño Z de los niveles
    [SerializeField] private int longFragmento = 4; //Cuantos niveles tiene un fragmento
    [SerializeField] private float separacionNiveles = .25f;
    [SerializeField] private int modoPrueba = -1;
    [SerializeField] private Mundo[] mundos;

    private List<GameObject> niveles = new List<GameObject>();
    private float longMundo = 180; //LLeva la cuenta de en que pos Z esta el mundo(Empieza en 180 por el inicio)

    public Mundo mundo { get => mundos[Save.Data.mundo]; }
    public static Generador data;
}
public partial class Generador : MonoBehaviour {
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

    private void Start() {
        //Instancia la clase
        Instance();

        //Coloca la textura a los iniciales
        GameObject.FindGameObjectWithTag("Inicio").GetComponentsInChildren<Transform>(true).ForEach((trans) => {
            Textura(trans.transform);
        });

        //Genera los prefabs aleatoriamente dependiendo el mundo
        for (int i = 0; i < 3; i++){
            GenerarFragmento();
        }
    }
}

public partial class Generador {
    public void Siguiente() {
        //Destruye fragmento anterior
        for (int i = 0; i < longFragmento; i++) {
            Destroy(niveles[0]);
            niveles.RemoveAt(0);
        }

        //Genera un nuevo fragmento
        GenerarFragmento();
    }

    private void GenerarFragmento() {
        List<int> random = new List<int>( //Genera una Lista
              System.Linq.Enumerable.Range( //Con los numeros del 0 a la cantidad de niveles
                  0,
                  mundo.niveles.Length
              )
          );

        for (int i = 0; i < longFragmento; i++) {
            //Ajustar para que no se bugeen los Colliders
            longMundo += separacionNiveles;

            //Saca un nivel random
            Transform nivel = (modoPrueba == 0)
                ? mundo.niveles[RandomNum(random)]
                : mundo.niveles[modoPrueba-1]; 

            Transform instancia = Instantiate(nivel, new Vector3(0, 0, longMundo) , Quaternion.identity);
            instancia.SetParent(transform, false);
            niveles.Add(instancia.gameObject);

            //Crea los puntos de raparicion
            CrearDetector("Reaparecer", true);

            //Sumar longitud del nivel al mundo
            longMundo += longNivel; 

            //Cambiar las texturas
            Transform[] descendientes = instancia.GetComponentsInChildren<Transform>(true);
            descendientes.ForEach((trans) => {
                Textura(trans);
            });

        }

        //Detector de pasar fragmento
        CrearDetector("Detector");
    }

    private void CrearDetector(string tipo, bool principio = false) {
        Transform detector = new GameObject(tipo).transform;
        detector.gameObject.AddComponent<BoxCollider>().isTrigger = true;

        detector.SetParent(transform, false);
        detector.localPosition = new Vector3(0, 0, (principio) 
            ? longMundo - (longNivel / 2)
            : longMundo);
        detector.localScale = new Vector3(100, 100, 1);
        detector.tag = tipo;
    }

    private int RandomNum(List<int> list) {
        //Saca un numero random de la lista y lo elimina para que no se repita
        int index = Random.Range(0, list.Count);
        int numero = list[index];
        list.RemoveAt(index);

        return numero;
    }

    private void Textura(Transform trans) {
        //Cambia la textura a los objetos mediante los tags
        List<string> texturas = new List<string>();
        mundo.texturas.ToList().ForEach((element) => {
            texturas.Add(element.indice);
        });

        texturas.ForEach((textura) => {
            if (trans.CompareTag(textura))
            {

                Material material = mundo.texturas.Get(textura);
                material.mainTextureScale = new Vector2(1, transform.localScale.z / transform.localScale.x);

                trans.GetComponent<Renderer>().material = material;
            }
        });
    }
}

