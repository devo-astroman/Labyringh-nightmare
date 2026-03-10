using UnityEngine;
using System.Collections.Generic;
using System;

public struct WaveData
{
    public int[] typeEnemies;         // 0-Spyder 1-Firer 2-Bat
    public Vector3[] bornPositions;
    public Vector3[][] patrolPoints;
    public Transform heroTransform;
}

public class WEnemy
{
    public int Id { get; private set; }
    public int TypeEnemy { get; private set; }   // 0-Spyder 1-Firer 2-Bat
    public Vector3 BornPoint { get; private set; }
    public Vector3[] PatrolPoints { get; private set; }
    public Transform HeroTransform { get; private set; }
    public GameObject Go { get; private set; }

    public WEnemy(int id, int typeEnemy, Vector3 bornPoint, Vector3[] patrolPoints, Transform heroTransform)
    {
        Id = id;
        TypeEnemy = typeEnemy;
        BornPoint = bornPoint;
        PatrolPoints = patrolPoints;
        HeroTransform = heroTransform;
        Go = null;
    }

    public void SetGo(GameObject go)
    {
        Go = go;
    }
}

public class Wave
{
    public int IdWave;
    public List<WEnemy> WEnemies = new List<WEnemy>();
    public int NDeads;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;

    private List<Wave> _allWaves = new List<Wave>();

    public Action<int> WaveFinishedAction;

    public int waveRunning = 0;

    public void PrepareWave(int idW, WaveData waveData)
    {
        Wave wave = new Wave();
        wave.IdWave = idW;
        wave.NDeads = 0;

        for (int i = 0; i < waveData.typeEnemies.Length; i++)
        {
            Vector3[] patrol = null;

            if (waveData.patrolPoints != null)
                patrol = waveData.patrolPoints[i];

            WEnemy wEnemy = new WEnemy(
                i,
                waveData.typeEnemies[i],
                waveData.bornPositions[i],
                patrol,
                waveData.heroTransform
            );



            wave.WEnemies.Add(wEnemy);
        }

        foreach (WEnemy we in wave.WEnemies)
        {
            GameObject g = null;

            switch (we.TypeEnemy)
            {
                case 0:
                    g = _enemyManager.CreateSpyderEnemy(we.BornPoint, we.HeroTransform);

                    break;

                case 1:
                    g = _enemyManager.CreateFirerEnemy(we.BornPoint);
                    break;

                case 2:
                    g = _enemyManager.CreateBatEnemy(we.BornPoint, we.PatrolPoints);
                    break;

                default:
                    Debug.LogWarning($"Unknown enemy type: {we.TypeEnemy}");
                    break;
            }

            if (g != null)
            {
                g.SetActive(false);
                we.SetGo(g);
            }
        }

        _allWaves.Add(wave);
    }

    public void RunWave(int idW)
    {
        // If there is a wave already running, unsubscribe first
        if (waveRunning != -1)
        {
            UnsubscribeWave(waveRunning);
        }

        waveRunning = idW;

        Wave waveToRun = _allWaves.Find(w => w.IdWave == idW);

        if (waveToRun == null)
        {
            Debug.LogError($"Wave with id {idW} was not found.");
            return;
        }

        foreach (WEnemy we in waveToRun.WEnemies)
        {
            if (we.Go == null) continue;

            we.Go.SetActive(true);

            IntNotifier dieIntNotifier = we.Go.GetComponent<IntNotifier>();

            if (dieIntNotifier != null)
            {
                dieIntNotifier.IntNotifyAction += HandleIntNotify;
            }
        }
    }

    private void HandleIntNotify(int id)
    {
        Wave wave = _allWaves.Find(w => w.IdWave == waveRunning);

        if (wave == null) return;

        if (wave.WEnemies[id].Go == null) return;

        wave.NDeads++;

        if (wave.NDeads >= wave.WEnemies.Count)
        {
            WaveFinishedAction?.Invoke(waveRunning);
        }
    }

    private void UnsubscribeWave(int idW)
    {
        Wave wave = _allWaves.Find(w => w.IdWave == idW);

        if (wave == null) return;

        foreach (WEnemy we in wave.WEnemies)
        {
            if (we.Go == null) continue;

            IntNotifier notifier = we.Go.GetComponent<IntNotifier>();

            if (notifier != null)
            {
                notifier.IntNotifyAction -= HandleIntNotify;
            }
        }
    }
}