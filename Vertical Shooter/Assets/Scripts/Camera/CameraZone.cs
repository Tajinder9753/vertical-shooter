using Unity.Cinemachine;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    [SerializeField] CinemachineCamera virtualCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraManager.instance.SwitchCamera(virtualCamera);
            virtualCamera = CameraManager.instance.GetOldCamera(virtualCamera);
        }
    }
}
