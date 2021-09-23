using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [InspectorName("Dash")]
    public float dashCameraShakeMultiplier = 2;
    public float dashCameraShakeTime = 0.2f;
    private DashComponent dash;

    public float shootCameraShakeTime = 0.1f;
    public float shootShakeMultiplier = 2;

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

    public void OnPlayerDashed(object sender, OnDashEventArgs args)
    {
        CinemachineShake.Current.ShakeCamera(args.direction.magnitude * dashCameraShakeMultiplier, dashCameraShakeTime);
    }
    private void OnPlayerShooted(object sender, OnShootEventArgs e)
    {
        CinemachineShake.Current.ShakeCamera(e.damage * shootShakeMultiplier, shootCameraShakeTime);
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

                if (weapon.GetType() == typeof(RangedWeapon))
                {
                    (weapon as RangedWeapon).onShoot += OnPlayerShooted;
                }
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0,LoadSceneMode.Single);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null)
        {
            interactableObject = collision.gameObject.GetComponent<IInteractable>();

            if (interactableObject != null)
            {
                Debug.Log($"Interactable = {interactableObject.GetInfo()}");
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactableObject = null;
    }
}
