using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Errores {
    public static Dictionary<string, string[]> get = new Dictionary<string, string[]> {
        { "Global", new string[]{
            "[BG] Se ha intentado crear mas de una instancia del mismo scrip"
            }
        },
    };
}
