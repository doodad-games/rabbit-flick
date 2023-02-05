using MyLibrary;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Slider _volumeSlider;

    public void OnEnable()
    {
        _volumeSlider.onValueChanged.AddListener(UpdateVolume);
        _volumeSlider.value = VolumeController.GetUserVolume("Volume");
    }

    public void OnDisable() =>
        _volumeSlider.onValueChanged.RemoveListener(UpdateVolume);

    void UpdateVolume(float vol) =>
        VolumeController.SetUserVolume("Volume", vol);
}
