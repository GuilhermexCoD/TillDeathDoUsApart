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
    private Vector2 _lastInputDelta;
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
            PickUpItem();
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
        //bool bIsAttackPressed = ;
        bool isUsingMouse = IsUsingMouse();
        Vector2 aimDirection = GetAimDirection();

        bool bIsAttackPressed = isUsingMouse ? _input.Player.Attack.triggered : aimDirection.magnitude == 1;
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

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            weaponTransform.eulerAngles = new Vector3(0, 0, angle);
        }

        //bool bIsOpenMenuPressed = Input.GetKeyDown(KeyCode.Escape);

    }

    public void PickUpItem(IInteractable item)
    {
        interactableObject = item;

        PickUpItem();
    }

    private void PickUpItem()
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

    private Vector2 GetAimDirection()
    {
        Vector2 aimDirection = Vector2.zero;
        try
        {
            aimDirection = _input.Player.Aim.ReadValue<Vector2>();

            if (IsUsingMouse())
            {
                Vector3 mousePosition = Util.GetMouseWorldPosition(aimDirection);
                aimDirection = (mousePosition - transform.position).normalized;
            }
            else
            {
                aimDirection = _input.Player.Aim.ReadValue<Vector2>();
            }
        }
        catch (System.Exception ex)
        {

            throw ex;
        }


        return aimDirection;
    }

    private bool IsUsingMouse()
    {
        //return false;
        return _input.Player.Aim.activeControl.device.GetType() == typeof(UnityEngine.InputSystem.Mouse);
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
            var coin = collision.gameObject.GetComponent<Coin>();
            if (coin != null)
            {
                coin.PickUp(this);
                Destroy(coin.gameObject);
            }
            else
            {
                var col = collision.gameObject.GetComponent<IInteractable>();
                //if (col != null)
                interactableObject = col;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactableObject = null;
    }

    private void OnEnable()
    {
        _input?.Enable();
    }

    private void OnDisable()
    {
        _input?.Disable();
    }
}
