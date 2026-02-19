using UnityEngine;
using TMPro;

public class Hud : MonoBehaviour
{
    [SerializeField] private GameObject _bigCrosshair;
    [SerializeField] private GameObject _medCrosshair;
    [SerializeField] private GameObject _smallCrosshair;
    [SerializeField] private GameObject _hurtScreen;
    [SerializeField] private TextMeshProUGUI _ammoText;

    public void SetCurrentSpeed(float speed){

        if (speed  == 0)
            SetSmallCrosshair();

        else if(speed < 2.5)
            SetMedCrosshair();

        else
            SetBigCrosshair();
    }

    public void SetBigCrosshair()
    {
        HideCrosshair();
        _bigCrosshair.SetActive(true);
    }

    public void SetMedCrosshair()
    {
        HideCrosshair();
        _medCrosshair.SetActive(true);
    }

    public void SetSmallCrosshair()
    {
        HideCrosshair();
        _smallCrosshair.SetActive(true);
    }

    public void HideCrosshair()
    {
        _bigCrosshair.SetActive(false);
        _medCrosshair.SetActive(false);
        _smallCrosshair.SetActive(false);
    }

    public void SetAmmo(int ammo)
    {
        _ammoText.text = ammo + "";
    }

    public void ShowAmmo()
    {
        _ammoText.gameObject.SetActive(true);
    }
    public void HideAmmo()
    {
        _ammoText.gameObject.SetActive(false);
    }

    public void ShowHurtScreen()
    {
        _hurtScreen.SetActive(true);
    }

    public void HideHurtScreen()
    {
        _hurtScreen.SetActive(false);
    }

}
