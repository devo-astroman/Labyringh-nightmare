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
    #endregion

    #region Properties
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _boxesParent = _generatedTransform.Find("Boxes");
        _ammosParent = _generatedTransform.Find("Ammos");
        _spikesParent = _generatedTransform.Find("Spikes");
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
    #endregion

    #region Private Methods
    #endregion
}

