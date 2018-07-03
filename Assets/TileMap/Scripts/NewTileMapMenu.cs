using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class NewTileMapMenu {
    [MenuItem("GameObject/Tile Map")]
    public static void CreateTimeMap()
    {
        var go = new GameObject("Tile Map");
        go.AddComponent<TileMap>();
    }

}
