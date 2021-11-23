using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [Header("Dash")]
    public float dashCameraShakeMultiplier = 2;
    public float dashCameraShakeTime = 0.2f;
    private DashComponent dash;

    [Header("Dash")]
    private Movement movement;

    [Header("Shot")]
    public float shootCameraShakeTime = 0.1f;
    public float shootShakeMultiplier = 2;


    [Header("Actions")]
    private PlayerControls _input;
    [SerializeField]
    private string pickUpActionName = "PickUp";
    [SerializeField]
    private string attackActionName = "Attack";
    [SerializeField]
    private string reloadActionName = "Reload";

    [Header("Weapon")]
    [SerializeField]
    private FlipAccordingRotation flipWeapon;
    [SerializeField]
    private Transform weaponTransform;
    [SerializeField]
    private Transform offSetTransform;

    [Header("Inventory")]
    [SerializeField]
    private IInteractable interactableObject;
    private InventoryManager inventoryManager = new InventoryManager();
    private int equipedItemIndex = -1;

    public float lastX;

    [Header("Hands")]
    [SerializeField]
    private Transform handRTransform;
    [SerializeField]
    private OverrideTransform handROverride;
    [SerializeField]
    private Transform handLTransform;
    [SerializeField]
    private OverrideTransform handLOverride;

    [Header("Animation")]
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        _input = new PlayerControls();

        if (!dash)
        {
            dash = GetComponent<DashComponent>();
        }

        dash.Subscribe(OnPlayerDashed);

        if (movement == null)
        {
            movement = GetComponent<Movement>();
        }

        movement.onSpeedChanged += OnSpeedChanged;
    }

    private void OnSpeedChanged(object sender, Vector2 e)
    {
        animator.SetFloat("Speed", e.magnitude);
    }

    private void OnPlayerDashed(object sender, OnDashEventArgs args)
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
        //bool bIsPickingUp = Input.GetButtonDown(pickUpActionName);
        bool bIsPickingUp = _input.Player.PickUp.triggered;
        if (bIsPickingUp && interactableObject != null)
        {
            var pickedUp = (Interactable)interactableObject.PickUp(this);

            var itemIndex = inventoryManager.AddItem(pickedUp);

            pickedUp.SetActive(false);
            pickedUp.transform.SetParent(offSetTransform);
            pickedUp.transform.localPosition = Vector3.zero;
            pickedUp.transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (!IsWeaponEquiped())
            {
                bool pickedUpIsWeapon = IsWeapon(itemIndex);

                if (pickedUpIsWeapon)
                {
                    EquipItem(itemIndex);
                }
            }
        }

        var xAxis = _input.Player.Move.ReadValue<Vector2>().x;
        //if (bIsPickingUp && Input.GetButtonDown("Horizontal"))
        if (bIsPickingUp && xAxis > 0)
        {
            //var xAxis = Input.GetAxisRaw("Horizontal");

            var desiredEquipIndex = Util.ModLoop(equipedItemIndex + (int)Mathf.Sign(xAxis), inventoryManager.GetItemsCount());
            Util.CreateWorldTextPopup($"Equip id:{equipedItemIndex}", this.transform.position, 20, Vector3.one * 0.2f, 2, 1);
            EquipItem(desiredEquipIndex);

        }

        //bool bIsAttackPressed = Input.GetButtonDown(attackActionName);
        bool bIsAttackPressed = _input.Player.Attack.triggered;
        if (bIsAttackPressed && IsWeaponEquiped())
        {
            GetWeapon()?.Attack();
        }

        //bool bIsReloadPressed = Input.GetButtonDown(reloadActionName);
        bool bIsReloadPressed = _input.Player.Reload.triggered;
        if (bIsReloadPressed && IsWeaponEquiped())
        {
            ((RangedWeapon)GetWeapon())?.Reload();
        }

        if (Camera.current != null && IsWeaponEquiped())
        {
            //Vector3 mousePosition = Util.GetMouseWorldPosition();

            //Vector3 aimDirection = (mousePosition - transform.position).normalized;
            Vector3 aimDirection = _input.Player.Aim.ReadValue<Vector2>().normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            weaponTransform.eulerAngles = new Vector3(0, 0, angle);
        }

        //bool bIsOpenMenuPressed = Input.GetKeyDown(KeyCode.Escape);
        bool bIsOpenMenuPressed = _input.Player.OpenMenu.triggered;
        if (bIsOpenMenuPressed)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    private bool IsWeaponEquiped()
    {
        return IsWeapon(equipedItemIndex);
    }

    private Weapon GetWeapon()
    {
        var weapon = inventoryManager.ElementAt(equipedItemIndex)?.CastTo<Weapon>();

        return weapon;
    }

    private bool IsWeapon(int index)
    {
        var inventoryItem = inventoryManager.ElementAt(index);

        if (inventoryItem != null)
        {
            return inventoryItem.interactable.GetType().BaseType == typeof(Weapon);
        }

        return false;
    }

    public InventoryManager GetInventory()
    {
        return inventoryManager;
    }

    private void EquipItem(int index)
    {
        if (equipedItemIndex != index)
        {
            var lastInventoryItem = inventoryManager.ElementAt(equipedItemIndex);

            if (lastInventoryItem != null)
                lastInventoryItem.interactable.SetActive(false);

            equipedItemIndex = index;

            var inventoryItem = inventoryManager.ElementAt(index);

            if (inventoryItem != null)
            {
                inventoryItem.interactable.SetActive(true);

                var interactable = (Interactable)inventoryItem.interactable;

                offSetTransform.localPosition = interactable.GetData<ItemData>().offSetPosition;

                UpdateHandPositionAndRotation(interactable.GetData<ItemData>(), 1);

                //TODO change logic for equipping weapon
                if (interactable.GetType().BaseType == typeof(Weapon))
                {
                    var weapon = ((Weapon)interactable);

                    if (weapon.GetType() == typeof(RangedWeapon))
                    {
                        (weapon as RangedWeapon).onShoot += OnPlayerShooted;
                    }
                }
            }
        }
    }

    private void UpdateHandPositionAndRotation(ItemData data, float weight)
    {
        handRTransform.localPosition = data.handR_Transform;
        handRTransform.localEulerAngles = data.handR_Rotation;
        handROverride.weight = weight;

        handLTransform.localPosition = data.handL_Transform;
        handLTransform.localEulerAngles = data.handL_Rotation;
        handLOverride.weight = weight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null)
        {
            interactableObject = collision.gameObject.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactableObject = null;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
