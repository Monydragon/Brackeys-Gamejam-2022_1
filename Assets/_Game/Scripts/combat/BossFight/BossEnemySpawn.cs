using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawn : MonoBehaviour
{
    public enum EditorIcon { Slime, Skeleton, Crow, Ghost, Mushroom, Spider }

    public GameObject PrefabToSpawn;
    public EditorIcon displayIcon = EditorIcon.Slime;

    public GameObject LastSpawnedEnemy;
    private bool isEnemyDead = true;

    private void OnEnable()
    {
        EventManager.onObjectDied += OnObjectDied;
    }
    private void OnObjectDied(GameObject objectThatDied)
    {
        if (LastSpawnedEnemy != null ? objectThatDied == LastSpawnedEnemy : false)
        {
            isEnemyDead = true;
        }
    }
    public bool IsEnemyDead()
    {
        return isEnemyDead;
    }

    public void SpawnEnemy()
    {

        LastSpawnedEnemy = Instantiate(PrefabToSpawn, transform.position, Quaternion.identity);
        isEnemyDead = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
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

