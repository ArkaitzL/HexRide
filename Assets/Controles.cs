using UnityEngine;
using BaboOnLite;

public partial class Controles {
    [SerializeField] private Dispositivos[] dispositivos; //Tipos de controles: 1:Android1, 2:Android2, 3:PC /// *** Cmbiarlo por un diccionario
    [HideInInspector] public static Controles data; //La instancia estatica

    private Touch tocar; //Se usa para almacenar la posicion que se esta tocando en Android1
}

public partial class Controles : MonoBehaviour {

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
        //Crea la instancia local de la clase
        Instance();
    }
}

public partial class Controles {
    public void RotarPC(Rigidbody rb, KeyCode tecla, int direccion) { //Hace rotar las plataforms[PC]
        if (dispositivos[2].activo)
        {
            if (Input.GetKey(tecla))
            {
                Mover(rb, dispositivos[2].velocidad, direccion);
            }
        }
    }
    public void RotarAndroid1(Rigidbody rb) { //Hace rotar las plataforms[Android1]
        if (dispositivos[0].activo)
        {
            if (Input.touchCount > 0)
            {
                tocar = Input.GetTouch(0);

                if (tocar.phase == TouchPhase.Moved)
                {
                    float direccion = Mathf.Sign(tocar.deltaPosition.x);
                    float rotacion = tocar.deltaPosition.magnitude * dispositivos[0].velocidad;

                    Mover(rb, rotacion, direccion);
                }
            }
        }
    }
    public void RotarAndroid2(Rigidbody rb) { //Hace rotar las plataforms[Android2]
        if (dispositivos[1].activo)
        {
            foreach (Touch tocados in Input.touches)
            {
                Mover(rb, dispositivos[1].velocidad,
                    (tocados.position.x > Screen.width / 2) ? 1 : -1
                );
            }
        }
    }

    private void Mover(Rigidbody rb, float velocidad, float direccion) { //Aplica el moviemiento
        rb.AddForce(
            new Vector3(direccion, 0, 0) * velocidad,
            ForceMode.Force
        );
    }

    //PUBLICAS
    public void ControlesAndroid(int tipo) { //Cambia entre los controles de android
        dispositivos[0].activo = false;
        dispositivos[1].activo = false;

        dispositivos[tipo].activo = true;
    }
}
