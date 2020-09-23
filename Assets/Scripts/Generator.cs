using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : ScenarioObject
{
    public EEnums.LitType ltStartOn = EEnums.LitType.LIT;

    public override void ShouldLit()
    {
        //AL SER RECEIVER NO TIENE INPUT
        print("GENERATOR SHOULD LIT");
    }

    private new void Awake()
    {
        base.Awake();
        ltLitType = ltStartOn;
        connectorOutput.bIsLit = IsLit();
    }


    void OnMouseDown()
    {
        connectorOutput.bIsLit = !connectorOutput.bIsLit;
        ltLitType = (ltLitType == EEnums.LitType.LIT) ? EEnums.LitType.UNLIT : EEnums.LitType.LIT;

        gameController.ManageNewConnections();
    }

}
