using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    
    public GameObject goTile, goInput, goOutput, goAND;
    public LevelData[] levels;


    private GameController gameController;
    public CameraManager cameraManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameController = GetComponent<GameController>();
    }

    public void Init()
    {
        LevelData curLevel = gameController.GetCurrentLevel();
        GenerateBaseLevel(curLevel);
        gameController.SetLevelStatus(curLevel.levelBuild, curLevel.MaxSize);
    }

    public void DeleteLevel()
    {
        EUtils.DeleteAllChildren(this.transform);
    }

    public void GenerateBaseLevel(LevelData curLevel)
    {
        int iObjectPlacementNumber = 0;
        string[] sLevelObjects = curLevel.levelBuild.Split(EConstants.LEVEL_SEPARATOR);
        gameController.scenarioObjects = new GameObject[curLevel.objRotation.Length];
        for(int x = 0; x < curLevel.MaxSize; x++)
        {
            for (int z = 0; z < curLevel.MaxSize; z++)
            {
                GameObject goNewTile = Instantiate(goTile, new Vector3(x, 0.4f, z), Quaternion.identity);
                goNewTile.transform.SetParent(this.transform);
                int index = EUtils.MatrixIndexesToListIndex(x, z, curLevel.MaxSize);
                
                if (index == (curLevel.MaxSize * curLevel.MaxSize) / 2)
                {
                    cameraManager.PlaceCamera(curLevel.MaxSize, goNewTile);
                }
                GameObject goToInstantiate = null;
                switch ((EEnums.TileType) int.Parse(sLevelObjects[index]))
                {
                    case EEnums.TileType.INPUT:
                        goToInstantiate = goInput;
                        break;
                    case EEnums.TileType.OUTPUT:
                        goToInstantiate = goOutput;
                        break;
                    case EEnums.TileType.AND:
                        goToInstantiate = goAND;
                        break;
                }

                if(goToInstantiate != null)
                {
                    GameObject goNewObj = Instantiate(goToInstantiate, new Vector3(x, 1, z) + goToInstantiate.GetComponent<ScenarioObject>().v3Offset, Quaternion.identity);
                    goNewObj.transform.SetParent(this.transform);
                    goNewObj.GetComponent<ScenarioObject>().SetLevelReference(gameController);
                    gameController.scenarioObjects[iObjectPlacementNumber] = goNewObj;

                    goNewObj.transform.rotation = Quaternion.Euler(curLevel.objRotation[iObjectPlacementNumber]);
                    goNewObj.name += iObjectPlacementNumber;
                    iObjectPlacementNumber++;

                }

            }

        }
    }


}
