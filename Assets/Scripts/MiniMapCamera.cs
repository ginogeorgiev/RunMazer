using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * use SetPosition if you want to move the mini map with the player
 */
public class MiniMapCamera : MonoBehaviour
{
    
    [SerializeField] private Transform player;
    private GUIStyle style;
    private Texture2D rectTexture;
    private void OnGUI()
    {
        //GUIDrawRect(new Rect(player.position.x,player.position.y,1000,1000),new Color(255,0,0));
        
    }
    void Start()
    {
        
        //SetPosition();
    }

    
    /*private void LateUpdate()
    {
        if (player != null)
        {
            SetPosition();
            
        }
    }*/

    /*void SetPosition()
    {
        var newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }*/
    private void GUIDrawRect(Rect position, Color color)
    {
        if( rectTexture == null )
        {
            rectTexture = new Texture2D( 1, 1 );
        }
 
        if(style == null)
        {
            style = new GUIStyle();
        }
 
        rectTexture.SetPixel( 0, 0, color);
        rectTexture.Apply();
 
        style.normal.background = rectTexture;
        
        GUI.Box( position, GUIContent.none, style);

    }
}
