using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {
    public Vector2 mapsize = new Vector2(20, 10);
    public Texture2D texture2D;
    public Vector2 tileSize = Vector2.zero;
    public Vector2 tilePadding = Vector2.zero;
    public Object[] spriteReferences;
    public Vector2 gridSize = Vector2.zero;
    public int pixelsToUnits = 1;
    public int tileID = 0;
    public GameObject tiles;
    public Sprite CurrentTileBrush
    {
        get { return spriteReferences[tileID] as Sprite; }
    }

    private void OnDrawGizmosSelected()
    {
        //pixelsToUnits = 1;
        var pos = transform.position;
        if(texture2D !=null)
        {
            Gizmos.color = Color.gray;

            var row = 0;
            var maxColumns =(int) mapsize.x;
            var total = (int)(mapsize.x * mapsize.y);
            var tile = new Vector3(tileSize.x / pixelsToUnits, tileSize.y / pixelsToUnits);
            var offset = new Vector2(tileSize.x / 2f, tileSize.y / 2f);
            for(var i =0;i<total;++i)
            {
                var column = i %maxColumns;
                var newX = (column * tile.x) + offset.x + pos.x;
                var newY = -((row * tile.y) + offset.y + pos.y);
                Gizmos.DrawWireCube(new Vector2(newX, newY), tile);
                if (column == maxColumns - 1)
                {
                    ++row;
                }
            }
            


            Gizmos.color = Color.white;
            var centerX = pos.x + (gridSize.x / 2f);
            var centerY = pos.y - (gridSize.y / 2f);
            Gizmos.DrawWireCube(new Vector2(centerX, centerY), gridSize);
        }
    }
}
