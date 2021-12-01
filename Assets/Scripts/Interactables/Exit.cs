using UnityEngine;

public class Exit : Actor
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ScenaryManager.current.LoadNextLevel();
            Destroy(this.gameObject);
        }
    }
}
