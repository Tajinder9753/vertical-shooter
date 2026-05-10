using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Zone_Manager : MonoBehaviour, IDataPersistance
{
    [SerializeField] CinemachineCamera virtualCamera;
    [SerializeField] CinemachineCamera permanentZoneCam;
    [SerializeField] private Base_Enemy[] zoneEnemies;
    private int remainingEnemies;
    [SerializeField] private bool zoneCleared;
    [SerializeField] GameObject[] bounds;
    [SerializeField] private bool safeArea = false;
    [SerializeField] GameObject[] enemies;
    [SerializeField] Transform[] baseEnemySpawnPoints;
    [SerializeField] Transform[] fastEnemySpawnPoints;
    [SerializeField] Transform[] strongEnemySpawnPoints;
    [SerializeField] private Tilemap exitTilemap;
    [SerializeField] float fadeInDuration = 0.5f;
    [SerializeField] string zoneName;
    [SerializeField] bool isCheckpoint;

    public void EnemyDefeated()
    {
        remainingEnemies--;

        if (remainingEnemies == 0)
        {
            zoneCleared = true;
            OpenArea();
            if (isCheckpoint)
            {
                //DataPersistanceManager.instance.SaveGame();
            }
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
        if (!safeArea && !zoneCleared)
        {
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        if (baseEnemySpawnPoints != null)
        {
            foreach (var spawnPoint in baseEnemySpawnPoints)
            {
                GameObject spawnedEnemy = Instantiate(enemies[0], spawnPoint.position, Quaternion.identity, this.transform);
                Base_Enemy enemyScript = spawnedEnemy.GetComponent<Base_Enemy>();
            }
        }


        if (fastEnemySpawnPoints != null)
        {
            foreach (var spawnPoint in fastEnemySpawnPoints)
            {
                GameObject spawnedEnemy = Instantiate(enemies[1], spawnPoint.position, Quaternion.identity, this.transform);
                Base_Enemy enemyScript = spawnedEnemy.GetComponent<Base_Enemy>();
            }
        }

        if (strongEnemySpawnPoints != null)
        {
            foreach (var spawnPoint in strongEnemySpawnPoints)
            {
                GameObject spawnedEnemy = Instantiate(enemies[2], spawnPoint.position, Quaternion.identity, this.transform);
                Base_Enemy enemyScript = spawnedEnemy.GetComponent<Base_Enemy>();
            }
        }

        zoneEnemies = GetComponentsInChildren<Base_Enemy>();
        remainingEnemies = zoneEnemies.Length;
    }

    public void OnPlayerExit()
    {
        if (!zoneCleared && !safeArea)
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

        if (isCheckpoint && safeArea)
        {
            //DataPersistanceManager.instance.SaveGame();
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

    public void LoadData(GameData data)
    {
       data.zonesCleared.TryGetValue(zoneName, out zoneCleared);

        if (zoneCleared)
        {
            this.zoneCleared = true;
        }

        if (!string.IsNullOrEmpty(data.currentCameraZone) && data.currentCameraZone == this.zoneName)
        {
            CameraManager.instance.SwitchCamera(permanentZoneCam);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.zonesCleared.ContainsKey(zoneName))
        {
            data.zonesCleared.Remove(zoneName);
        }
        data.zonesCleared.Add(zoneName, zoneCleared);

        if (CameraManager.instance.IsCurrentCamera(permanentZoneCam))
        {
            Debug.Log("Saving camera");
            data.currentCameraZone = zoneName;
        } else
        {
            Debug.Log("Not saving camera");
        }
    }
}
