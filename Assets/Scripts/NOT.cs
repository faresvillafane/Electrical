using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOT : ScenarioObject
{
    public override void ShouldLit()
    {
        bool bLit = true;
        bLit = !connectorsInput[0].bIsLit;

        connectorOutput.bIsLit = bLit;

        ltLitType = CurrentLitType(bLit);
    }

}
