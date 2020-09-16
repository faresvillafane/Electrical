using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Connector : MonoBehaviour

{

    private Vector3 v3StartingPos;
    private Vector3 mOffset;

    private float mZCoord;

    private CableComponent cableComponent;

    private bool bBeingUsed = false;

    private GameController gameController;

    private Connector cConnectedTo;

    public GameObject goCableStart;

   // private Plane p;
    private void Awake()
    {
        cableComponent = goCableStart.GetComponent<CableComponent>();
        v3StartingPos = this.transform.localPosition;
        gameController = GameObject.FindGameObjectWithTag(EConstants.TAG_GAMECONTROLLER).GetComponent<GameController>();
    }

    public void HandleMouseDown()
    {
        gameController.SetSnapCollider(true);
       
        //p = new Plane(transform.up, transform.position);
            
        cableComponent.Init();
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos

        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    void OnMouseDown()
    {
        HandleMouseDown();
    }



    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = mZCoord;
        /*
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        float d;
        if (p.Raycast(r, out d))
        {
            Vector3 v = r.GetPoint(d);
            mousePoint.z = v.z;
        }
        
        */

        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }



    void OnMouseDrag()
    {

        transform.position = GetMouseAsWorldPoint() + mOffset;
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                if (ShouldConnect(hit.collider.gameObject))
                {
                    this.transform.position = hit.collider.transform.position;
                }
            }
        }
    }

    private bool ShouldConnect(GameObject goCollider)
    {
        return goCollider.CompareTag(EConstants.TAG_CABLE_END) && goCollider != this.gameObject && goCollider.GetComponent<Connector>().IsFree() && this.IsFree();
    }

    private void OnMouseUp()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider != null)
            {
                if (ShouldConnect(hit.collider.gameObject))
                {
                    this.transform.position = hit.collider.transform.position;
                    SetConnections(hit.collider.GetComponent<Connector>());
                }
                else
                {
                    ResetCable();
                }
            }
            else
            {
                ResetCable();
            }
        }
        else
        {
            ResetCable();
        }
        gameController.SetSnapCollider(false);
    }

    private void SetConnections(Connector connector)
    {
        connector.SetConnection(this);
        this.SetConnection(connector);
    }

    public void ResetCable()
    {
        this.transform.localPosition = v3StartingPos;
        cableComponent.Delete();
        goCableStart.GetComponent<Collider>().enabled = false;
        this.GetComponent<Collider>().enabled = true;
    }

    public void SetConnection(Connector newConnector)
    {
        bBeingUsed = true;
        cConnectedTo = newConnector;
        goCableStart.GetComponent<Collider>().enabled = true;
        this.GetComponent<Collider>().enabled = false;
    }
    public void BreakConnection()
    {
        BreakConnectionTo();
        ResetCable();
        bBeingUsed = false;
        cConnectedTo = null;
    }
    public void BreakConnectionTo()
    {
        cConnectedTo.cConnectedTo = null;
        cConnectedTo.bBeingUsed = false;
        cConnectedTo.goCableStart.GetComponent<Collider>().enabled = false;
        cConnectedTo.GetComponent<Collider>().enabled = true;
    }
    public bool IsFree()
    {
        return !bBeingUsed;
    }

}