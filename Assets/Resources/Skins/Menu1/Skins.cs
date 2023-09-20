using System;
using BaboOnLite;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public partial class Skins{
    [SerializeField] private Prefabs prefabs;
    [SerializeField] private Secciones[] secciones;
    [SerializeField] private Colores colores;

}
public partial class Skins : MonoBehaviour {
    private void Start()  {
        //GENERAR
        bool estado = true;

        secciones.ForEach((section) => {
            Transform contenedor = GameObject.FindGameObjectWithTag("Contenedor").transform;
            Transform nav = GameObject.FindGameObjectWithTag("Nav").transform;
            Transform salir = GameObject.FindGameObjectWithTag("Salir").transform;

            //COLORES
            contenedor.GetComponent<Image>().color = colores.section;
            salir.GetComponent<Image>().color = colores.nav.fondo;
            nav.GetComponent<Image>().color = colores.nav.fondo;

            //SECTION
            GameObject sectionObjeto = Instantiate(prefabs.section, contenedor);
            sectionObjeto.SetActive(estado);

            if (estado) { //Añade el 1 section al scroll
                contenedor.GetComponent<ScrollRect>().content = sectionObjeto.GetComponent<RectTransform>();
            }

            estado = false;

            //NAV
            Transform opcion = Instantiate(prefabs.opcion, nav).transform; 
            opcion.GetChild(0).GetComponent<TextMeshProUGUI>().text = section.nombre;
            opcion.GetComponent<Button>().onClick.AddListener(() => {
                //Desactiva los demas sections
                for (int i = 0; i < contenedor.childCount; i++) {
                    contenedor.GetChild(i).gameObject.SetActive(false);  ///********BUG**************
                }
                //Activa el que esta usasndo
                sectionObjeto.SetActive(true);
                //Cambia el scroll
                contenedor.GetComponent<ScrollRect>().content = sectionObjeto.GetComponent<RectTransform>();
            });

            for (int i = 0; i < nav.childCount; i++)
            {
                nav.GetChild(i).GetComponent<Image>().color = colores.nav.opciones;
            }


            //ARTICLES
            section.article.ForEach((article) => {
                Transform articleObjeto = Instantiate(prefabs.article, sectionObjeto.transform).transform;
                articleObjeto.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = article.info;

                Save.Data.skins.TryAdd(article.unidad, 0); // CREAR CONTADOR

                //FILAS DE SKINS
                int y = 0;
                article.skinFila.ForEach((skinFila) => {
                    Transform skinsObjeto = Instantiate(prefabs.skin, articleObjeto.transform).transform;
                    RectTransform rectTransform = articleObjeto.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + y);

                    y = 350;

                    //SKINS
                    SkinsInfo[] misSkins = {
                        skinFila.skins1,
                        skinFila.skins2,
                        skinFila.skins3
                    };
                    int num = 0;

                    misSkins.ForEach((skin) =>
                    {
                        if (skin.imagen != null)
                        {
                            Transform skinObjeto = skinsObjeto.GetChild(num);
                            skinObjeto.gameObject.SetActive(true);

                            //IMAGEN
                            Transform imagen = skinObjeto.GetChild(0);
                            imagen.GetChild(0).GetComponent<Image>().sprite = skin.imagen;
                            imagen.GetChild(1).GetComponent<Image>().color = colores.bloqueo;

                            //Poner la skin
                            imagen.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
                            {
                                Instanciar<SkinAdmin>.Coger("Skins").CambiarSkin(skin.skin, skin.materiales);
                            });

                            //PROGRESO
                            Transform progreso = skinObjeto.GetChild(1);
                            progreso.GetChild(0).GetComponent<Image>().color = colores.progreso.fondo;
                            progreso.GetChild(1).GetComponent<Image>().color = colores.progreso.carga;
                            progreso.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{Save.Data.skins[article.unidad]}/{skin.cantUnidad} {article.unidad}";

                            //Barra de progreso y Desbloquear skin
                            if (Save.Data.skins[article.unidad] < skin.cantUnidad)
                            {
                                progreso.GetChild(1).GetComponent<Image>().fillAmount = Save.Data.skins[article.unidad] / skin.cantUnidad;
                                ///AVISO DESBLOQUEO***
                            }
                            else 
                            {
                                progreso.GetChild(0).gameObject.SetActive(false);
                            }

                        }
                        num++;
                    });

                });

                //Cambia el tamaño del section
                RectTransform sectionRect = sectionObjeto.GetComponent<RectTransform>();
                RectTransform articleRect = articleObjeto.GetComponent<RectTransform>();

                sectionRect.sizeDelta = new Vector2(sectionRect.sizeDelta.x, sectionRect.sizeDelta.y + articleRect.sizeDelta.y);
            });

            //Poner un tamaño minimo al section
            RectTransform sectionRect = sectionObjeto.GetComponent<RectTransform>();
            RectTransform contenedorRect = contenedor.GetComponent<RectTransform>();

            if (sectionRect.sizeDelta.y < contenedorRect.sizeDelta.y)
            {
                sectionRect.sizeDelta = new Vector2(sectionRect.sizeDelta.x, contenedorRect.sizeDelta.y);
            }

            //Cambia la posicion del section
            sectionRect.anchoredPosition = new Vector2(sectionRect.anchoredPosition.x, sectionRect.sizeDelta.y / -2);

        });


    }
}
public partial class Skins { 

}


//CLASES
[Serializable]
public class Prefabs {
    public GameObject opcion;
    public GameObject section;
    public GameObject article;
    public GameObject skin;

}

[Serializable]
public class Colores {
    public Nav nav;
    public Color section;
    public Color bloqueo;
    public Progreso progreso;

    [Serializable]
    public class Progreso {
        public Color fondo;
        public Color carga;
    }
    [Serializable]
    public class Nav
    {
        public Color fondo;
        public Color opciones;
    }
}

[Serializable]
public class Secciones {
    //Secciones con diferentes skins
    public string nombre;
    public Article[] article;

    [Serializable]
    public class Article
    {
        //Grupo de todas las skins
        public string info;
        public string unidad;
        public SkinsFila[] skinFila;

        [Serializable]
        //Una fila de skins
        public class SkinsFila
        {
            public SkinsInfo skins1;
            public SkinsInfo skins2;
            public SkinsInfo skins3;
        }
    }
}

[Serializable]
//Info de la skins
public class SkinsInfo
{
    public Sprite imagen;
    public int cantUnidad;
    public Mesh skin;
    public Material[] materiales;
}