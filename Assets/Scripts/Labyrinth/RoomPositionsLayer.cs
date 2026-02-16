using UnityEngine;

public class RoomPositionsLayer : MonoBehaviour
{
    #region Fields
	[SerializeField] private GameObject[] _cubePositionsGO;    
    #endregion

    #region Private properties
    #endregion


    #region Public Methods
    public void HideAllCubePosition()
    {
        for(int i = 0; i< _cubePositionsGO.Length; i++)
        {
            _cubePositionsGO[i].GetComponent<MeshRenderer>().enabled = false;    
        }
    }
    public void DisplayCubePosition(int iCube)
    {
        _cubePositionsGO[iCube].GetComponent<MeshRenderer>().enabled = true;
    }
    public void HideCubePosition(int iCube)
    {
        _cubePositionsGO[iCube].GetComponent<MeshRenderer>().enabled = false;
    }

    public Vector3 GetPosition(int iCube)
    {
        return _cubePositionsGO[iCube].transform.position;
    }

    #endregion


}
