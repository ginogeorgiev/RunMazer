using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapUIScript : MonoBehaviour
{
    private RectTransform trans;
    private bool isMaximized;
    void Start()
    {
        trans = (RectTransform)gameObject.transform;
        isMaximized = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        //maximize minimap when pressing m
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isMaximized)
            {
                float scaleVal = Screen.height / trans.rect.height;
                //to the center of our screen
                trans.localPosition = new Vector3(0.5f, 0.5f);
                //scale it up to the height of our screen
                trans.localScale = new Vector3(scaleVal, scaleVal);
                
                isMaximized = true;
            }
            else
            {
                trans.localScale = new Vector3(1.0f,1.0f);
                //default position of the minimap relative to the Canvas
                trans.anchoredPosition = new Vector2(-172f,172f);
                isMaximized = false;
            }

        }
    }
}
