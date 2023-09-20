using BaboOnLite;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SkinAdmin : MonoBehaviour
{
    private Transform personaje;
    private Vector3 tama�o;

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material[] materiales;
}

public partial class SkinAdmin : MonoBehaviour
{
    private void Awake()
    {
        Instanciar<SkinAdmin>.A�adir("Skins", this, gameObject, true);
    }
    private void Start()
    {
        //Recoger tama�o
        GameObject personaje = GameObject.FindGameObjectWithTag("Player");
        if (personaje != null) {
            tama�o = personaje.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size;
        }

        //Cargar
        SceneManager.sceneLoaded += OnEscenaCargada;
        CargarSkin();
    }

    private void OnEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        CargarSkin();
    }

    private void CargarSkin()
    {
        GameObject personajeObj = GameObject.FindGameObjectWithTag("Player");
        if (personajeObj != null)
        {
            Transform personaje = personajeObj.transform.GetChild(0);

            MeshFilter filter = personaje.GetComponent<MeshFilter>();
            MeshRenderer render = personaje.GetComponent<MeshRenderer>();

            // Cambia el tama�o
            //mesh.bounds.size = tama�o;
            personaje.transform.localScale = tama�o / mesh.bounds.size.magnitude;

            // Asigna el nuevo mesh y materiales
            filter.mesh = mesh;
            render.materials = materiales;

            //Aplicar

            render.materials = materiales;
            filter.mesh = mesh;


        }
    }

    public void CambiarSkin(Mesh mesh, Material[] materiales)
    {
        this.mesh = mesh;
        this.materiales = materiales;
    }

}
