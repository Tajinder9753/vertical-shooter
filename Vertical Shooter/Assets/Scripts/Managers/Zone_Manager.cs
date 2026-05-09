using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Zone_Manager : MonoBehaviour
{
    [SerializeField] CinemachineCamera virtualCamera;
    [SerializeField] private Base_Enemy[] zoneEnemies;
    private int remainingEnemies;
    private bool zoneCleared = false;
    [SerializeField] GameObject[] bounds;
    [SerializeField] private bool safeArea = false;
    [SerializeField] GameObject enemy;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private Tilemap exitTilemap;
    [SerializeField] float fadeInDuration = 0.5f;

    public void EnemyDefeated()
    {
        remainingEnemies--;

        if (remainingEnemies == 0)
        {
            Debug.Log("Enemies Defeated!");
            zoneCleared = true;
            OpenArea();
        }
    }

    void OpenArea()
    {
        foreach (var bound in bounds)
        {
            Collider2D collider = bound.GetComponent<Collider2D>();
            collider.isTrigger = true;
        }
        FadeOutTilemap();
    }

    public void OnPlayerEnter()
    {
        CameraManager.instance.SwitchCamera(virtualCamera);
        virtualCamera = CameraManager.instance.GetOldCamera();
        if (!safeArea)
        {
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            GameObject spawnedEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity, this.transform);
            Base_Enemy enemyScript = spawnedEnemy.GetComponent<Base_Enemy>();
        }
        zoneEnemies = GetComponentsInChildren<Base_Enemy>();
        remainingEnemies = zoneEnemies.Length;
    }

    public void OnPlayerExit()
    {
        if (!zoneCleared)
        {
            foreach (var bound in bounds)
            {
                Collider2D collider = bound.GetComponent<Collider2D>();
                collider.isTrigger = false;
            }
            foreach (var enemy in zoneEnemies)
            {
                enemy.SetActive(true);
            }

            FadeInTilemap();
        }
    }

    public void FadeInTilemap()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeOutTilemap()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        exitTilemap.gameObject.SetActive(true);
        Color c = exitTilemap.color;
        c.a = 0f;
        exitTilemap.color = c;

        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeInDuration);
            exitTilemap.color = c;
            yield return null;
        }
    }   

    IEnumerator FadeOut()
    {
        Color c = exitTilemap.color;
        c.a = 1f;
        exitTilemap.color = c;
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(elapsedTime / fadeInDuration);
            exitTilemap.color = c;
            yield return null;
        }
        exitTilemap.gameObject.SetActive(false);
    }

}
