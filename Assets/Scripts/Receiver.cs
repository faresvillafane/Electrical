using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : ScenarioObject
{

    public override void ShouldLit()
    {
        bool bLit = true;
        for (int i = 0; i< connectorsInput.Length; i++)
        {
            bLit &= connectorsInput[i].bIsLit;
        }

        bIsLit = bLit;
        print("RECEIVER SHOULD LIT " + connectorsInput.Length + " // " + bLit);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


}
