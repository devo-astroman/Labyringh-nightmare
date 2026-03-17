using UnityEngine;
using System;

public struct EnemyInfo
{
    public int Id;
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
            Id=0,
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
            Id=0,
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
            Id=0,
            EnemyType = 2,
            BornPoint = bornPoint,
            PatrolPoint = patrolPoint,
            Go = null,
        };
    }
}

public struct WaveInfo
{
    public int Id;
    public bool allDead;
    public int nEnemies;
    public int nDeads;
    

}

public class EnemyWaveManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Transform _generatedTransform;
    [SerializeField] private EnemyHeroTarget _enemyHeroTarget;
    #endregion 
    #region Public Fields
    public Action<int> waveAllDeadAction;
    #endregion 

    #region Private Fields
    private EnemyInfo[] _wave1;
    private EnemyInfo[] _wave2;
    private EnemyInfo[] _wave3;
    private EnemyInfo[] _wave4;
    private EnemyInfo[] _wave5;
    private EnemyInfo[] _wave6;

    private WaveInfo[] _wavesInfo;

    private Transform _heroTransform;
    private Transform _enemiesParent;

    private int _currentWave = -1;
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
        _wavesInfo = new WaveInfo[6];

        //wave1
        _wave1 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(5,2,0)),
            /* EnemyInfoFactory.CreateSpyderEnemy(new Vector3(2,3,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(1,5,0)), */
        };
        RegisterWaveInfo(_wave1,1);        

        //wave2
        _wave2 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(1,6,0)),
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(2,7,0)),
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(5,4,0)),
        };
        RegisterWaveInfo(_wave2,2);

        //wave3
        _wave3 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateBatEnemy(new Vector3(6,9,0),new Vector3(6,5,0)),
            EnemyInfoFactory.CreateBatEnemy(new Vector3(4,4,0),new Vector3(3,2,0)),
        };
        RegisterWaveInfo(_wave3,3);

        //wave4
        _wave4 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(8,7,0)),
            EnemyInfoFactory.CreateBatEnemy(new Vector3(9,8,0),new Vector3(9,4,0)),
        };
        RegisterWaveInfo(_wave4,4);

        //wave5
        _wave5 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(8,3,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(8,4,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(7,4,0)),
            EnemyInfoFactory.CreateBatEnemy(new Vector3(8,5,0),new Vector3(8,1,0)),
        };
        RegisterWaveInfo(_wave5,5);

        //wave6
        _wave6 = new EnemyInfo[]
        {
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(7,5,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(7,6,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(2,4,0)),
            EnemyInfoFactory.CreateSpyderEnemy(new Vector3(1,4,0)),
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(9,5,0)),
            EnemyInfoFactory.CreateFirerEnemy(new Vector3(9,8,0)),
            EnemyInfoFactory.CreateBatEnemy(new Vector3(0,4,0),new Vector3(7,7,0)),
        };
        RegisterWaveInfo(_wave6,6);
    }


    public void ConvertPositions(Func<int, int, Vector3> callback)
    {
        ConvertWavePositions(_wave1, callback);
        ConvertWavePositions(_wave2, callback);
        ConvertWavePositions(_wave3, callback);
        ConvertWavePositions(_wave4, callback);
        ConvertWavePositions(_wave5, callback);
        ConvertWavePositions(_wave6, callback);
    }

    public void SetHeroTransform(Transform heroTransform)
    {
        _enemyHeroTarget.SetCurrentTransform(heroTransform);
    }

    public void PrepareEnemies()
    {
        InstantiateEnemies(_wave1);
        InstantiateEnemies(_wave2);
        InstantiateEnemies(_wave3);
        InstantiateEnemies(_wave4);
        InstantiateEnemies(_wave5);
        InstantiateEnemies(_wave6);
    }

    public void RestartWave()
    {   
        if(_currentWave == 1)
        {
            ReinstantiateEnemies(_wave1);
            ResetWaveInfo(1);
            
        }
        else if(_currentWave == 2)
        {
            ReinstantiateEnemies(_wave2);
            ResetWaveInfo(2);
        }
        else if(_currentWave == 3)
        {
            ReinstantiateEnemies(_wave3);
            ResetWaveInfo(3);
        }
        else if(_currentWave == 4)
        {
            ReinstantiateEnemies(_wave4);
            ResetWaveInfo(4);
        }
        else if(_currentWave == 5)
        {
            ReinstantiateEnemies(_wave5);
            ResetWaveInfo(5);
        }
        else if(_currentWave == 6)
        {
            ReinstantiateEnemies(_wave6);
            ResetWaveInfo(6);
        }
            

    }

    public void RunWave(int idWave)
    {
        if (idWave == 1)
        {
            foreach(EnemyInfo eInfo in _wave1)
            {
                eInfo.Go.SetActive(true);
            }
        }else if (idWave == 2)
        {
            foreach(EnemyInfo eInfo in _wave2)
            {
                eInfo.Go.SetActive(true);
            }
            
        }
        _currentWave = idWave;
    }

/*     public void StopCurrentWave()
    {
        if (_currentWave == 1)
        {
            foreach(EnemyInfo eInfo in wave1)
            {
                eInfo.Go.SetActive(false);
                eInfo.Go.transform.position = eInfo.BornPoint;
            }
        }
    } */


    public void DebugWaves()
    {
        DebugWave(_wave1);
        /* DebugWave(wave2);
        DebugWave(wave3);
        DebugWave(wave4);
        DebugWave(wave5);
        DebugWave(wave6); */
    }

    #endregion
    #region Private Methods
    private void RegisterWaveInfo(EnemyInfo[] wave, int id)
    {
        _wavesInfo[id-1] = new WaveInfo()
        {
            Id=id,
            nDeads = 0,
            nEnemies= wave.Length,
            allDead = false, 
        };
    }
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
                    g = _enemyManager.CreateSpyderEnemy(wave[i].BornPoint, _enemyHeroTarget,_enemiesParent);
                    g.GetComponent<EB2FSM>().SetId(i);
                    break;
                case 1:
                    g = _enemyManager.CreateFirerEnemy(wave[i].BornPoint,_enemiesParent);
                    g.GetComponent<EB3FSM>().SetId(i);
                    break;
                case 2:
                    g = _enemyManager.CreateBatEnemy(wave[i].BornPoint, new Vector3[]{wave[i].BornPoint, wave[i].PatrolPoint},_enemiesParent);
                    g.GetComponent<EnemyB1FSM>().SetId(i);
                    break;
            }
            if (g != null)
            {
                wave[i].Id = i;
                g.GetComponent<IntNotifier>().IntNotifyAction += HandleDieIntNotify;
                wave[i].Go = g;
                g.SetActive(false);
            }
        }
    }

    private void ReinstantiateEnemies(EnemyInfo[] wave)
    {
        for (int i = 0; i < wave.Length; i++)
        {
            GameObject newG = null;
            switch (wave[i].EnemyType)
            {
                case 0:
                    newG = _enemyManager.CreateSpyderEnemy(wave[i].BornPoint, _enemyHeroTarget,_enemiesParent);
                    break;
                case 1:
                    newG = _enemyManager.CreateFirerEnemy(wave[i].BornPoint,_enemiesParent);
                    break;
                case 2:
                    newG = _enemyManager.CreateBatEnemy(wave[i].BornPoint, new Vector3[]{wave[i].BornPoint, wave[i].PatrolPoint},_enemiesParent);
                    break;
            }
            if (newG != null)
            {
                //destroy the current
                wave[i].Go.GetComponent<IntNotifier>().IntNotifyAction -= HandleDieIntNotify;
                Destroy(wave[i].Go);
                //substitute
                wave[i].Go = newG;
                newG.GetComponent<IntNotifier>().IntNotifyAction += HandleDieIntNotify;
                newG.SetActive(false);
            }
        }
    }

    private void ResetWaveInfo(int idWave)
    {
        for(int i=0; i<_wavesInfo.Length; i++)
        {
            WaveInfo wInfo = _wavesInfo[i];

            if(wInfo.Id == idWave)
            {
                wInfo.allDead = false;
                wInfo.nDeads = 0;

                _wavesInfo[i] = wInfo;
            }
        }
    }

    private void HandleDieIntNotify(int enemyId)
    {
        Debug.Log("Enemy die " + enemyId);

        for (int i = 0; i < _wavesInfo.Length; i++)
        {
            if (_wavesInfo[i].Id == _currentWave)
            {
                RegisterEnemyDie(ref _wavesInfo[i], enemyId);
                break;
            }
        }
    }

    private void RegisterEnemyDie(ref WaveInfo  waveInfo, int enemyId)
    {
        Debug.Log("before waveInfo.nDeads "+ waveInfo.nDeads);
        waveInfo.nDeads++;
        Debug.Log("after waveInfo.nDeads "+ waveInfo.nDeads);

        waveInfo.allDead = waveInfo.nDeads == waveInfo.nEnemies;
        if (waveInfo.allDead)
        {
            Debug.Log("Invoking "+ waveInfo.Id);
            waveAllDeadAction?.Invoke(waveInfo.Id);
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