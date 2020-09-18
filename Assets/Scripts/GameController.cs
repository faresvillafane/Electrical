using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool bIsComplete = false;

    private const float CHECK_WIN_CONDITION = 1;
    private float fCurrentTime = 0;
    private int[,] iLevelStatus;

    private bool bToNextLevel = false;

    private LevelBuilder levelBuilder;

    public GameObject canvas;

    public int iCurrentLevel = 0;

    public GameObject[] scenarioObjects;

    public bool bInLevel = false;

    private const float REGISTER_PRESS_IN = .25f;
    private float fButtonPressTime = 0;

    void Start()
    {
        canvas.SetActive(true);
        levelBuilder = GetComponent<LevelBuilder>();
        Init();
    }

    public void Init()
    {
        levelBuilder.Init();
        bInLevel = true;
        bToNextLevel = false;
        bIsComplete = false;
    }

    public void SetLevelStatus(string sLevel, int iLevelDim)
    {
        string[] sLevelArray = sLevel.Split(EConstants.LEVEL_SEPARATOR);
        iLevelStatus = new int[iLevelDim, iLevelDim];
        for (int x = 0; x < iLevelDim; x++)
        {
            for (int z = 0; z < iLevelDim; z++)
            {
                int index = EUtils.MatrixIndexesToListIndex(x,z,iLevelDim);
                iLevelStatus[x, z] = int.Parse(sLevelArray[index]);
            }
        }
    }

    void Update()
    {
        if (bInLevel)
        {
            fCurrentTime += Time.deltaTime;
            if (!bIsComplete && fCurrentTime >= CHECK_WIN_CONDITION)
            {
                fCurrentTime = 0;
                bool bIsCompletePrev = CheckLevelCompletion();
                bIsComplete = bIsCompletePrev;
            }
            else if (!bToNextLevel && bIsComplete)
            {

                bToNextLevel = true;

                StartCoroutine(PrepareNextLevel());
            }

            if (Input.GetButton(EConstants.INPUT_BUMPER_LEFT))
            {
                fButtonPressTime += Time.deltaTime;
                if(fButtonPressTime >= REGISTER_PRESS_IN)
                {
                    fButtonPressTime = 0;
                    ResetLevel();
                }
            }
          
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                      
            Application.Quit();
            
        }
    }

    private bool CheckLevelCompletion()
    {
        bool bIsComplete = false;
        //for(int i = 0; receivers != null && i < receivers.Length; i++)
        {
            //bIsComplete &= receivers[i].GetComponentInParent<Receiver>().IsSolved();
        }

        return bIsComplete;
    }

    public IEnumerator PrepareNextLevel()
    {
        yield return new WaitForSeconds(2f);
        levelBuilder.DeleteLevel();
        NextLevel();
        Init();

    }

    public void ResetLevel()
    {
        levelBuilder.DeleteLevel();
        Init();
    }

    public void StartLevel(int iLvl)
    {
        levelBuilder.DeleteLevel();
        iCurrentLevel = iLvl;
        Init();
    }

    public LevelData GetCurrentLevel()
    {
        return levelBuilder.levels[iCurrentLevel];
    }

    public void NextLevel()
    {
        iCurrentLevel = (iCurrentLevel == levelBuilder.levels.Length - 1) ? iCurrentLevel : iCurrentLevel + 1;
    }

    public void SetSnapCollider(bool bActive)
    {
        float iSize = (bActive)? 5: 1/5f;

        for(int i = 0; i < scenarioObjects.Length; i++)
        {
            scenarioObjects[i].GetComponentInChildren<Connector>().GetComponent<SphereCollider>().radius = scenarioObjects[i].GetComponentInChildren<Connector>().GetRadius(bActive);

        }
    }

}
