using System.Collections.Generic;
using UnityEngine;
namespace BaboOnLite
{
    [System.Serializable]
    public partial class SaveScript
    {
        //----------------------------------------------------------------//
        //Variables por defecto: Estas varibles se usan automaticamente   //
        //----------------------------------------------------------------//
        public int language = 0;
        public bool mute = false;
        public Dictionary<string, int> skins = new Dictionary<string, int>();

        public int mundo = 0; //En quemundoe estas
        public int record = 0; //La puntuacion maxima que ha conseguido

    }
}
