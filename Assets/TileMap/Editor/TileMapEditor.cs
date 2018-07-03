using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor {
    public TileMap map;
    private TileBrush brush;
    private Vector3 mouseHitPos;
    public override void OnInspectorGUI()
    {

        //EditorGUILayout.LabelField("Our custom editor");
        // base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        var oldSize = map.mapsize;
        map.mapsize = EditorGUILayout.Vector2Field("Map Size: ", map.mapsize);
 
        if (oldSize != map.mapsize)
        {
            UpodateCalculations();
        }
        map.texture2D = (Texture2D)EditorGUILayout.ObjectField("Texture2D: ", map.texture2D, typeof(Texture2D), false);

        if (map.texture2D == null)
        {
            EditorGUILayout.HelpBox("You have not selected a texture 2d yet", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.LabelField("Tile size", map.tileSize.x + "*" + map.tileSize.y);
            map.tilePadding = EditorGUILayout.Vector2Field("Tile Padding", map.tilePadding);
            EditorGUILayout.LabelField("Grid size", map.gridSize.x + "*" + map.gridSize.y);
            EditorGUILayout.LabelField("Fixels to Unit", map.pixelsToUnits.ToString());
            UpdateBrush(map.CurrentTileBrush);
            if (GUILayout.Button("Clear Tiles"))
            {
                if (EditorUtility.DisplayDialog("Clear map's tiles?", "Are you sure?", "Clear", "Do not clear"))
                {
                    ClearMap();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }
    public void OnEnable()
    {
        map = target as TileMap;
        Tools.current = Tool.View;
        if(map.tiles ==null)
        {
            var go = new GameObject("Tiles");
            go.transform.SetParent(map.transform);
            go.transform.position = Vector3.zero;
            map.tiles = go;
        }
        if(map.texture2D != null)
        {
            UpodateCalculations();
            NewBrush();
        }
    }
    private void OnDisable()
    {
        DestroyBrush();
    }
    private void OnSceneGUI()
    {
        if(brush!=null)
        {
            UpdateHitPostion();
            MoveBrush();
            if(map.texture2D!=null&&IsMouseOnMap)
            {
                var current = Event.current;
                if(current.shift)
                {
                    Draw();
                }
                else if(current.alt)
                {
                    Removetile();
                }
            }
        }
    }
    private void UpodateCalculations()
    {
        map = target as TileMap;
        Tools.current = Tool.View;
        if (map.texture2D != null)
        {
            var path = AssetDatabase.GetAssetOrScenePath(map.texture2D);

            map.spriteReferences = AssetDatabase.LoadAllAssetsAtPath(path); ;
            var sprite = (Sprite)map.spriteReferences[1];
            var width = sprite.textureRect.width;
            var height = sprite.textureRect.height;
            map.tileSize = new Vector2(width, height);
            map.pixelsToUnits = (int)(sprite.rect.width / sprite.bounds.size.x);
            map.gridSize = new Vector2((width/map.pixelsToUnits) * map.mapsize.x, (height/ map.pixelsToUnits) * map.mapsize.y);
            //AssetDatabase.LoadAllAssetsAtPath(path);
        }
    }
    private void CreateBrush()
    {
        var sprite = map.CurrentTileBrush;
        if(sprite != null)
        {
            var newGO = new GameObject("Brush");
            newGO.transform.SetParent(map.transform);
            brush = newGO.AddComponent<TileBrush>();
            brush.rendered2D = newGO.AddComponent<SpriteRenderer>();
            var pixelsToUnits = map.pixelsToUnits;
            brush.brushSize = new Vector2(sprite.textureRect.width/pixelsToUnits,sprite.textureRect.height/pixelsToUnits);
            brush.UpdateBrush(sprite);
        }
    }
    private void NewBrush()
    {
        if (brush == null)
        {
            CreateBrush();
        }
    }
    private void DestroyBrush()
    {
        if(brush!=null)
        {
            DestroyImmediate(brush.gameObject);
        }
    }
    public void UpdateBrush(Sprite sprite)
    {
        if(brush!=null)
        {
            brush.UpdateBrush(sprite);
        }
    }
    private void UpdateHitPostion()
    {
        var plane = new Plane(map.transform.TransformDirection(Vector3.forward),Vector3.zero);
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        var hit = Vector3.zero;
        var dist = 0f;
        if (plane.Raycast(ray, out dist))
            hit = ray.origin + ray.direction.normalized * dist;
        mouseHitPos = map.transform.InverseTransformPoint(hit);
    }
    private void MoveBrush()
    {
        if(!IsMouseOnMap)
        {
            return;
        }
        var tileSize = map.tileSize.x / map.pixelsToUnits;
        var setPosX = Mathf.Floor(mouseHitPos.x/tileSize) * tileSize;
        var setPosY = Mathf.Floor(mouseHitPos.y/ tileSize) * tileSize;
        setPosX += map.transform.position.x + tileSize / 2;
        setPosY += map.transform.position.y + tileSize / 2;
        brush.transform.position = new Vector3(setPosX , setPosY , map.transform.position.z);
        var column = setPosX / tileSize;
        var row = Mathf.Abs(setPosY / tileSize) - 1;
        brush.tileID = Mathf.FloorToInt(row * map.mapsize.x + column);
    }
    private bool IsMouseOnMap
    {
        get { return mouseHitPos.x > 0f && mouseHitPos.x < map.gridSize.x && mouseHitPos.y < 0f && mouseHitPos.y > -map.gridSize.y; }
    }
    private void Draw()
    {
        var id = brush.tileID.ToString();
        var pos = brush.transform.position;
        var tile = GameObject.Find(map.name + "/Tiles/tile_" + id);
        if(tile == null)
        {
            tile = new GameObject("tile_" + id);
            tile.transform.SetParent(map.tiles.transform);
            tile.transform.position = pos;
            tile.AddComponent<SpriteRenderer>();
        }
        tile.GetComponent<SpriteRenderer>().sprite = brush.rendered2D.sprite;
    }
    private void Removetile()
    {
        var id = brush.tileID.ToString();       
        var tile = GameObject.Find(map.name + "/Tiles/tile_" + id);
        if (tile == null)
        {
            return;
        }
        //tile.GetComponent<SpriteRenderer>().sprite =null;
        DestroyImmediate(tile);
    }
    private void ClearMap()
    {
        for (var i= map.tiles.transform.childCount-1; i>=0;i--)
        {
            var t = map.tiles.transform.GetChild(i);
            DestroyImmediate(t.gameObject);
        }
    }
}