using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : ScenarioObject
{

    public override void ShouldLit()
    {
        //AL SER RECEIVER NO TIENE OUTPUT
        bool bLit = true;
        for (int i = 0; i < connectorsInput.Length; i++)
        {
            bLit &= connectorsInput[i].bIsLit;
        }

        ltLitType = CurrentLitType(bLit);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }



    
}
