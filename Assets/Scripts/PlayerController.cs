#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    public class PlayerController : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] private Transform? hips;
        [SerializeField] private Transform? body;
        [SerializeField] private GameObject? hipsDirArrow;
        [SerializeField] private Camera? mainCamera;
        [SerializeField] private Rigidbody? rb;
        [SerializeField] private PlayerInput? playerInput;
        [SerializeField] private Animator? animator;
        [SerializeField] private MechController? mech;

        [Header("Settings")]
        [SerializeField] private Settings? settings;
        [SerializeField] private LayerMask aimMask;
        [SerializeField] private LayerMask deadenemyMask;
        [SerializeField] private Texture2D defaultCursor;
        [SerializeField] private Texture2D pickupCursor;
        [SerializeField] private Texture2D outofrangeCursor;


        public MechController? Mech { get => mech; set => mech = value; }

        private InputAction? startAction;
        private InputAction? selectAction;
        private InputAction? moveAction;
        private InputAction? mousePosAction;
        private InputAction? primaryFire;
        private InputAction? secondaryFire;
        private InputAction? changePrimaryAction;
        private InputAction? changeSecondaryAction;
        private InputAction? repairAction;
        private InputAction? overdriveAction;
        private InputAction? coolantAction;


        private RaycastHit[] raycastHits = new RaycastHit[1];

        void Awake()
        {
            playerInput = playerInput ?? GetComponent<PlayerInput>();
            rb = rb ?? GetComponent<Rigidbody>();
            animator = animator ?? GetComponent<Animator>();
            mech = mech ?? GetComponent<MechController>();

            startAction = playerInput.actions["Start"];
            selectAction = playerInput.actions["Select"];
            moveAction = playerInput.actions["Move"];
            mousePosAction = playerInput.actions["Mouse Position"];
            primaryFire = playerInput.actions["Primary Fire"];
            secondaryFire = playerInput.actions["Secondary Fire"];
            repairAction = playerInput.actions["Repair"];
            overdriveAction = playerInput.actions["Overdrive"];
            coolantAction = playerInput.actions["Coolant Dump"];

            changePrimaryAction = playerInput.actions["Change Primary Weapon"];
            changeSecondaryAction = playerInput.actions["Change Secondary Weapon"];

            primaryFire.performed += OnPrimaryFire;
            secondaryFire.performed += OnSecondaryFire;
            startAction.performed += OnStart;
            selectAction.performed += OnSelect;
            changePrimaryAction.performed += OnChangePrimaryWeapon;
            changeSecondaryAction.performed += OnChangeSecondaryWeapon;
            repairAction.performed += OnRepairAction;
            overdriveAction.performed += OnOverdriveAction;
            coolantAction.performed += OnCoolantDumpAction;

        }

        public void Reset()
        {
            if (settings == null)
            {
                Debug.LogError("settings is null", this);
                return;
            }
            if (mech == null)
            {
                Debug.LogError("weapons is null", this);
                return;
            }
            NewLevel();
            mech.Reset();
            this.transform.position = settings.startPosition;
            mech.selectedPrimaryWeapon = settings.startingPrimary;
            mech.selectedSecondaryWeapon = settings.startingSecondary;
            mech.linkArms = false;
            mech.linkShoulders = false;
            mech.UpdateModels();
            mech.SetItem(MechPosition.RightArm, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.claw));
            mech.SetItem(MechPosition.LeftArm, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.cannon));
            mech.SetItem(MechPosition.LeftShoulder, null);
            mech.SetItem(MechPosition.Armour, null);
            mech.SetItem(MechPosition.Motor, null);
            mech.SetItem(MechPosition.Radar, null);
            mech.SetItem(MechPosition.InvSlot0, null);
            mech.SetItem(MechPosition.InvSlot1, null);
            mech.SetItem(MechPosition.InvSlot2, null);
            mech.SetItem(MechPosition.InvSlot3, null);
            mech.SetItem(MechPosition.InvSlot4, null);
            mech.SetItem(MechPosition.InvSlot5, null);
            mech.SetItem(MechPosition.InvSlot6, null);
            mech.SetItem(MechPosition.InvSlot7, null);

            //mech.SetItem(MechPosition.Motor, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.motor));
        }

        public void Die()
        {
            ResetAnimation();
        }

        private void ResetAnimation()
        {
            moveAction?.Reset();
            Animate(Vector2.zero);
        }

        public void NewLevel()
        {
            if (settings == null)
            {
                Debug.LogError("settings is null", this);
                return;
            }
            Cursor.SetCursor(defaultCursor, new Vector2(50, 50), CursorMode.Auto);
            ResetAnimation();
            this.transform.position = settings.startPosition;
            this.transform.rotation = Quaternion.Euler(0, 45, 0);
            body.rotation = Quaternion.Euler(0, 45, 0);
        }

        public void UpdateBehaviour()
        {
            var mousePos = GetMousePosition();
            UpdateCursor(mousePos);
            UpdateBodyRotation(mousePos);
            if (moveAction is not null)
            {
                var movement = moveAction.ReadValue<Vector2>();
                Animate(movement);
            }
        }



        public void FixedUpdateBehaviour()
        {
            if (moveAction == null)
            {
                Debug.LogError("moveAction not set", this);
                return;
            }

            if (rb == null)
            {
                Debug.LogError("rb not set", this);
                return;
            }

            if (settings == null)
            {
                Debug.LogError("settings not set", this);
                return;
            }

            if (Mech == null)
            {
                Debug.LogError("MechController not set", this);
                return;
            }

            var movement = moveAction.ReadValue<Vector2>();
            if (PlayerPreferences.Instance.mechControls)
            {
                rb.MovePosition(this.transform.position + (this.transform.forward * movement.y * Time.fixedDeltaTime * (Mech.WalkSpeed)));
                rb.MoveRotation(Quaternion.Euler(0, this.transform.rotation.eulerAngles.y + movement.x * Mech.RotateSpeed * Time.fixedDeltaTime, 0));
            }
            else
            {
                rb.MovePosition(this.transform.position + (this.transform.forward * movement.y * Time.fixedDeltaTime * (Mech.WalkSpeed)) + (this.transform.right * movement.x * Time.fixedDeltaTime * (Mech.WalkSpeed)));
                if (hips is not null && movement.sqrMagnitude > Vector3.kEpsilonNormalSqrt)
                {
                    var rotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.y), Vector3.up);
                    hips.rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 45, 0);

                }
            }

            hipsDirArrow?.SetActive(PlayerPreferences.Instance.mechControls);

            Animate(movement);
        }

        private void OnStart(InputAction.CallbackContext context)
        {
            Game.Instance.State.Start();
        }

        private void OnSelect(InputAction.CallbackContext context)
        {
            Game.Instance.State.Select();
        }

        private void OnPrimaryFire(InputAction.CallbackContext context)
        {
            if (Game.Instance.State.Playing)
            {
                if (mech != null)
                {
                    if (!TryPickup())
                    {
                        mech.FirePrimaryWeapon();
                    }
                }
                else
                {
                    Debug.LogError("weapons is null", this);
                }
            }
        }

        private void OnSecondaryFire(InputAction.CallbackContext context)
        {
            if (Game.Instance.State.Playing)
            {
                if (mech != null)
                {
                    mech.FireSecondaryWeapon();
                }
                else
                {
                    Debug.LogError("weapons is null", this);
                }
            }
        }

        private void OnRepairAction(InputAction.CallbackContext context)
        {
            Mech?.MaxHeal();
        }

        private void OnOverdriveAction(InputAction.CallbackContext context)
        {
            Mech?.Overdrive();
        }

        private void OnCoolantDumpAction(InputAction.CallbackContext context)
        {
            Mech?.CoolantDump();
        }

        private void OnChangePrimaryWeapon(InputAction.CallbackContext context)
        {
            var dir = context.ReadValue<float>();

            if (dir > 0)
            {
                mech?.NextPrimaryWeapon();
            }

            if (dir < 0)
            {
                mech?.PrevPrimaryWeapon();
            }
        }

        private void OnChangeSecondaryWeapon(InputAction.CallbackContext context)
        {
            var dir = context.ReadValue<float>();

            if (dir > 0)
            {
                mech?.NextSecondaryWeapon();
            }
            if (dir < 0)
            {
                mech?.PrevSecondaryWeapon();
            }
        }

        private Vector2 GetMousePosition()
        {
            if (mousePosAction == null)
            {
                Debug.LogError("mousePosAction not set", this);
                return Vector2.zero;
            }
            return mousePosAction.ReadValue<Vector2>();
        }

        private void UpdateBodyRotation(Vector2 mousePosition)
        {
            if (mousePosAction == null)
            {
                Debug.LogError("mousePosAction not set", this);
                return;
            }

            if (mainCamera == null)
            {
                Debug.LogError("mainCamera not set", this);
                return;
            }

            if (body == null)
            {
                Debug.LogError("body not set", this);
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.RaycastNonAlloc(ray, raycastHits, 100, aimMask) > 0)
            {
                var rotation = Quaternion.LookRotation(raycastHits[0].point - this.transform.position, Vector3.up);
                body.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

            }
        }

        private void UpdateCursor(Vector2 mousePos)
        {


            if (mainCamera is null)
            {
                Debug.LogError("mainCamera not set", this);
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray.origin, ray.direction, out var hit, 20, deadenemyMask))
            {
                hit.collider.TryGetComponent<DeadEnemy>(out var deadEnemy);

                if (deadEnemy is not null && deadEnemy.HasLoot)
                {

                    if ((transform.position - hit.collider.transform.position).sqrMagnitude <= (settings is not null ? settings.sqPickupRadius : 4f))
                    {
                        Cursor.SetCursor(pickupCursor, new Vector2(50, 50), CursorMode.Auto);
                        return;
                    }
                    else
                    {
                        Cursor.SetCursor(outofrangeCursor, new Vector2(50, 50), CursorMode.Auto);
                        return;
                    }
                }
            }

            Cursor.SetCursor(defaultCursor, new Vector2(50, 50), CursorMode.Auto);
        }

        private bool TryPickup()
        {

            if (mainCamera is null)
            {
                Debug.LogError("mainCamera not set", this);
                return false;
            }

            var mousePos = GetMousePosition();


            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray.origin, ray.direction, out var hit, 20, deadenemyMask))
            {
                if ((transform.position - hit.collider.transform.position).sqrMagnitude <= (settings is not null ? settings.sqPickupRadius : 4f))
                {
                    hit.collider.TryGetComponent<DeadEnemy>(out var deadEnemy);
                    if (deadEnemy != null)
                    {
                        List<InventoryItem> pickedupItems = new List<InventoryItem>();
                        foreach (var item in deadEnemy.loot)
                        {
                            if (Mech is not null && Mech.PickupItem(item))
                            {
                                Game.Instance.AddConsoleMessage("Picked up " + item.name + " (" + item.itemLevel + ")");
                                pickedupItems.Add(item);
                            }
                            else
                            {
                                Game.Instance.AddConsoleMessage("Could not pick up " + item.name + " (" + item.itemLevel + ")");
                            }
                        }

                        foreach (var item in pickedupItems)
                        {
                            deadEnemy.PickupLoot(item);
                        }
                    }
                    return true;
                }
                else
                {
                    Game.Instance.AddConsoleMessage("Out of range");
                    return false;
                }
            }
            return false;
        }

        private void Animate(Vector2 movement)
        {
            animator?.SetFloat("Rotation", movement.x);
            animator?.SetFloat("Speed", movement.y);
        }
    }
}