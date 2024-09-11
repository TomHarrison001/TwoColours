using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private AudioManager audioManager;
    private GameController controller;
    private Player player;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        controller = FindObjectOfType<GameController>();
        player = FindObjectOfType<Player>();
    }

    public void PlayButtonAudio()
    {
        audioManager.Play("select");
    }

    public void PauseGame()
    {
        player.Paused = true;
    }

    public void ResumeGame()
    {
        StartCoroutine(Resume());
    }

    private IEnumerator Resume()
    {
        yield return new WaitForSeconds(0.2f);
        player.Paused = false;
    }

    public void LoadMainMenu()
    {
        controller.LoadLevel(0);
    }
}
