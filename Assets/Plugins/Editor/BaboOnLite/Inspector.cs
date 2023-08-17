using UnityEditor;
using UnityEngine;

namespace BaboOnLite
{
    //Editor de save
    [CustomEditor(typeof(Save))]
    public class SaveEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Save ins = (Save)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Remove Data"))
            {
                ins.Remove();
            }
        }
    }
    //Editor de language
    [CustomEditor(typeof(Lenguaje))]
    public class LanguageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Lenguaje lang = (Lenguaje)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Copy"))
            {
                lang.Copy();
            }

            if (GUILayout.Button("Paste"))
            {
                lang.Paste();
            }

            if (GUILayout.Button("Paste as new"))
            {
                lang.PasteAsNew();
            }
        }
    }
    //Diccionario
    //[CustomPropertyDrawer(typeof(DictionarySerializable<>.Elementos))]
    //public class DictionaryElementDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        EditorGUI.BeginProperty(position, label, property);

    //        SerializedProperty indiceProperty = property.FindPropertyRelative("indice");
    //        SerializedProperty valorProperty = property.FindPropertyRelative("valor");

    //        Rect indiceRect = new Rect(position.x, position.y, position.width * 0.4f, position.height);
    //        Rect valorRect = new Rect(position.x + position.width * 0.45f, position.y, position.width * 0.4f, position.height);

    //        EditorGUI.PropertyField(indiceRect, indiceProperty, GUIContent.none);
    //        EditorGUI.PropertyField(valorRect, valorProperty, GUIContent.none);


    //        EditorGUI.EndProperty();

    //    }
    //}
}