using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsFactory : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _boxAllFacesPrefab;
    [SerializeField] private GameObject _box2EmptiesPrefab;
    [SerializeField] private GameObject _ammoOnBoxPrefab;
    [SerializeField] private GameObject _spikesPrefab;
    [SerializeField] private GameObject _puzzle1Prefab;
     
     private List<int> _allAmmoBoxIdsCreated = new List<int>();
     private List<int> _allSpikesIdsCreated = new List<int>();
     private List<int> _allPuzzlesIdsCreated = new List<int>();

    

    public GameObject GetBox2Empties(Vector3 position)
    {
        GameObject box = Instantiate(_box2EmptiesPrefab,position,Quaternion.identity);
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

    // Use the same bit values you used when building _doors
    private const int DIR_N = 1;
    private const int DIR_E = 2;
    private const int DIR_S = 4;
    private const int DIR_W = 8;

    public GameObject GetPuzzle1(Vector3 position, int id, int mask)
    {
        if (_allPuzzlesIdsCreated.Contains(id))
            return null;

        // Pick a CLOSED side (empty wall) from the mask
        int closedSideBit = PickRandomClosedSide(mask);

        // Compute Y rotation so the puzzle faces into the room from that wall
        Quaternion rot = RotationFacingIntoRoomFromWall(closedSideBit);

        GameObject puzzle = Instantiate(_puzzle1Prefab, position, rot);
        puzzle.name = "Puzzle_" + id;

        Debug.Log($"MASK {mask} | closedSideBit {closedSideBit}");

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

}
