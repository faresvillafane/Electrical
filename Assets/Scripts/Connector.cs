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

    public Connector cConnectedTo;

    public GameObject goCableStart;

    private float START_RADIUS = .3f;
    private float SNAP_RADIUS = 1f;

    public bool bIsLit = false;

    public EEnums.ConnectorType ctType = EEnums.ConnectorType.INPUT;

    // private Plane p;
    private void Awake()
    {
        cableComponent = goCableStart.GetComponent<CableComponent>();
        v3StartingPos = this.transform.localPosition;
        gameController = GameObject.FindGameObjectWithTag(EConstants.TAG_GAMECONTROLLER).GetComponent<GameController>();
    }
    public float GetRadius(bool bSnapping)
    {
        return (bSnapping) ? SNAP_RADIUS : START_RADIUS;
    }
    void OnMouseDown()
    {
        HandleMouseDown();
    }

    public void HandleMouseDown()
    {
        if (IsFree())
        {
            gameController.SetSnapCollider(true);
       
            //p = new Plane(transform.up, transform.position);
            
            cableComponent.Init();
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            this.GetComponent<Collider>().enabled = false;
            // Store offset = gameobject world pos - mouse world pos

            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
        else
        {
            cConnectedTo.BreakConnection();
        }
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
        if (IsFree())
        {
            //transform.position = GetMouseAsWorldPoint() + mOffset;
            bool bSettedPos = false;
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //print(hit.collider.name);
                if (hit.collider != null)
                {
                    if (ShouldConnect(hit.collider.gameObject))
                    {
                        this.transform.position = hit.collider.transform.position;
                        bSettedPos = true;
                    }
                }
            }
            if (!bSettedPos)
            {
                transform.position = GetMouseAsWorldPoint() + mOffset;
            }
        }
    }

    private bool ShouldConnect(GameObject goCollider)
    {
        return goCollider.CompareTag(EConstants.TAG_CABLE_END) && goCollider != this.gameObject && goCollider.GetComponent<Connector>().IsFree() && this.IsFree() && ctType != goCollider.GetComponent<Connector>().ctType;
    }

    private void OnMouseUp()
    {
        print("MOUSEUP");
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
                    gameController.ManageNewConnections();
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
        this.SetConnection(connector);
        connector.SetConnection(this, false);
    }

    public void ResetCable()
    {
        this.transform.localPosition = v3StartingPos;
        cableComponent.Delete();
        goCableStart.GetComponent<Collider>().enabled = false;
        this.GetComponent<Collider>().enabled = true;
    }

    public void SetConnection(Connector newConnector, bool bMain = true)
    {
        bBeingUsed = true;
        cConnectedTo = newConnector;
        goCableStart.GetComponent<Collider>().enabled = bMain;
        this.GetComponent<Collider>().enabled = !bMain;
    }
    public void BreakConnection()
    {

        BreakConnectionTo();
        ResetCable();
        if (ctType == EEnums.ConnectorType.INPUT)
        {
            bIsLit = false;
        }
        bBeingUsed = false;
        cConnectedTo = null;
        gameController.ManageNewConnections();
    }
    public void BreakConnectionTo()
    {
        cConnectedTo.cConnectedTo = null;
        cConnectedTo.bBeingUsed = false;
        if(cConnectedTo.ctType == EEnums.ConnectorType.INPUT)
        {
            cConnectedTo.bIsLit = false;
        }
        cConnectedTo.goCableStart.GetComponent<Collider>().enabled = false;
        cConnectedTo.GetComponent<Collider>().enabled = true;
    }
    public bool IsFree()
    {
        return !bBeingUsed;
    }

    public void Lit()
    {
        bIsLit = true;
    }
    public void Unlit()
    {
        bIsLit = false;
    }

    public ScenarioObject GetScenarioObject()
    {
        return this.GetComponentInParent<ScenarioObject>();
    }
}