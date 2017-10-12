using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//<author>Georgina Perera</author>
public class MouseCursor : MonoBehaviour
{
    public Texture2D defaultMouse;
    public Texture2D hoverCursor;
    public CursorMode cursorM = CursorMode.Auto;
    public Vector2 hotspot = Vector2.zero;

    void start()
    {
        //Cursor.SetCursor(defaultMouse, hotspot, cursorM);
        Cursor.visible = false;
    }

    void OnMouseEnter()
    {
        if (gameObject.tag == "onHover")
        {
            Cursor.SetCursor (hoverCursor, hotspot, cursorM);
        }
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(defaultMouse, hotspot, cursorM);
    }

}