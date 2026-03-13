using UnityEngine;
public class Enviroment : MonoBehaviour
{
    #region Fields
    [SerializeField] private ObjectsFactory _objectsFactory;
    [SerializeField] private Transform _generatedTransform;
    #endregion

    #region Private Fields
    private Transform _boxesParent;
    private Transform _ammosParent;
    private Transform _spikesParent;
    private Transform _puzzlesParent;
    private Transform _checkpointsParent;
    #endregion

    #region Properties
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _boxesParent = _generatedTransform.Find("Boxes");
        _ammosParent = _generatedTransform.Find("Ammos");
        _spikesParent = _generatedTransform.Find("Spikes");
        _puzzlesParent= _generatedTransform.Find("Puzzles");
        _checkpointsParent= _generatedTransform.Find("Checkpoints");
    }
    #endregion

    #region Public Methods
    public void PlaceBox(Vector3 position)
    {
        GameObject box = _objectsFactory.GetBox2Empties(position,_boxesParent);
    }

    public void PlaceAmmo(Vector3 position, int id)
    {
        GameObject ammo = _objectsFactory.GetAmmoOnBox(position, id,_ammosParent);
    }

    public void PlaceSpike(Vector3 position, int id)
    {
        GameObject spike = _objectsFactory.GetSpikes(position, id,_spikesParent);
    }

    public GameObject PlacePuzzle(Vector3 position, int idPuzzle)
    {
        switch (idPuzzle)
        {
            case 1:
                return _objectsFactory.GetPuzzle1(position,idPuzzle,_puzzlesParent);
            case 2:
                return _objectsFactory.GetPuzzle2(position,idPuzzle,_puzzlesParent);
            case 3:
                return _objectsFactory.GetPuzzle3(position,idPuzzle,_puzzlesParent);
            case 4:
                return _objectsFactory.GetPuzzle4(position,idPuzzle,_puzzlesParent);
            case 5:
                return _objectsFactory.GetPuzzle5(position,idPuzzle,_puzzlesParent);
            case 6:
                return _objectsFactory.GetPuzzle6(position,idPuzzle,_puzzlesParent);
        }

        return null;
    }

    public GameObject PlaceCheckpoint(Vector3 position)
    {
        return _objectsFactory.GetCheckpoint(position,_checkpointsParent);
    }
    #endregion

    #region Private Methods
    #endregion
}

