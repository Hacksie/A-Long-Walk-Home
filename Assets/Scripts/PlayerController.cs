#nullable enable
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    public class PlayerController : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] private Transform? body;
        [SerializeField] private Camera? mainCamera;
        [SerializeField] private Rigidbody? rb;
        [SerializeField] private PlayerInput? playerInput;
        [SerializeField] private Animator? animator;
        [SerializeField] private MechController? mech;

        [Header("Settings")]
        [SerializeField] private Settings? settings;
        [SerializeField] private LayerMask aimMask;
        [SerializeField] private LayerMask deadenemyMask;

        public MechController? Mech { get => mech; set => mech = value; }

        private InputAction? startAction;
        private InputAction? selectAction;
        private InputAction? moveAction;
        private InputAction? mousePosAction;
        private InputAction? primaryFire;
        private InputAction? secondaryFire;
        private InputAction? primaryAction;
        private InputAction? secondaryAction;
        private InputAction? changePrimaryAction;
        private InputAction? changeSecondaryAction;

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
            primaryAction = playerInput.actions["Overdrive"];
            secondaryAction = playerInput.actions["Coolant Dump"];

            changePrimaryAction = playerInput.actions["Change Primary Weapon"];
            changeSecondaryAction = playerInput.actions["Change Secondary Weapon"];

            primaryFire.performed += OnPrimaryFire;
            secondaryFire.performed += OnSecondaryFire;
            startAction.performed += OnStart;
            selectAction.performed += OnSelect;
            changePrimaryAction.performed += OnChangePrimaryWeapon;
            changeSecondaryAction.performed += OnChangeSecondaryWeapon;
            primaryAction.performed += OnPrimaryAction;
            secondaryAction.performed += OnSecondaryAction;

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
            this.transform.position = settings.startPosition;
            mech.selectedPrimaryWeapon = settings.startingPrimary;
            mech.selectedSecondaryWeapon = settings.startingSecondary;
            mech.linkArms = false;
            mech.linkShoulders = false;
            mech.UpdateModels();
            mech.SetItem(MechPosition.RightArm, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.claw));
            mech.SetItem(MechPosition.LeftArm, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.cannon));
            mech.SetItem(MechPosition.LeftShoulder, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.missiles));
            mech.SetItem(MechPosition.InvSlot0, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.scrap));
            mech.SetItem(MechPosition.Armour, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.armour).Randomize());
            mech.SetItem(MechPosition.Motor, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.motor).Randomize());
            mech.SetItem(MechPosition.Radar, ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.radar).Randomize());
        }

        public void Die()
        {
            moveAction?.Reset();
        }

        public void NewLevel()
        {
            if (settings == null)
            {
                Debug.LogError("settings is null", this);
                return;
            }
            this.transform.position = settings.startPosition;
        }

        public void UpdateBehaviour()
        {
            var mousePos = GetMousePosition();
            UpdateBodyRotation(mousePos);
            UpdatePickupHover(mousePos);
            if (moveAction != null)
            {
                var movement = moveAction.ReadValue<Vector2>();
                Animate(movement);
            }
        }

        public void UpdatePickupHover(Vector2 mousePosition)
        {
            if (mainCamera == null)
            {
                Debug.LogError("mainCamera not set", this);
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, 20, deadenemyMask))
            {
                Debug.Log("hovering over dead enemy", this);
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
            rb.MovePosition(this.transform.position + this.transform.forward * movement.y * Time.fixedDeltaTime * (Mech.WalkSpeed));
            rb.MoveRotation(Quaternion.Euler(0, this.transform.rotation.eulerAngles.y + movement.x * settings.rotateSpeed * Time.fixedDeltaTime, 0));
            Animate(movement);
        }

        public void OnStart(InputAction.CallbackContext context)
        {
            Game.Instance.State.Start();
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            Game.Instance.State.Select();
        }

        private void OnPrimaryFire(InputAction.CallbackContext context)
        {
            if (Game.Instance.State.Playing)
            {
                if (mech != null)
                {
                    mech.FirePrimaryWeapon();
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

        private void OnPrimaryAction(InputAction.CallbackContext context)
        {
            Mech?.Overdrive();
        }

        private void OnSecondaryAction(InputAction.CallbackContext context)
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

        private void Animate(Vector2 movement)
        {
            animator?.SetFloat("Rotation", movement.x);
            animator?.SetFloat("Speed", movement.y);
        }
    }
}