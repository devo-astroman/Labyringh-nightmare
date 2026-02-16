using UnityEngine;

public class RoomPositions : MonoBehaviour
{
    #region Fields
	[SerializeField] private RoomPositionsLayer _roomPositionsLayerUp;
    [SerializeField] private RoomPositionsLayer _roomPositionsLayerMid;
    [SerializeField] private RoomPositionsLayer _roomPositionsLayerBelow;
    #endregion

    #region Public properties
    public readonly int LAYER_UP = 0;
    public readonly int LAYER_MID = 1;
    public readonly int LAYER_BELOW = 2;
    #endregion

    #region Private properties
    private void HideLayerCubePositions(RoomPositionsLayer layer)
    {
        layer.HideAllCubePosition();
    }
    #endregion


    #region Public Methods 
    public void HideAllCubePositions()
    {
        HideLayerCubePositions(_roomPositionsLayerUp);
        HideLayerCubePositions(_roomPositionsLayerMid);
        HideLayerCubePositions(_roomPositionsLayerBelow);
    }

    public void ShowCubePosition(int idLayer, int idCubePosition)
    {
        if(idLayer == LAYER_UP)
        {
            _roomPositionsLayerUp.DisplayCubePosition(idCubePosition);
        }else if(idLayer == LAYER_MID)
        {
            _roomPositionsLayerMid.DisplayCubePosition(idCubePosition);
        }

        _roomPositionsLayerBelow.DisplayCubePosition(idCubePosition);
    }

    public Vector3 GetPosition(int idLayer, int idCubePosition)
    {
        if(idLayer == LAYER_UP)
        {
            return _roomPositionsLayerUp.GetPosition(idCubePosition);
        }else if(idLayer == LAYER_MID)
        {
            return _roomPositionsLayerMid.GetPosition(idCubePosition);
        }

        return _roomPositionsLayerBelow.GetPosition(idCubePosition);
    }
    #endregion


}

