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



    public Renderer[] rToLit;

    public Light[] lLights;
    /*
    public bool bIsLit = false;
    private bool bLastLitMode = false;
    */
    public EEnums.LitType ltLitType = EEnums.LitType.NOT_CONNECTED;
    private EEnums.LitType eltLitTypePrevious = EEnums.LitType.NOT_CONNECTED;


    public void Update()
    {
        if(ltLitType != eltLitTypePrevious)
        {
            SetLitActive(ltLitType);
        }
    }

    public void Awake()
    {
    }
    public void Start()
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
    public void SetLitActive(EEnums.LitType ltNewLit)
    {
        eltLitTypePrevious = ltLitType = ltNewLit;
        if (ltNewLit == EEnums.LitType.LIT)
        {
            Lit();
        }
        else if (ltNewLit == EEnums.LitType.UNLIT)
        {
           Unlit();
        }
        else
        {
            Disconnect();

        }
    }
    private void Disconnect()
    {
        SetLitStatus(false, gameController.DisconnectedMaterial, gameController.clrUnlit);
    }

    private void Lit()
    {
        SetLitStatus(true, gameController.litMaterial, gameController.clrLit);
    }

    private void Unlit()
    {
        SetLitStatus(true, gameController.unlitMaterial, gameController.clrUnlit);
    }

    private void SetLitStatus(bool bActive, Material mat, Color clr)
    {
        SetLightsActive(bActive, clr);
        ChangeAllLitMaterials(mat);
    }
    private void SetLightsActive(bool bEnabled, Color clr)
    {
        for(int i = 0; i< lLights.Length; i++)
        {
            ChangeLitLight(lLights[i], bEnabled, clr);
        }
    }
    private void ChangeAllLitMaterials(Material mat)
    {
        for (int i = 0; i < rToLit.Length; i++)
        {
            ChangeLitRenderer(rToLit[i], mat);
        }
    }

    protected void ChangeLitRenderer(Renderer rToLit, Material mLit)
    {
        rToLit.material = mLit;
    }

    protected void ChangeLitLight( Light lToLit, bool bEnabled, Color clr)
    {
        lToLit.enabled = bEnabled;
        lToLit.color = clr;
    }

    public bool IsLit()
    {
        return ltLitType == EEnums.LitType.LIT;
    }


    protected bool IsConnected()
    {
        bool bRes = false;

        for(int i = 0; i < connectorsInput.Length && !bRes; i++)
        {
            bRes |= connectorsInput[i].cConnectedTo != null;
        }
        return bRes;
    }

    protected EEnums.LitType CurrentLitType(bool bShouldLit)
    {
        EEnums.LitType ltRes = EEnums.LitType.NOT_CONNECTED;
        if (IsConnected())
        {
            if (bShouldLit)
            {
                ltRes = EEnums.LitType.LIT;
            }
            else
            {
                ltRes = EEnums.LitType.UNLIT;
            }
        }
        return ltRes;
    }
}
