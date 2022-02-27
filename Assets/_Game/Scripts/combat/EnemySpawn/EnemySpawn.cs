using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public enum EditorIcon{Slime, Skeleton, Crow,Ghost,Mushroom,Spider}

    public GameObject PrefabToSpawn;
    public bool limitNumberToSpawn;
    public int numberToSpawn;
    public bool ShouldRespawn = true;
    public float RespawnTime = 5f;
    public EditorIcon displayIcon = EditorIcon.Slime;

    private GameObject LastSpawnedEnemy;
    private int spawnedAmount;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy();
    }
    private void OnEnable()
    {
        EventManager.onObjectDied += OnObjectDied;
    }
    private void OnObjectDied(GameObject objectThatDied)
    {
        if(LastSpawnedEnemy != null ? objectThatDied == LastSpawnedEnemy : false)
        {
            if (ShouldRespawn == true)
            {
                if (limitNumberToSpawn == true)
                {
                    if (spawnedAmount < numberToSpawn)
                    {
                        StartCoroutine(RespawnEnemy(RespawnTime));
                    }
                }
                else
                {
                    StartCoroutine(RespawnEnemy(RespawnTime));
                }
            }
        }
    }
    
    private IEnumerator RespawnEnemy(float timeToRespawn)
    {
        yield return new WaitForSeconds(timeToRespawn);
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        
        spawnedAmount++;
        LastSpawnedEnemy = Instantiate(PrefabToSpawn, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,new Vector3(1f,1f,1f));
        Gizmos.color = Color.white;
        switch (displayIcon)
        {
            case EditorIcon.Slime:
                Gizmos.DrawIcon(transform.position, "slime.tiff", true);
                break;
            case EditorIcon.Spider:
                Gizmos.DrawIcon(transform.position, "spider.tiff", true);
                break;
            case EditorIcon.Mushroom:
                Gizmos.DrawIcon(transform.position, "mushroom.tiff", true);
                break;
            case EditorIcon.Ghost:
                Gizmos.DrawIcon(transform.position, "ghost.tiff", true);
                break;
            case EditorIcon.Crow:
                Gizmos.DrawIcon(transform.position, "crow.tiff", true);
                break;
            case EditorIcon.Skeleton:
                Gizmos.DrawIcon(transform.position, "skeleton.tiff", true);
                break;
            default:
                break;
        }

    }
}
