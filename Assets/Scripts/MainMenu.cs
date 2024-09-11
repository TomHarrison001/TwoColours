using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;
    private GameController controller;
    private Slider musicSlider, sfxSlider;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        controller = FindObjectOfType<GameController>();
        audioManager.sounds[0].source.volume = controller.MusicVolume;
        audioManager.sounds[1].source.volume = controller.SfxVolume;
        audioManager.sounds[2].source.volume = controller.SfxVolume;
        audioManager.sounds[3].source.volume = controller.SfxVolume;
        audioManager.Play("song");
        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.gameObject.name == "MusicVolume")
                musicSlider = s;
            else
                sfxSlider = s;
        }
        SetSettings();
    }

    private void SetSettings()
    {
        musicSlider.value = controller.MusicVolume;
        sfxSlider.value = controller.SfxVolume;
        if (controller.Deaths == 0 && controller.Level == 1)
            GameObject.Find("Continue").SetActive(false);
    }

    public void VolumeChange(Slider s)
    {
        if (s.gameObject.name == "MusicVolume")
        {
            controller.MusicVolume = s.value;
            audioManager.sounds[0].source.volume = controller.MusicVolume;
        }
        else if (s.gameObject.name == "SfxVolume")
        {
            controller.SfxVolume = s.value;
            audioManager.sounds[1].source.volume = controller.SfxVolume;
            audioManager.sounds[2].source.volume = controller.SfxVolume;
            audioManager.sounds[3].source.volume = controller.SfxVolume;
        }
    }

    public void PlayButtonAudio()
    {
        audioManager.Play("select");
    }

    public void NewGame()
    {
        controller.NewGame();
    }

    public void LoadLevel()
    {
        controller.LoadLevel(controller.Level);
    }
}
