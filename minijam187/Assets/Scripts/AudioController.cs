using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip music;
    [SerializeField] private Slider musicSlider;

    public void Awake()
    {
        musicSlider.value = musicPlayer.volume;
        musicSlider.onValueChanged.AddListener(UpdateVolume);
    }

    public void Start()
    {
        musicPlayer.clip = music;
        musicPlayer.loop = true;
        musicPlayer.Play();
    }

    public void UpdateVolume(float volume)
    {
        musicPlayer.volume = volume;
    }
}
