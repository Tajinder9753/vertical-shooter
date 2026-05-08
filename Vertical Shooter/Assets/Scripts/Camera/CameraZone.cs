using Unity.Cinemachine;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    private Zone_Manager zoneManager;

    private void Awake()
    {
        zoneManager = GetComponentInParent<Zone_Manager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            zoneManager.OnPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            zoneManager.OnPlayerExit();
        }
    }
}
