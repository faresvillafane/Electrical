using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private GameObject goCenterTile;


    public float xSpeed = 3.5f;
    float sensitivity = 17f;

    float minFov = 35;
    float maxFov = 100;

    private Vector3 v3CameraStartPos;
    private Quaternion qCameraStartRot;
    private float fStartFov;

    public GameController gc;

   // private float MAX_ANGLE_ROTATION = 30;

    //private float fCurrentAngleRotation = 0;

    // Start is called before the first frame update
    void Awake()
    {
        gc = GameObject.FindGameObjectWithTag(EConstants.TAG_GAMECONTROLLER).GetComponent<GameController>();
        v3CameraStartPos = this.transform.position;
        qCameraStartRot = transform.rotation;
        fStartFov = Camera.main.fieldOfView;
    }
    
    public void PlaceCamera(int iMaxSize, GameObject goCenterTile)
    {
       // fCurrentAngleRotation = 0;
        this.transform.position = goCenterTile.transform.position + new Vector3(0, iMaxSize * .6f, -iMaxSize * .6f);
        transform.LookAt(goCenterTile.transform.position);
        this.goCenterTile = goCenterTile;
    }

    public void InitMenu()
    {
        transform.position = v3CameraStartPos;
        transform.rotation = qCameraStartRot;
        Camera.main.fieldOfView = fStartFov;
    }

    // Update is called once per frame
    void Update()
    {
        //if (gc.bInLevel)
        {
            if (Input.GetMouseButton(1))
            {
                //float fCalcNextRotation = Input.GetAxis("Mouse X") * xSpeed + fCurrentAngleRotation;
                //if (fCalcNextRotation >= -MAX_ANGLE_ROTATION && fCalcNextRotation <= MAX_ANGLE_ROTATION)
                {
                    transform.RotateAround(goCenterTile.transform.position, Vector3.up, Input.GetAxis("Mouse X") * xSpeed);
                 //   fCurrentAngleRotation = fCalcNextRotation;
                }
                //transform.RotateAround(goCenterTile.transform.position, transform.right, -Input.GetAxis("Mouse Y") * xSpeed);

            }
            
            //ZOOM

            float fov = Camera.main.fieldOfView;
            fov += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;

        }

    }
}
