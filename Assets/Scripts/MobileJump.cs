using UnityEngine;

public class MobileJump : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Jump()
    {
        player.MobileJump();
    }
}
