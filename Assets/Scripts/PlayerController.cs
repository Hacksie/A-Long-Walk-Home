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
        [SerializeField] private MechController? weapons;

        [Header("Settings")]
        [SerializeField] private Settings? settings;
        [SerializeField] private LayerMask aimMask;

        public MechController? Weapons { get => weapons; set => weapons = value; }

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
            weapons = weapons ?? GetComponent<MechController>();

            startAction = playerInput.actions["Start"];
            selectAction = playerInput.actions["Select"];
            moveAction = playerInput.actions["Move"];
            mousePosAction = playerInput.actions["Mouse Position"];
            primaryFire = playerInput.actions["Primary Fire"];
            secondaryFire = playerInput.actions["Secondary Fire"];
            primaryAction = playerInput.actions["Primary Action"];
            secondaryAction = playerInput.actions["Secondary Action"];

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
            if (weapons == null)
            {
                Debug.LogError("weapons is null", this);
                return;
            }
            this.transform.position = settings.startPosition;
            weapons.selectedPrimaryWeapon = settings.startingPrimary;
            weapons.selectedSecondaryWeapon = settings.startingSecondary;
            // weapons.leftArmWeapon = settings.startingLeftArm;
            // weapons.rightArmWeapon = settings.startingRightArm;
            // weapons.leftShoulderWeapon = settings.startingLeftShoulder;
            // weapons.rightShoulderWeapon = settings.startingRightShoulder;
            // weapons.noseWeapon = settings.startingNose;
            weapons.linkArms = false;
            weapons.linkShoulders = false;
            weapons.UpdateModels();
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
            UpdateBodyRotation();
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

            var movement = moveAction.ReadValue<Vector2>();
            rb.MovePosition(this.transform.position + this.transform.forward * movement.y * Time.fixedDeltaTime * (settings.walkSpeed));
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
                if (weapons != null)
                {
                    weapons.FirePrimaryWeapon();
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
                if (weapons != null)
                {
                    weapons.FireSecondaryWeapon();
                }
                else
                {
                    Debug.LogError("weapons is null", this);
                }
            }
        }

        private void OnPrimaryAction(InputAction.CallbackContext context)
        {

        }

        private void OnSecondaryAction(InputAction.CallbackContext context)
        {
            Game.Instance.CoolantDump();
        }

        private void OnChangePrimaryWeapon(InputAction.CallbackContext context)
        {
            var dir = context.ReadValue<float>();

            if (dir > 0)
            {
                weapons?.NextPrimaryWeapon();
            }

            if (dir < 0)
            {
                weapons?.PrevPrimaryWeapon();
            }
        }

        private void OnChangeSecondaryWeapon(InputAction.CallbackContext context)
        {
            var dir = context.ReadValue<float>();

            if (dir > 0)
            {
                weapons?.NextSecondaryWeapon();
            }

            if (dir < 0)
            {
                weapons?.PrevSecondaryWeapon();
            }
        }

        private void UpdateBodyRotation()
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

            var mousePosition = mousePosAction.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.RaycastNonAlloc(ray, raycastHits, 100, aimMask) > 0)
            {
                var rotation = Quaternion.LookRotation(raycastHits[0].point - this.transform.position, Vector3.up);
                body.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

            }
        }

        private void Animate(Vector3 movement)
        {
            animator?.SetFloat("Rotation", movement.x);
            animator?.SetFloat("Speed", movement.y);
        }
    }
}