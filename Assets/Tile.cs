using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Tile : MonoBehaviour
{
    public Camera camera;
    Vector3 origin;
    Vector3 screenPoint;
    Vector3 offset;

    bool inOnMouseDown = false;

	GameObject currentHit;

    // Use this for initialization
    void Start()
    {
        camera = GetComponent<Camera>();

    }

    void _OnMouseUp(bool touch)
    {
        GameObject.Find("Text").GetComponent<Text>().text = "_OnMouseUp()";
        Debug.Log("_OnMouseUp");
        inOnMouseDown = false;
        currentHit = null;
    }

    void _OnMouseDrag(bool touch)
    {
        GameObject.Find("Text").GetComponent<Text>().text = "_OnMouseDrag() " + currentHit;
        Debug.Log ("_OnMouseDrag");

        // if we get a position from the touch screen where it is
        if (touch)
        {
            Vector3 curScreenPoint = camera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, screenPoint.z));
            if (currentHit != null)
            {
                currentHit.transform.position = curScreenPoint;
            }
        }
        // if we need to find the position of the mouse on the screen
        else {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            
            Vector3 curPosition = camera.ScreenToWorldPoint(curScreenPoint) + offset;

            if (currentHit != null)
            {
                currentHit.gameObject.transform.position = curPosition;
            }
        }
    }

    void _OnMouseDown(bool touch)
    {
        GameObject.Find("Text").GetComponent<Text>().text = "_OnMouseDown() " + currentHit;
        Debug.Log("_OnMouseDown " + name); ;
        inOnMouseDown = true;

        // if this is from a touch call
        if (touch) {
            origin = currentHit.transform.position;
            screenPoint = camera.WorldToScreenPoint(currentHit.gameObject.transform.position);
            offset = currentHit.gameObject.transform.position - camera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, screenPoint.z));
            GameObject.Find("Text").GetComponent<Text>().text = currentHit.gameObject.name;
        }
        // if this is from a mouse call, find the current position
        else { 
            origin = currentHit.transform.position;
            screenPoint = camera.WorldToScreenPoint(currentHit.gameObject.transform.position);
            offset = currentHit.gameObject.transform.position - camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        // if this is from a touch call
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended)
            {
                // up
                _OnMouseUp(true);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                // drag
                _OnMouseDrag(true);
            }
            if (touch.phase == TouchPhase.Began)
            {
                // down
                RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
                if (hit.collider != null)
                {
                    // can't use current hit as slots
                    if (hit.collider.gameObject != null)
                    {
                        currentHit = hit.collider.gameObject;
                        _OnMouseDown(true);
                    }
                }
            }
        }

        // if this is from a mouse call
        if (Input.GetMouseButtonDown(0))
        {
            // the screen is currently being touched for the first time
            RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                // can't use current hit as slots
                if (hit.collider.gameObject != null)
                {
                    currentHit = hit.collider.gameObject;
                    _OnMouseDown(false);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // the screen is currently stopped being touched
            _OnMouseUp(false);
        }
        else if (Input.GetMouseButton(0))
        {
            // the screen is currently being held
            _OnMouseDrag(false);
        }
    }
}

