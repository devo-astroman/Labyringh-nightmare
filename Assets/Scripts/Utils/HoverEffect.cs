using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    #region Fields
    [SerializeField] float scaleFactor;
    #endregion

    #region Public methods
    public void OnHoverEnterEffect(GameObject go)
    {
        go.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }

    public void OnHoverExitEffect(GameObject go)
    {
        go.transform.localScale = Vector3.one;
    }
    #endregion
}