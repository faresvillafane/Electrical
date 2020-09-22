using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioObject : MonoBehaviour
{
    public EEnums.TileType tTileType;


    public Vector3 v3Offset = Vector3.zero;
    protected GameController gameController;

    public Connector connectorOutput;
    public Connector[] connectorsInput;


    public bool bIsLit = false;

    public Material litMaterial, unlitMaterial;

    public Renderer[] rToLit;

    public Light lLight;

    private bool bLastLitMode = false;

    public void Update()
    {
        if(bIsLit != bLastLitMode)
        {
            SetLitActive(bIsLit);
        }
    }

    public void Awake()
    {
        ShouldLit();
    }

    public void SetLevelReference(GameController gc)
    {
        gameController = gc;
    }

    public virtual void ShouldLit()
    {
        print("SHOULD LIT SCENARIO OBJECT");
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
        print("unlit");
        for (int i = 0; i < rToLit.Length; i++)
        {
            rToLit[i].material = unlitMaterial;
        }
    }


}
