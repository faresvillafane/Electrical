using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EUtils : MonoBehaviour
{
    public static  void DeleteAllChildren(Transform t)
    {
        DeleteAllChildrenWithException(t, "");
    }

    public static void DeleteAllChildrenWithException(Transform t, string sNameException)
    {
        int iNumOfChilds = t.childCount;
        for (int i = iNumOfChilds - 1; i >= 0; i--)
        {
            if (t.GetChild(i).name != sNameException)
            {
                GameObject.DestroyImmediate(t.GetChild(i).gameObject);
            }
        }
    }

    public static int MatrixIndexesToListIndex(Vector3 v3Pos, int iDim)
    {
        return (int)v3Pos.z + (int)v3Pos.x * iDim;
    }

    public static Vector3 ListToMatrix(int idx, int maxDim)
    {
        idx++;
        Vector3 v2Sol = new Vector3(idx % maxDim, 0, idx / maxDim);
        return v2Sol;
    }

    public static int MatrixIndexesToListIndex(int x, int z, int iDim)
    {
        return MatrixIndexesToListIndex(new Vector3(x,0,z), iDim);
    }

   
    public static bool AtTarget(float f1, float f2, float fThreshold = 0.01f)
    {
        return Mathf.Abs(f1 - f2) <= fThreshold;
    }

    public static bool IsScenarioObject(EEnums.TileType tt)
    {
        return (tt == EEnums.TileType.INPUT || tt == EEnums.TileType.OUTPUT || tt == EEnums.TileType.AND);
    }

    public static bool IsScenarioObject(string sTag)
    {
        return (sTag == EConstants.TAG_INPUT || sTag == EConstants.TAG_OUTPUT || sTag == EConstants.TAG_AND);
    }

}
