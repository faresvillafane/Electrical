using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
[ExecuteInEditMode]
public class LevelEditor : MonoBehaviour
{
    private Transform tContainer;
    public GameObject prefEditorTile, prefInput, prefOutput, prefAND;

    [Range(7,50)]
    public int iTileSize = 5;

    public GameObject[] tiles;

    private const float REFRESH_EVERY_SECOND = .15f;

    public LevelData lvlExit;

    private int iNumberOfObjects = 0;
    private int iNumberOfMovementObjects = 0;
    private int iNumberOfColors = 0;


    // Start is called before the first frame update
    void Start()
    {
        tContainer = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Build NEW Level")]
    private void BuildLevel()
    {
        DeleteLevel();
        BuildLevelTiles(iTileSize);
    }

    private void BuildLevelTiles(int iTiles)
    {
        tiles = new GameObject[iTiles * iTiles];
        print("building..");
        for (int x = 0; x < iTiles; x++)
        {
            for (int z = 0; z < iTiles; z++)
            {
                GameObject goNewEditorTile = Instantiate(prefEditorTile, new Vector3(x, 0.4f, z), Quaternion.identity);
                goNewEditorTile.transform.SetParent(tContainer);

                int index = EUtils.MatrixIndexesToListIndex(x, z, iTiles);
                tiles[index] = goNewEditorTile;
            }

        }
    }


    [ContextMenu("DELETE Level")]
    private void DeleteLevel()
    {
        iNumberOfObjects = 0;
        iNumberOfColors = 0;
        iNumberOfMovementObjects = 0;
        EUtils.DeleteAllChildren(tContainer);
    }
 

    public void RefreshTiles()
    {

        for (int i=0; i < tiles.Length; i++)
        {
            if (tiles[i].GetComponent<Tile>().HasChangedValue())
            {
                EUtils.DeleteAllChildrenWithException(tiles[i].transform, "Tile");

                tiles[i].GetComponent<Tile>().SetNewTile();

                
                tiles[i].GetComponent<Tile>().goTile.SetActive(true);
                GameObject goToInstantiate = null;
                switch (tiles[i].GetComponent<Tile>().ttTile)
                {
                    case EEnums.TileType.INPUT:
                        goToInstantiate = prefInput;
                        break;
                    case EEnums.TileType.OUTPUT:
                        goToInstantiate = prefOutput;
                        break;
                    case EEnums.TileType.AND:
                        goToInstantiate = prefAND;
                        break;
                }

                if (goToInstantiate != null)
                {
                    GameObject goNewObj = Instantiate(goToInstantiate, Vector3.zero , Quaternion.identity);

                    goNewObj.transform.SetParent(tiles[i].transform);
                    goNewObj.transform.localPosition = new Vector3(0, 0.2f , 0) + goToInstantiate.GetComponent<ScenarioObject>().v3Offset;
                    goNewObj.transform.localRotation = Quaternion.identity;

                   
                    iNumberOfObjects++;
                    
                }
            }
        }
    }

    [ContextMenu("EXPORT Level")]
    public void GetLevelData()
    {
        lvlExit.levelBuild = "";
        Vector3[] v3ObjectsRotations = new Vector3[iNumberOfObjects];
        Color[] clrObjects = new Color[iNumberOfColors];
        bool[] bMovementObjects = new bool[iNumberOfMovementObjects];

        int iObjIndex = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            lvlExit.levelBuild += ((int)tiles[i].GetComponent<Tile>().ttTile).ToString() + EConstants.LEVEL_SEPARATOR;

            if (EUtils.IsScenarioObject(tiles[i].GetComponent<Tile>().ttTile))
            {
                v3ObjectsRotations[iObjIndex] = tiles[i].transform.rotation.eulerAngles;
                iObjIndex++;
            }

        }

       
        lvlExit.objRotation = new Vector3[iNumberOfObjects];
        lvlExit.objRotation = (Vector3[])v3ObjectsRotations.Clone();

        lvlExit.levelBuild = lvlExit.levelBuild.Substring(0, lvlExit.levelBuild.Length - 1);
        lvlExit.MaxSize = iTileSize;
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorUtility.SetDirty(lvlExit);
#endif

        print("DATA GENERATED!");
    }

    [ContextMenu("BUILD Level From Data")]
    public void GenerateLevelFromLevelData()
    {
        DeleteLevel();
        iTileSize = lvlExit.MaxSize;

        BuildLevelTiles(lvlExit.MaxSize);

        string[] sLevelData = lvlExit.levelBuild.Split(EConstants.LEVEL_SEPARATOR);
        int iRotationIdx = 0;
        for(int i= 0; i < sLevelData.Length; i++)
        {
            
            tiles[i].GetComponent<Tile>().ttTile = (EEnums.TileType)int.Parse(sLevelData[i]);
            
            if (EUtils.IsScenarioObject(tiles[i].GetComponent<Tile>().ttTile))
            {
                tiles[i].transform.rotation = Quaternion.Euler(lvlExit.objRotation[iRotationIdx]);

                iRotationIdx++;
            }

        }
        RefreshTiles();

    }

}
