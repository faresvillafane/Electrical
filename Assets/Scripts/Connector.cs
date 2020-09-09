using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Connector : MonoBehaviour

{
    private Vector3 mOffset;

    private float mZCoord;

    private CableComponent cableComponent;

    private bool bBeingUsed = false;

    private GameController gameController;

    private ScenarioObject soConnectedTo;
   // private Plane p;
    private void Awake()
    {
        cableComponent = GetComponentInParent<CableComponent>();
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
                if (EUtils.IsScenarioObject(hit.collider.tag) && hit.collider.gameObject != this.transform.parent.transform.parent.gameObject)
                {
                    this.transform.position = hit.collider.GetComponent<ScenarioObject>().GetFirstFreeConnector().transform.position;
                }
                else if (hit.collider.CompareTag(EConstants.TAG_CONNECTOR) && hit.collider.gameObject != this)
                {
                    this.transform.position = hit.collider.transform.position;
                }
            }
        }
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
                if (EUtils.IsScenarioObject(hit.collider.tag) && hit.collider.gameObject != this.transform.parent.transform.parent.gameObject)
                {
                    this.transform.position = hit.collider.GetComponent<ScenarioObject>().GetFirstFreeConnector().transform.position;

                    SetConnections(hit.collider.GetComponentInChildren<Connector>(), hit.collider.GetComponent<ScenarioObject>());
                }
                else if(hit.collider.CompareTag(EConstants.TAG_CONNECTOR) && hit.collider.gameObject != this)
                {
                    this.transform.position = hit.collider.transform.position;
                    SetConnections(hit.collider.GetComponent<Connector>(), hit.collider.GetComponentInParent<ScenarioObject>());

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

    private void SetConnections(Connector connector, ScenarioObject soConnected)
    {
        connector.SetConnection(this.GetComponentInParent<ScenarioObject>());
        this.SetConnection(soConnected);
    }

    private void ResetCable()
    {
        this.transform.localPosition = Vector3.zero;
        cableComponent.Delete();

    }

    public void SetConnection(ScenarioObject newSO)
    {
        bBeingUsed = true;
        soConnectedTo = newSO;
    }
    public void BreakConnection()
    {
        bBeingUsed = false;
        soConnectedTo = null;
    }
    public bool IsFree()
    {
        return !bBeingUsed;
    }

}