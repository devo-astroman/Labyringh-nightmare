using System.Collections.Generic;
using UnityEngine;

public class ObjectsFactory : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _boxAllFacesPrefab;
    [SerializeField] private GameObject _box2EmptiesPrefab;
    [SerializeField] private GameObject _ammoOnBoxPrefab;
    [SerializeField] private GameObject _spikesPrefab;    

    [SerializeField] private GameObject _p1SpyderPuzzlePrefab;
    [SerializeField] private GameObject _p2FirerPuzzlePrefab;
    [SerializeField] private GameObject _p3BatPuzzlePrefab;
    [SerializeField] private GameObject _p4TrianglePuzzlePrefab;
    [SerializeField] private GameObject _p5CompassPuzzlePrefab;
    [SerializeField] private GameObject _p6RowbinPuzzlePrefab;

    [SerializeField] private GameObject _checkpointPrefab;


    
    
     
     private List<int> _allAmmoBoxIdsCreated = new List<int>();
     private List<int> _allSpikesIdsCreated = new List<int>();
     private List<int> _allPuzzlesIdsCreated = new List<int>();

    

    public GameObject GetBox2Empties(Vector3 position)
    {
        GameObject box = Instantiate(_box2EmptiesPrefab,position,Quaternion.identity);
        return box;
    }

    public GameObject GetBox2Empties(Vector3 position, Transform parent)
    {
        GameObject box = Instantiate(_box2EmptiesPrefab,position,Quaternion.identity, parent);
        return box;
    }

    public GameObject GetAmmoOnBox(Vector3 position, int id)
    {
        if (!_allAmmoBoxIdsCreated.Contains(id))
        {
            GameObject ammoOnBox = Instantiate(_ammoOnBoxPrefab,position,Quaternion.identity);
            ammoOnBox.GetComponent<AmmoOnBox>().SetId(id);

            _allAmmoBoxIdsCreated.Add(id);
            return ammoOnBox;

        }
        
        return null;
    }

    public GameObject GetAmmoOnBox(Vector3 position, int id, Transform parent)
    {
        if (!_allAmmoBoxIdsCreated.Contains(id))
        {
            GameObject ammoOnBox = Instantiate(_ammoOnBoxPrefab,position,Quaternion.identity,parent);
            ammoOnBox.GetComponent<AmmoOnBox>().SetId(id);

            _allAmmoBoxIdsCreated.Add(id);
            return ammoOnBox;

        }
        
        return null;
    }

    public GameObject GetSpikes(Vector3 position, int id)
    {
        if (!_allSpikesIdsCreated.Contains(id))
        {
            GameObject spikes = Instantiate(_spikesPrefab,position,Quaternion.identity);
            spikes.GetComponent<Spikes>().SetId(id);

            _allSpikesIdsCreated.Add(id);
            return spikes;

        }
        
        return null;
    }

    public GameObject GetSpikes(Vector3 position, int id, Transform parent)
    {
        if (!_allSpikesIdsCreated.Contains(id))
        {
            GameObject spikes = Instantiate(_spikesPrefab,position,Quaternion.identity,parent);
            spikes.GetComponent<Spikes>().SetId(id);

            _allSpikesIdsCreated.Add(id);
            return spikes;
        }

        return null;
    }

    public GameObject GetCheckpoint(Vector3 position, int id)
    {
        GameObject checkpoint = Instantiate(_checkpointPrefab,position,Quaternion.identity);
        return checkpoint;
    }

    public GameObject GetCheckpoint(Vector3 position, Transform parent)
    {
        GameObject checkpoint = Instantiate(_checkpointPrefab,position,Quaternion.identity, parent);
        return checkpoint;
    }
    

    // Use the same bit values you used when building _doors
    private const int DIR_N = 1;
    private const int DIR_E = 2;
    private const int DIR_S = 4;
    private const int DIR_W = 8;

    public GameObject GetPuzzle1(Vector3 position, int id, Transform parent)
    {
        if (_allPuzzlesIdsCreated.Contains(id))
            return null;
        
        Quaternion rot = RotationFacingSouth();

        GameObject puzzle = Instantiate(_p1SpyderPuzzlePrefab, position, rot, parent);
        puzzle.name = "Puzzle_" + id;

        _allPuzzlesIdsCreated.Add(id);
        return puzzle;
    }

    public GameObject GetPuzzle2(Vector3 position, int id, Transform parent)
    {
        if (_allPuzzlesIdsCreated.Contains(id))
            return null;
        
        Quaternion rot = RotationFacingEast();

        GameObject puzzle = Instantiate(_p2FirerPuzzlePrefab, position, rot, parent);
        puzzle.name = "Puzzle_" + id;

        _allPuzzlesIdsCreated.Add(id);
        return puzzle;
    }

    public GameObject GetPuzzle3(Vector3 position, int id, Transform parent)
    {
        if (_allPuzzlesIdsCreated.Contains(id))
            return null;
       
        Quaternion rot = RotationFacingSouth();

        GameObject puzzle = Instantiate(_p3BatPuzzlePrefab, position, rot, parent);
        puzzle.name = "Puzzle_" + id;

        _allPuzzlesIdsCreated.Add(id);
        return puzzle;
    }

    public GameObject GetPuzzle4(Vector3 position, int id, Transform parent)
    {
        if (_allPuzzlesIdsCreated.Contains(id))
            return null;

        Quaternion rot = RotationFacingSouth();

        GameObject puzzle = Instantiate(_p4TrianglePuzzlePrefab, position, rot, parent);
        puzzle.name = "Puzzle_" + id;

        _allPuzzlesIdsCreated.Add(id);
        return puzzle;
    }

    public GameObject GetPuzzle5(Vector3 position, int id, Transform parent)
    {
        if (_allPuzzlesIdsCreated.Contains(id))
            return null;

        Quaternion rot = RotationFacingNorth();

        GameObject puzzle = Instantiate(_p5CompassPuzzlePrefab, position, rot, parent);
        puzzle.name = "Puzzle_" + id;

        _allPuzzlesIdsCreated.Add(id);
        return puzzle;
    }

    public GameObject GetPuzzle6(Vector3 position, int id, Transform parent)
    {
        if (_allPuzzlesIdsCreated.Contains(id))
            return null;

        Quaternion rot = RotationFacingEast();

        GameObject puzzle = Instantiate(_p6RowbinPuzzlePrefab, position, rot, parent);
        puzzle.name = "Puzzle_" + id;

        _allPuzzlesIdsCreated.Add(id);
        return puzzle;
    }

    private int PickRandomClosedSide(int mask)
    {
        // Collect all CLOSED sides
        // (mask bit NOT set => closed)
        List<int> open = new List<int>(4);

        if ((mask & DIR_N) != 0) open.Add(DIR_N);
        if ((mask & DIR_E) != 0) open.Add(DIR_E);
        if ((mask & DIR_S) != 0) open.Add(DIR_S);
        if ((mask & DIR_W) != 0) open.Add(DIR_W);

        // If all sides are open (mask==15), fallback: pick any side
        if (open.Count == 0)
            return DIR_N; // or pick random among all 4, your choice

        return open[UnityEngine.Random.Range(0, open.Count)];
    }

    private int PickClosedSides(bool N, bool S, bool E, bool W)
    {
        int mask = 0;

        if (N) mask |= DIR_N;
        if (E) mask |= DIR_E;
        if (S) mask |= DIR_S;
        if (W) mask |= DIR_W;

        return mask;
    }

    private Quaternion RotationFacingIntoRoomFromWall(int wallBit)
    {
        // Rotation so the object faces into the room when placed on that wall.
        // South wall => face North (0)
        // West wall  => face East  (90)
        // North wall => face South (180)
        // East wall  => face West  (270)
        float y;

        if (wallBit == DIR_S) y = 0f;
        else if (wallBit == DIR_W) y = 90f;
        else if (wallBit == DIR_N) y = 180f;
        else if (wallBit == DIR_E) y = 270f;
        else y = 0f;

        return Quaternion.Euler(0f, y, 0f);
    }

    private Quaternion RotationFacingNorth()
    {
        return Quaternion.Euler(0,0,0);
    }

    private Quaternion RotationFacingEast()
    {
        return Quaternion.Euler(0,90,0);
    }

    private Quaternion RotationFacingSouth()
    {
        return Quaternion.Euler(0,180,0);
    }

    private Quaternion RotationFacingWest()
    {
        return Quaternion.Euler(0,270,0);
    }

}
