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
    private string pickUpActionName = "PickUp";
    [SerializeField]
    private string attackActionName = "Attack";

    [SerializeField]
    private FlipAccordingRotation flipWeapon;
    [SerializeField]
    private Transform weaponTransform;
    [SerializeField]
    private Transform offSetTransform;

    [SerializeField]
    private IInteractable interactableObject;
    public List<IInteractable> inventory = new List<IInteractable>();

    private Weapon equipedWeapon;

    public Transform handRTransform;
    public Transform handLTransform;

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
        if (Input.GetButtonDown(pickUpActionName) && interactableObject != null)
        {
            var pickedUp = interactableObject.PickUp(this);

            if (pickedUp.GetType().BaseType == typeof(Weapon))
            {
                var weapon = ((Weapon)pickedUp);
                weapon.transform.SetParent(offSetTransform);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
                equipedWeapon = weapon;

                handRTransform.localPosition = equipedWeapon.GetData<RangedWeaponData>().handR_Transform;
                handRTransform.localEulerAngles = equipedWeapon.GetData<RangedWeaponData>().handR_Rotation;

                handLTransform.localPosition = equipedWeapon.GetData<RangedWeaponData>().handL_Transform;
                handLTransform.localEulerAngles = equipedWeapon.GetData<RangedWeaponData>().handL_Rotation;

            }

            Debug.Log($"Picked up an item : {pickedUp}");
            inventory.Add(pickedUp);
        }

        if (Input.GetButtonDown(attackActionName) && equipedWeapon != null)
        {
            equipedWeapon.Attack();
        }

        if (Camera.current != null && equipedWeapon != null)
        {
            Vector3 mousePosition = Util.GetMouseWorldPosition();

            Vector3 aimDirection = (mousePosition - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            weaponTransform.eulerAngles = new Vector3(0, 0, angle);
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
