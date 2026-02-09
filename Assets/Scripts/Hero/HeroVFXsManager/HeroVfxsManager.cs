using UnityEngine;

public class HeroVfxsManager : MonoBehaviour
{
    [SerializeField] private GameObject _gunFireVFXs;
    [SerializeField] private Transform _originBullet;
    [SerializeField] private float _vfxScale = 0.05f;

    public void ShowGunFireVfxs()
    {
        if (_gunFireVFXs == null || _originBullet == null)
            return;

        GameObject vfx = Instantiate(
            _gunFireVFXs,
            _originBullet.position,
            _originBullet.rotation,
            _originBullet   // parent
        );

        vfx.transform.localScale = Vector3.one * _vfxScale;
    }
}
