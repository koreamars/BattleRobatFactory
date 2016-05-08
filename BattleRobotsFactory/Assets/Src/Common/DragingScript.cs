using UnityEngine;
using System.Collections;

public class DragingScript : MonoBehaviour
{
    //This code is for 2D click/drag gameobject
    //Please make sure to change Camera Projection to Orthographic
    //Add Collider (not 2DCollider) to gameObject  

    public GameObject gameObjectTodrag; //refer to GO that being dragged

    public Vector3 GOcenter; //gameobjectcenter
    public Vector3 touchPosition; //touch or click position
    public Vector3 offset;//vector between touchpoint/mouseclick to object center
    public Vector3 newGOCenter; //new center of gameObject

    RaycastHit hit; //store hit object information

    public bool draggingMode = false;

    public delegate void SetTouchDown(bool isDown);
    public SetTouchDown setTouchDown;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //***********************
        // *** CLICK TO DRAG ****
        //***********************

#if UNITY_EDITOR
        //first frame when user click left mouse
        if (Input.GetMouseButtonDown(0) == true)
        {
            //convert mouse click position to a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if ray hit a Collider ( not 2DCollider)
            if (Physics.Raycast(ray, out hit))
            {
                gameObjectTodrag = hit.collider.gameObject;
                GOcenter = gameObjectTodrag.transform.position;
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset = touchPosition - GOcenter;
                draggingMode = true;
                setTouchDown(draggingMode);
            }
            
        }

        //every frame when user hold on left mouse
        if (Input.GetMouseButton(0))
        {
            if (draggingMode)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameManager.getInstance.ShowLog("touchPosition : " + touchPosition); 
                newGOCenter = touchPosition - offset;
                if (newGOCenter.x < -7) newGOCenter.x = -7f;
                if (newGOCenter.y < -3) newGOCenter.y = -3f;
                if (newGOCenter.y > 4) newGOCenter.y = 4f;
                gameObjectTodrag.transform.position = new Vector3(newGOCenter.x, newGOCenter.y, GOcenter.z);
            }
        }

        //when mouse is released
        if (Input.GetMouseButtonUp(0))
        {
            draggingMode = false;
            setTouchDown(draggingMode);
        }
#endif

        //***********************
        // *** TOUCH TO DRAG ****
        //***********************
        foreach (Touch touch in Input.touches)
        {
            //GameManager.getInstance.ShowLog("touch : " + Input.mousePosition);
            if (touch.rawPosition.y < 280)
            {
                continue;
            }
            //GameManager.getInstance.ShowLog("touch : " + touch.rawPosition.x + "/" + touch.rawPosition.y);
            GameManager.getInstance.ShowLog("mouse : " + Input.mousePosition);
            GameManager.getInstance.ShowLog("touch : " + touch.position);
            switch (touch.phase)
            {
                //When just touch
                case TouchPhase.Began:
                    //convert mouse click position to a ray
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    //if ray hit a Collider ( not 2DCollider)
                    // if (Physics.Raycast(ray, out hit))
                    if (Physics.SphereCast(ray, 0.3f, out hit))
                    {
                        gameObjectTodrag = hit.collider.gameObject;
                        GOcenter = gameObjectTodrag.transform.position;
                        //touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        offset = touchPosition - GOcenter;
                        draggingMode = true;
                        setTouchDown(draggingMode);
                    }
                    break;

                case TouchPhase.Moved:
                    if (draggingMode)
                    {
                        //touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        newGOCenter = touchPosition - offset;
                        if (newGOCenter.x < -7) newGOCenter.x = -7f;
                        if (newGOCenter.y < -3) newGOCenter.y = -3f;
                        if (newGOCenter.y > 4) newGOCenter.y = 4f;
                        gameObjectTodrag.transform.position = new Vector3(newGOCenter.x, newGOCenter.y, GOcenter.z);
                    }
                    break;

                case TouchPhase.Ended:
                    draggingMode = false;
                    setTouchDown(draggingMode);
                    break;
            }
        }
    }
}