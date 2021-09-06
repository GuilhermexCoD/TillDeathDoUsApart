using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [InspectorName("Dash")]
    public float DashCameraShakeMultiplier = 2;
    public float DashCameraShakeTime = 0.2f;
    private DashComponent dash;

    [SerializeField]
    private IInteractable interactableObject;
    public List<IInteractable> inventory = new List<IInteractable>();

    // Start is called before the first frame update
    void Awake()
    {
        if (!dash)
        {
            dash = GetComponent<DashComponent>();
        }

        dash.Subscribe(OnPlayerDashed);
    }

    public void OnPlayerDashed(Vector2 direction)
    {
        Debug.Log("Player Dashed");
        CinemachineShake.Current.ShakeCamera(direction.magnitude * DashCameraShakeMultiplier, 0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PickUp") && interactableObject != null)
        {
            var pickedUp = interactableObject.PickUp(this);
            Debug.Log($"Picked up an item : {pickedUp}");
            inventory.Add(pickedUp);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactableObject = collision.gameObject.GetComponent<IInteractable>();

        Debug.Log($"Interactable = {interactableObject.GetInfo()}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactableObject = null;
    }
}
