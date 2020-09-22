using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : ScenarioObject
{
    public bool bStartOn = false;

    public override void ShouldLit()
    {
        //connectorOutput.bIsLit = connectorOutput.bIsLit;

        print("GENERATOR SHOULD LIT");
    }

    private new void Awake()
    {
        base.Awake();
        connectorOutput.bIsLit = bIsLit = bStartOn;
    }


    void OnMouseDown()
    {
        connectorOutput.bIsLit = bIsLit = !bIsLit;
        gameController.ManageNewConnections();
    }

}
