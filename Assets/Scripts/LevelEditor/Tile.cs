using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[SelectionBase]
public class Tile : MonoBehaviour
{

    public EEnums.TileType ttTile = EEnums.TileType.EMPTY;
    private EEnums.TileType ttPrevTile = EEnums.TileType.EMPTY;

    private bool bValueChanged = false;

    public GameObject goTile;

    private void Update()
    {
        if (!bValueChanged && ttTile != ttPrevTile)
        {
            bValueChanged = true;

            GetComponentInParent<LevelEditor>().RefreshTiles();
        }
    }

    public bool HasChangedValue()
    {
        return bValueChanged;
    }

    public void SetNewTile()
    {
        ttPrevTile = ttTile;
        bValueChanged = false;
    }

    public void Rotate(float fAngle, Vector3 v3Direction)
    {
        this.transform.rotation = (this.transform.rotation * Quaternion.AngleAxis(fAngle, v3Direction));
    }

    [ContextMenu("Rotate")]
    private void Rotate22D()
    {
        Rotate(90f, Vector3.up);
    }

}
