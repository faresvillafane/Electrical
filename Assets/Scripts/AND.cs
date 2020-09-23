using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AND : ScenarioObject
{

    public Renderer[] rBlocks;

    public override void ShouldLit()
    {
        bool bLit = true;
        for (int i = 0; i < connectorsInput.Length; i++)
        {
            bLit &= connectorsInput[i].bIsLit;
        }

        connectorOutput.bIsLit = bLit && IsConnected();
        LitBlocks();
        ltLitType = CurrentLitType(bLit);
    }

    private void LitBlocks()
    {
        if (IsConnected())
        {
            for (int i = 0; i < connectorsInput.Length; i++)
            {
                if (connectorsInput[i].cConnectedTo != null && connectorsInput[i].bIsLit)
                {
                    ChangeLitRenderer(rBlocks[i], gameController.litMaterial);
                    ChangeLitLight(rBlocks[i].GetComponentInChildren<Light>(), true, gameController.clrLit);
                }
                else if (connectorsInput[i].cConnectedTo != null && !connectorsInput[i].bIsLit)
                {
                    ChangeLitRenderer(rBlocks[i], gameController.unlitMaterial);
                    ChangeLitLight(rBlocks[i].GetComponentInChildren<Light>(), true, gameController.clrUnlit);
                }
                else
                {
                    ChangeLitRenderer(rBlocks[i], gameController.DisconnectedMaterial);
                    ChangeLitLight(rBlocks[i].GetComponentInChildren<Light>(), false, gameController.clrUnlit);
                }
            }

        }
        else
        {
            for (int i = 0; i < rBlocks.Length; i++)
            {
                ChangeLitRenderer(rBlocks[i], gameController.DisconnectedMaterial);
                ChangeLitLight(rBlocks[i].GetComponentInChildren<Light>(), false, gameController.clrUnlit);
            }
        }
    }

}
