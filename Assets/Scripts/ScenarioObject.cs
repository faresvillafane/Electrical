using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioObject : MonoBehaviour
{
    public EEnums.TileType tTileType;


    public Vector3 v3Offset = Vector3.zero;
    protected GameController gameController;
    public Connector[] connectors;

    public bool bStartOn = false;

    public bool bIsLit = false;

    public Material litMaterial, unlitMaterial;

    public Renderer[] rToLit;

    public Light lLight;

    private bool bLastLitMode = false;

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

    public void Update()
    {
        if(bIsLit != bLastLitMode)
        {
            SetLitActive(bIsLit);
        }
    }

    public void Awake()
    {
        SetLitActive(bStartOn);
    }

    public void SetLevelReference(GameController gc)
    {
        gameController = gc;
    }

    public virtual bool ShouldLit()
    {
        return false;
    }




    public virtual void Interact(bool bBumperRight, bool bBumperLeft)
    {
    }

    public void HandleClick()
    {

    }
    public void SetLitActive(bool bActive)
    {
        bLastLitMode = bIsLit = bActive;
        if (bActive)
        {
            Lit();
        }
        else
        {
           Unlit();
        }
    }
    private void Lit()
    {
        lLight.enabled = true;
        for(int i = 0; i < rToLit.Length; i++)
        {
            rToLit[i].material = litMaterial;
        }
    }

    private void Unlit()
    {
        lLight.enabled = false;

        for (int i = 0; i < rToLit.Length; i++)
        {
            rToLit[i].material = unlitMaterial;
        }
    }

}
