using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //Crea sonidos de una sola vez. Puedes elegir que sonidos y en que posicion
        public AudioSource Audio(string sonido, Vector3 posicion = default(Vector3), bool bucle = false)
        {
            return Creator(sonido, posicion, bucle);
        }
        public AudioSource[] Audio(string[] sonido, Vector3 posicion = default(Vector3), bool bucle = false)
        {
            List<AudioSource> miSounds = new List<AudioSource>();
            sonido.ForEach((i) => {
                miSounds.Add(Creator(i, posicion, bucle));
            });
            return miSounds.ToArray();
        }

        //Crea todos los sonidos
        AudioSource Creator(string s, Vector3 p, bool loop = false) {
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

                if (loop) audioSource.loop = true;
                soundInstance.transform.position = p;

                audioSource.Play();

                Destroy(soundInstance, 5f);
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