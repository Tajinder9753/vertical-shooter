using Unity.Cinemachine;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    //[SerializeField] CinemachineCamera virtualCamera;
    //[SerializeField] private Base_Enemy[] zoneEnemies;
    //private int remainingEnemies;
    //private bool zoneCleared = false;
    //[SerializeField] GameObject[] bounds;
    //[SerializeField] private bool safeArea = false;
    //private void Awake()
    //{
    //    if (!safeArea)
    //    {

    //        remainingEnemies = zoneEnemies.Length;
    //    }

    //}

    //public void EnemyDefeated()
    //{
    //    remainingEnemies--;

    //    if (remainingEnemies == 0)
    //    {
    //        Debug.Log("Enemies Defeated!");
    //        zoneCleared = true;
    //        OpenArea();
    //    }
    //}

    //void OpenArea()
    //{
    //    foreach (var bound in bounds)
    //    {
    //        Collider2D collider = bound.GetComponent<Collider2D>();
    //        collider.isTrigger = true;
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        CameraManager.instance.SwitchCamera(virtualCamera);
    //        virtualCamera = CameraManager.instance.GetOldCamera();
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (!zoneCleared)
    //    {
    //        foreach (var bound in bounds)
    //        {
    //            Collider2D collider = bound.GetComponent<Collider2D>();
    //            collider.isTrigger = false;
    //        }
    //        foreach (var enemy in zoneEnemies)
    //        {
    //            enemy.SetActive(true);
    //        }
    //    }
    //}

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
