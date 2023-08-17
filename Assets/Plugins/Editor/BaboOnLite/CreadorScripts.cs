using UnityEditor;
using UnityEngine;
using System.IO;

public class CreadorScripts : MonoBehaviour
{
    [MenuItem("Assets/Create/BaboOnLite/SaveScript")]
    private static void Creador()
    {
        // Consigue las rutas
        string rutaPlantilla = AssetDatabase.GUIDToAssetPath(
            AssetDatabase.FindAssets("BaboOnLite")[0]
        ) + "/Scripts/SaveScript.txt";
        
        string rutaSeleccionada = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (!string.IsNullOrEmpty(rutaSeleccionada))
        {
            // Recoge el contenido de la plantilla
            string contenido = File.ReadAllText(rutaPlantilla);

            // Crea el script sin nombre
            string rutaNueva = Path.Combine(rutaSeleccionada, "NewScript.cs");
            File.WriteAllText(rutaNueva, contenido);

            // Recarga
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath(rutaNueva, typeof(Object));
        }
    }
}
