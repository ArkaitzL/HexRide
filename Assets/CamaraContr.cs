using UnityEngine;
using BaboOnLite;

public partial class CamaraContr {
    [SerializeField] private float distanciaY = 12.5f, distanciaZ = -12.5f; // Velocidad de suavizado
    private Transform seguimiento; // Referencia al transform del personaje
}

public partial class CamaraContr : MonoBehaviour {
    private void OnEnable() {
        seguimiento = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate(){
        // Calcula la nueva posición de la cámara
        if (seguimiento != null) {
            transform.position = new Vector3(
                transform.position.x,
                seguimiento.position.y + distanciaY,
                seguimiento.position.z + distanciaZ
            );
        }
    }
}
