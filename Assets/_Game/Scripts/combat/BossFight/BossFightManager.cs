using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BossFightManager : MonoBehaviour
{
    public enum BossFightState { NotStarted,FightingBoss,FightingEnemies,TransitioningToBoss,TrasitioningToEnemies,Ended}
    public BossFightState fightState = BossFightState.NotStarted;
    public int numberOfRounds = 3;
    public List<BossEnemySpawn> bossEnemySpawns = new List<BossEnemySpawn>();
    public BossEnemySpawn bossSpawner;
    public SongScriptableObject BossFightSong;
    public SongScriptableObject PostFightSong;
    public BlockReference dialogueBlock;
    private int round = 0;

    public void StartBossFight()
    {
        bossSpawner.SpawnEnemy();
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayRequestedSong(BossFightSong, true);
        }
        fightState = BossFightState.FightingBoss;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) StartBossFight();
        switch (fightState)
        {
            case BossFightState.NotStarted:
                break;
            case BossFightState.FightingBoss:
                if (bossSpawner.IsEnemyDead())
                {
                    round++;
                    if(round >= numberOfRounds)
                    {
                        if (BackgroundMusicManager.Instance != null)
                        {
                            BackgroundMusicManager.Instance.PlayRequestedSong(PostFightSong, true);
                        }
                        fightState = BossFightState.Ended;
                        dialogueBlock.Execute();
                        /////////////////////////////////////////////////
                        ///Connect To Further Logic After the fight here:
                        /////////////////////////////////////////////////
                        
                    }
                    else
                    {
                        if(bossSpawner.LastSpawnedEnemy != null)
                        {
                            bossSpawner.LastSpawnedEnemy.GetComponent<Animator>().SetBool("isDisappear", true);
                        }
                        fightState = BossFightState.TrasitioningToEnemies;
                        SpawnAllEnemies();
                    }
                }
                    
                break;
            case BossFightState.FightingEnemies:
                if (CheckAllEnemiesDead())
                {
                    fightState = BossFightState.TransitioningToBoss;
                    bossSpawner.SpawnEnemy();
                    fightState = BossFightState.FightingBoss;
                }
                break;
            default:
                break;
        }
    }
    private void SpawnAllEnemies()
    {
        foreach (BossEnemySpawn spawn in bossEnemySpawns)
        {
            spawn.SpawnEnemy();
            fightState = BossFightState.FightingEnemies;
        }
    }
    private bool CheckAllEnemiesDead()
    {
        foreach (BossEnemySpawn spawn in bossEnemySpawns)
        {
            if (!spawn.IsEnemyDead()) return false;
        }
        return true;
    }
}
