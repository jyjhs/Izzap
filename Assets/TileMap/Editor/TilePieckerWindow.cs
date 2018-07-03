﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TilePieckerWindow : EditorWindow { 
     public enum Scale
    {
         x1,
         x2,
         x3,
         x4,
         x5
    }
    private Scale scale;
    private Vector2 currentSelection = Vector2.zero;
    public Vector2 scrollPosition = Vector2.zero;
    [MenuItem("Window/Tile Picker")]
    public static void OpenTilePickerWindow()
    {
        var window = EditorWindow.GetWindow(typeof(TilePieckerWindow));
        var title = new GUIContent();
        title.text = "Tile Piecker";
        window.titleContent = title;
    }
    private void OnGUI()
    {
        if (Selection.activeGameObject == null)
            return;

        var selection = Selection.activeGameObject.GetComponent<TileMap>();
        if(selection!=null)
        {
            var texture2D = selection.texture2D;
            if(texture2D != null)
            {
               scale = (Scale)EditorGUILayout.EnumPopup("Zoom", scale);
                var newScale = (int)scale + 1;
                var newTextureSize = new Vector2(texture2D.width, texture2D.height) * newScale;
                var offset = new Vector2(10f, 20f);

                var viewPort = new Rect(0, 0, position.width - 5f,position.height - 5f);
                var contentSize = new Rect(0f, 0f, newTextureSize.x, newTextureSize.y);
                scrollPosition = GUI.BeginScrollView(viewPort, scrollPosition, contentSize);
                GUI.DrawTexture(new Rect(offset.x, offset.y, newTextureSize.x, newTextureSize.y),texture2D);
                var tile = selection.tileSize * newScale;
                tile.x += selection.tilePadding.x * newScale;
                tile.y += selection.tilePadding.y * newScale;
                var grid = new Vector2(newTextureSize.x/tile.x,newTextureSize.y/tile.y);
                var selectionPos = new Vector2(tile.x * currentSelection.x+offset.x, tile.y * currentSelection.y + offset.y);
                var boxTex = new Texture2D(1, 1);
                boxTex.SetPixel(0,0,new Color(0f,0.5f,1f,0.4f));
                boxTex.Apply();
                var style = new GUIStyle(GUI.skin.customStyles[0]);
                style.normal.background = boxTex;
                GUI.Box(new Rect(selectionPos.x, selectionPos.y, tile.x, tile.y), "", style);
                var cEvent = Event.current;
                var mousPos = new Vector2(cEvent.mousePosition.x, cEvent.mousePosition.y);
                if(cEvent.type == EventType.MouseDown&&cEvent.button == 0)
                {

                    mousPos.x = Mathf.Clamp(mousPos.x, offset.x, newTextureSize.x);
                    mousPos.y = Mathf.Clamp(mousPos.y, offset.y, newTextureSize.y);
                    currentSelection.x = Mathf.Floor((mousPos.x  - offset.x) / tile.x);
                    currentSelection.y = Mathf.Floor((mousPos.y  - offset.y) / tile.y);
                    selection.tileID = (int)currentSelection.x + (int)(currentSelection.y * grid.x) + 1;
                    Repaint();
                }

                GUI.EndScrollView();
            }
        }
    }
}
