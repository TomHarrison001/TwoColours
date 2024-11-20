using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    private GameController controller;
    private AudioManager audioManager;
    private GameObject pauseMenu;
    private float speed = 0.1f, jumpForce = 10f;
    private int newJump = 20;
    private bool jumping = false, paused = false;
    public bool Paused { set { this.paused = value; } }

    private void Start()
    {
        controller = FindObjectOfType<GameController>();
        audioManager = FindObjectOfType<AudioManager>();
        pauseMenu = GameObject.Find("PauseMenu");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Obstacle")
        {
            SpawnParticles();
            controller.IncrementDeaths();
            jumping = false;
            newJump = 50;
            transform.position = new Vector3(-11f, -4.25f);
            audioManager.Play("death");
        }
        if (other.gameObject.name == "End")
        {
            controller.LevelComplete();
        }
    }

    private void FixedUpdate()
    {
        if (paused) return;
        // esc to pause
        if (Input.GetKey(KeyCode.Escape))
        {
            pauseMenu.GetComponent<PauseMenu>().PlayButtonAudio();
            pauseMenu.GetComponent<PauseMenu>().PauseGame();
            pauseMenu.GetComponent<ActivateMenu>().EnableMenu();
            return;
        }
        // move
        transform.position += transform.right * speed;
        // jump
        if (!jumping && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)))
        {
            jumping = true;
            newJump = 0;
            audioManager.Play("jump");
        }
        if (jumping)
        {
            Jump();
        }
    }

    public void MobileJump()
    {
        if (!jumping)
        {
            jumping = true;
            newJump = 0;
            audioManager.Play("jump");
        }
    }

    private void Jump()
    {
        if (newJump < 10) transform.position += transform.up * jumpForce / (20 * (newJump + 1));
        else transform.position -= transform.up * jumpForce / (20 * (20 - newJump));
        newJump++;
        if (newJump == 20) jumping = false;
    }

    private void SpawnParticles()
    {
        GameObject g = Instantiate(particles, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        g.transform.localScale = new Vector2(0.02f, 0.02f);
        StartCoroutine(DestroyParticles(g));
    }

    private IEnumerator DestroyParticles(GameObject g)
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(g);
    }
}
