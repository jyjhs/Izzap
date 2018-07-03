using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBrush : MonoBehaviour {
    public Vector2 brushSize = Vector2.zero;
    public int tileID = 0;
    public SpriteRenderer rendered2D;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, brushSize);
    }

    public void UpdateBrush(Sprite sprite)
    {
        rendered2D.sprite = sprite;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
