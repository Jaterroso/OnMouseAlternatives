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

    }

    void _OnMouseUp()
    {
        GameObject.Find("Text").GetComponent<Text>().text = "_OnMouseUp()";
        Debug.Log("_OnMouseUp");
        inOnMouseDown = false;
    }

    void _OnMouseDrag(bool touch)
    {
        GameObject.Find("Text").GetComponent<Text>().text = "_OnMouseDrag()";
        Debug.Log ("_OnMouseDrag");

        // if we get a position from the touch screen where it is
        if (touch)
        {
            if (camera == null)
                camera = GameObject.Find("Main Camera").GetComponent<Camera>();

            Vector3 curScreenPoint = camera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, screenPoint.z));
            transform.position = curScreenPoint;
        }
        // if we need to find the position of the mouse on the screen
        else {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            if (camera == null)
                camera = GameObject.Find("Main Camera").GetComponent<Camera>();

            Vector3 curPosition = camera.ScreenToWorldPoint(curScreenPoint) + offset;

            if (currentHit != null)
            {
                currentHit.gameObject.transform.position = curPosition;
            }
        }
    }

    void _OnMouseDown()
    {
        GameObject.Find("Text").GetComponent<Text>().text = "_OnMouseDown()";
        Debug.Log("_OnMouseDown " + name); ;
        inOnMouseDown = true;
        if (camera == null)
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        // if this is from a touch call
        if (currentHit == null) {
            origin = transform.position;
            screenPoint = camera.WorldToScreenPoint(transform.position);
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
                _OnMouseUp();
            }
            if (touch.phase == TouchPhase.Moved)
            {
                // drag
                _OnMouseDrag(true);
            }
            if (touch.phase == TouchPhase.Began)
            {
                // down
                _OnMouseDown();
            }
        }

        // if this is from a mouse call
        if (Input.GetMouseButtonDown(0))
        {
            // the screen is currently being touched for the first time
            if (camera == null)
                camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                // can't use current hit as slots
                if (hit.collider.gameObject != null)
                {
                    currentHit = hit.collider.gameObject;
                    _OnMouseDown();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // the screen is currently stopped being touched
            _OnMouseUp();
            currentHit = null;
        }
        else if (Input.GetMouseButton(0))
        {
            // the screen is currently being held
            _OnMouseDrag(false);
        }
    }
}

