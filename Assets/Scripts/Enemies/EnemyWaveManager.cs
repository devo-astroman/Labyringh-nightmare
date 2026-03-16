using UnityEngine;
using System;

public struct EnemyInfo
{
    public int EnemyType;
    public Vector3 BornPoint;
    public Vector3 PatrolPoint;
    public GameObject Go;
}

public static class EnemyInfoFactory
{
    public static EnemyInfo CreateSpyderEnemy(Vector3 bornPoint)
    {
        return new EnemyInfo()
        {
            EnemyType = 0,
            BornPoint = bornPoint,
            PatrolPoint = Vector3.zero,
            Go = null,
        };
    }

    public static EnemyInfo CreateFirerEnemy(Vector3 bornPoint)
    {
        return new EnemyInfo()
        {
            EnemyType = 1,
            BornPoint = bornPoint,
            PatrolPoint = Vector3.zero,
            Go = null,
        };
    }
    public static EnemyInfo CreateBatEnemy(Vector3 bornPoint, Vector3 patrolPoint)
    {
        return new EnemyInfo()
        {
            EnemyType = 2,
            BornPoint = bornPoint,
            PatrolPoint = patrolPoint,
            Go = null,
        };
    }
}



public class EnemyWaveManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Transform _generatedTransform;
    #endregion 
    #region Private Fields
    private EnemyInfo[] wave1;
    private EnemyInfo[] wave2;
    private EnemyInfo[] wave3;
    private EnemyInfo[] wave4;
    private EnemyInfo[] wave5;
    private EnemyInfo[] wave6;

    private Transform _heroTransform;
    private Transform _enemiesParent;
    #endregion 

    #region Unity Callbacks
    void Start()
    {
        _enemiesParent= _generatedTransform.Find("Enemies");
    }
    #endregion

    #region Public Methods
    public void CreateInfoWaves()
    {
        //wave1
        wave1 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(5,2,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(2,3,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(1,5,0)),
        };

        //wave2
        wave2 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(1,6,0)),
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(2,7,0)),
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(5,4,0)),
        };

        //wave3
        wave3 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateBatEnemy(new Vector3(6,9,0),new Vector3(6,5,0)),
            EnemyInfoFactory.CreateBatEnemy(new Vector3(4,4,0),new Vector3(3,2,0)),
        };

        //wave4
        wave4 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(8,7,0)),
            EnemyInfoFactory.CreateBatEnemy(new Vector3(9,8,0),new Vector3(9,4,0)),
        };

        //wave5
        wave5 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(8,3,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(8,4,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(7,4,0)),
            EnemyInfoFactory.CreateBatEnemy(new Vector3(8,5,0),new Vector3(8,1,0)),
        };

        //wave6
        wave6 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(7,5,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(7,6,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(2,4,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(1,4,0)),
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(9,5,0)),
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(9,8,0)),
            EnemyInfoFactory.CreateBatEnemy(new Vector3(0,4,0),new Vector3(7,7,0)),
        };
    }

    public void ConvertPositions(Func<int, int, Vector3> callback)
    {
        ConvertWavePositions(wave1, callback);
        ConvertWavePositions(wave2, callback);
        ConvertWavePositions(wave3, callback);
        ConvertWavePositions(wave4, callback);
        ConvertWavePositions(wave5, callback);
        ConvertWavePositions(wave6, callback);
    }

    public void SetHeroTransform(Transform heroTransform)
    {
        _heroTransform = heroTransform;
    }

    public void PrepareEnemies()
    {
        InstantiateEnemies(wave1);
        InstantiateEnemies(wave2);
        InstantiateEnemies(wave3);
        InstantiateEnemies(wave4);
        InstantiateEnemies(wave5);
        InstantiateEnemies(wave6);
    }

    public void RunWave(int idWave)
    {
        if (idWave == 1)
        {
            foreach(EnemyInfo eInfo in wave1)
            {
                eInfo.Go.SetActive(true);
            }
        }
    }

    public void DebugWaves()
    {
        DebugWave(wave1);
        /* DebugWave(wave2);
        DebugWave(wave3);
        DebugWave(wave4);
        DebugWave(wave5);
        DebugWave(wave6); */
    }

    #endregion
    #region Private Methods
    private void ConvertWavePositions(EnemyInfo[] wave, Func<int, int, Vector3> converterFn)
    {
        for (int i = 0; i < wave.Length; i++)
        {
            wave[i].BornPoint = converterFn((int)wave[i].BornPoint.x, (int)wave[i].BornPoint.y);

            if (wave[i].EnemyType == 2)
            {
                wave[i].PatrolPoint = converterFn((int)wave[i].PatrolPoint.x, (int)wave[i].PatrolPoint.y);
            }
        }
    }

    private void InstantiateEnemies(EnemyInfo[] wave)
    {
        for (int i = 0; i < wave.Length; i++)
        {
            GameObject g = null;
            switch (wave[i].EnemyType)
            {
                case 0:
                    g = _enemyManager.CreateSpyderEnemy(wave[i].BornPoint, _heroTransform,_enemiesParent);
                    break;
                case 1:
                    g = _enemyManager.CreateFirerEnemy(wave[i].BornPoint,_enemiesParent);
                    break;
                case 2:
                    g = _enemyManager.CreateBatEnemy(wave[i].BornPoint, new Vector3[]{wave[i].BornPoint, wave[i].PatrolPoint},_enemiesParent);
                    break;
            }
            if (g != null)
            {
                wave[i].Go = g;
                g.SetActive(false);
            }
            

        }
    }

    private void DebugWave(EnemyInfo[] wave)
    {
        foreach (EnemyInfo eInfo in wave)
        {
            Debug.Log("born point " + eInfo.BornPoint);
            Debug.Log("patrol point " + eInfo.PatrolPoint);
        }
    }

    #endregion

}