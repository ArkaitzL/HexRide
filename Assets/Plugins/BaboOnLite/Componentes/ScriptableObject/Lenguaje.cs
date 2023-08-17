using System.Linq;
using UnityEngine;

namespace BaboOnLite
{
    [CreateAssetMenu(fileName = "Lenguaje", menuName = "BaboOnLite/Lenguaje", order = 1)]
    public class Lenguaje : ScriptableObject
    {
        //Almacena las palabras del idioma
        public string[] dictionary = new string[0];
        //Copia el diccionario
        public void Copy()
        {
            GUIUtility.systemCopyBuffer = dictionary.inString();
            Debug.Log("Idioma copiado en el portapaeles");
        }
        //Crea un nuevo diccionario como el que le pasas
        public void Paste()
        {
            dictionary = dictionary.Concat(
                GUIUtility.systemCopyBuffer.inArray<string>()
             ).ToArray();
        }

        //Añadir datos al diccionario con el que le pasas
        public void PasteAsNew()
        {
            dictionary = GUIUtility.systemCopyBuffer.inArray<string>();
        }
    }
}
