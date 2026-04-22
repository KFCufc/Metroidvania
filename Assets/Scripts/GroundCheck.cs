using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool OnGround;

    private string GROUND = "Ground";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND))
        {
            OnGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND))
        {
            OnGround = false;
        }
    }
}
