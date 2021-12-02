using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveToPlayer : MonoBehaviour
{
    IInteractable item;

    public float waitSeconds = 0.5f;
    
    // Start is called before the first frame update
    void Awake()
    {
        item = GetComponent<IInteractable>();

        Level.current.onGenerated += OnLevelGenerated;
        
        if (Level.current.IsGenerated())
        {
            OnLevelGenerated(this, null);
        }
    }

    private void OnLevelGenerated(object sender, System.EventArgs e)
    {
        StartCoroutine(GiveWeapon());
    }

    private IEnumerator GiveWeapon()
    {
        yield return new WaitForSeconds(waitSeconds);
        GameEventsHandler.current.playerGo.GetComponent<PlayerController>().PickUpItem(item);
        Level.current.onGenerated -= OnLevelGenerated;
        Destroy(this);
    }

}
