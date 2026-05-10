using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance {  get; private set; }

    [SerializeField] CinemachineCamera startingCamera;

    private CinemachineCamera currentCamera;

    private Stack<CinemachineCamera> cameraStack = new Stack<CinemachineCamera>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (currentCamera == null)
        {
            SwitchCamera(startingCamera);
        }
    }

    public void SwitchCamera(CinemachineCamera newCamera)
    {
        if (currentCamera != null)
        {
            cameraStack.Push(currentCamera);
            currentCamera.Priority = 0;
        }
        newCamera.Priority = 10;
        currentCamera = newCamera;
    }

    public CinemachineCamera GetOldCamera()
    {
        return cameraStack.Pop();
    }

    public bool IsCurrentCamera(CinemachineCamera camera)
    {
        return currentCamera == camera;
    }
}
