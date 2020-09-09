using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioObject : MonoBehaviour
{
    public EEnums.TileType tTileType;


    public Vector3 v3Offset = Vector3.zero;
    protected GameController gameController;
    public Connector[] connectors;
    protected Collider cSnapCollider;


    public Connector GetFirstFreeConnector()
    {
        Connector cRes = null;
        for(int i = 0; i < connectors.Length && cRes == null; i++)
        {
            if (connectors[i].IsFree())
            {
                cRes = connectors[i];
            }
        }
        return connectors[0];
    }

    public void Start()
    {
        cSnapCollider = GetComponents<Collider>()[1];
    }

    public void SetLevelReference(GameController gc)
    {
        gameController = gc;
    }



    public virtual void Interact(bool bBumperRight, bool bBumperLeft)
    {
    }

    public void HandleClick()
    {

    }

    public Collider GetMetaCollider()
    {
        return cSnapCollider;
    }

}
