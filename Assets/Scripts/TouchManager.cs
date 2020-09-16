using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    Ray ray;
    Ray rayD;
    RaycastHit hitD;
    RaycastHit hit;
    private GameController gc;
    private void Start()
    {
        gc = GetComponent<GameController>();
    }
    void Update()
    {
        if (EConstants.DEBUG_MODE)
        {
            rayD = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayD, out hitD))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if(hitD.collider.tag == EConstants.TAG_CONNECTOR)
                    {
                        //hitD.transform.GetComponent<MovementObject>().Rotate22D();
                    }
                }
            }
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                if (EUtils.IsScenarioObject(hit.collider.tag))
                {
                   // hit.collider.GetComponent<ScenarioObject>().HandleClick();
                    //gc.StartLevel(int.Parse(hit.transform.name));
                }
                else if (hitD.collider.tag == EConstants.TAG_CABLE_START)
                {
                    print("TOUCHED CONNECTOR");
                    
                    if (!hit.collider.GetComponent<CableComponent>().GetEndPoint().GetComponent<Connector>().IsFree())
                    {
                        hit.collider.GetComponent<CableComponent>().GetEndPoint().GetComponent<Connector>().BreakConnection();
                    }
                    
                }
            }
        }
    }
}