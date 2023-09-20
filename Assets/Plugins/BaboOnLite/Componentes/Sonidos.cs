using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaboOnLite
{
    [DefaultExecutionOrder(-1)]
    [AddComponentMenu("BaboOnLite/Sonidos")]
    [DisallowMultipleComponent]
    //[HelpURL("")]

    public class Sonidos : MonoBehaviour
    {
        [SerializeField] DictionarySerializable<AudioClip> sonidos;
        bool vibration;

        public static Sonidos get;
        void Instance()
        {
            if (get == null)
            {
                DontDestroyOnLoad(gameObject);
                get = this;
                return;
            }

            //No se puede poner dos scripts de este tipo en la misma escena
            //Debug.LogError($"baboOn: 5.1.-Existen varias instancias de languages, se ha destruido la instancia de \"{gameObject.name}\"");
            Destroy(gameObject);
        }

        void Awake()
        {
            Instance();
        }

        //Añade el sonido a los botones
        public void Boton(string sonido, float volumen = 1) {
            //Sonido botones
            Controlador.Rutina(.1f, () => {

                Button[] botones = FindObjectsOfType<Button>(includeInactive: true);
                botones.ForEach((boton) => {
                    boton.onClick.AddListener(() => Audio(sonido, volumen));
                });

            });
        }

        //Destruye todos los sonidos
        public void Destruir() {
            for (int i = 0; i < transform.childCount; i++) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        //Crea sonidos de una sola vez. Puedes elegir que sonidos y en que posicion
        public AudioSource Audio(string sonido, float volumen = 1, bool bucle = false, Vector3 posicion = default(Vector3))
        {
            return Creator(sonido, posicion, bucle, volumen);
        }
        public AudioSource[] Audio(string[] sonido, float volumen = 1, bool bucle = false, Vector3 posicion = default(Vector3))
        {
            List<AudioSource> miSounds = new List<AudioSource>();
            sonido.ForEach((i) => {
                miSounds.Add(Creator(i, posicion, bucle, volumen));
            });
            return miSounds.ToArray();
        }

        //Crea todos los sonidos
        AudioSource Creator(string s, Vector3 p, bool loop = false, float volumen = 1) {
            if (!Save.Data.mute)
            {
                if (!sonidos.Inside(s))
                {
                    //Ese sonido no esta dentro del array
                    Debug.LogError($"baboOn: 5.2.-No existe el sonido {s} dentro de Sounds");
                    return null;
                }

                GameObject soundInstance = new GameObject($"Sound-{s}");
                soundInstance.transform.SetParent(transform);

                AudioSource audioSource = soundInstance.AddComponent<AudioSource>();
                audioSource.clip = sonidos.Get(s);
                audioSource.volume = volumen;

                if (loop) audioSource.loop = true;
                soundInstance.transform.position = p;

                audioSource.Play();

                if (!loop) {
                    Destroy(soundInstance, 10f);
                }
                return audioSource;
            }
            return null;
        }

        public void Vibracion(float duracion = 0)
        {
            if (vibration) {
                //Ya esta vibrando
                Debug.LogWarning($"baboOn: 5.4.-El dispositivo esta ejecutando otra vibracion");
            }

            if (SystemInfo.supportsVibration)
            {
                vibration = true;

                StartCoroutine(StartVibration());
                StartCoroutine(StopVibration(duracion));

                return;
            }

            //El dispositivo no puede vibrar
            Debug.LogWarning($"baboOn: 5.3.-El dispositivo no es compatible con la vibracion");
        }
        IEnumerator StartVibration()
        {
            while (vibration)
            {
                    #if UNITY_ANDROID
                Handheld.Vibrate();
                    #endif
                yield return null;
            }
        }
        IEnumerator StopVibration(float duration)
        {
            duration.Log();
            yield return new WaitForSeconds(duration);
            vibration = false;
        }

    }

}