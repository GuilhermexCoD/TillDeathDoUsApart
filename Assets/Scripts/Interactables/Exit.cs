using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Exit : Actor
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colision!");
        if (collision.gameObject.name == "Player") {
            ScenaryManager.current.LoadNextLevel();
        }
    }
}
