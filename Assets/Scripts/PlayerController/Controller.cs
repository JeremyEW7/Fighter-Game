using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.InputSystem;

using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    [Header("Player Movement")]
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    private CharacterController characterController;
    private Animator animator;
    private PlayerInput playerInput;
    private Vector3 movement;

    [Header("Player Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamages = 5;
    public string[] attackAnimation = {"Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation"};
    private Controller enemyController;
    public GameObject[] characterModels;

    private float lastAttackTime;

    [Header("SFX")]
    public AudioClip[] hitSounds;
    public AudioClip[] blockSounds;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    [SerializeField] private int playerIndex;

    public static Action<int> OnDeath;

    void Awake()
    {
        Initialize();
    }

    void Update() 
    {
        PerformMovement();
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.GetType() == typeof(CharacterController) && other.TryGetComponent(out Controller controller))
        {
            if (controller.playerIndex != playerIndex)
            {
                enemyController = controller;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetType() == typeof(CharacterController) && other.TryGetComponent(out Controller controller))
        {
            if (controller.playerIndex != playerIndex)
            {
                enemyController = null;
            }
        }
    }

    private void OnEnable()
    {
        PauseMenu.OnPauseStatusChanged += ChangeActionMapBasedOnPauseStatus;
    }

    private void OnDisable()
    {
        PauseMenu.OnPauseStatusChanged -= ChangeActionMapBasedOnPauseStatus;
    }

    private void ChangeActionMapBasedOnPauseStatus(bool isPaused)
    {
        if (isPaused)
        {
            playerInput.SwitchCurrentActionMap("Pause");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Battle");
        }
    }

    void Initialize()
    {
        Camera main = Camera.main;
        main.GetComponent<CameraController>().playerTransforms.Add(gameObject.transform);
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
        healthBar = GameObject.FindGameObjectsWithTag("Healthbar").First((hb) => hb.GetComponent<HealthBar>().playerIndex == playerIndex).GetComponent<HealthBar>();
        currentHealth = maxHealth;
        healthBar.GiveFullHealth(currentHealth);
        characterController = GetComponent<CharacterController>();
        characterController.enabled = true;

        characterModels[playerIndex].SetActive(true);
        animator = GetComponentInChildren<Animator>();
    }

    public void PerformMovement(InputAction.CallbackContext obj)
    {
        if (obj.phase == InputActionPhase.Performed)
        {
            float horizontalInput = obj.ReadValue<float>();

            movement = new Vector3(0f, 0f, horizontalInput);
        }
        else if (obj.phase == InputActionPhase.Canceled)
        {
            movement = Vector3.zero;
        }
    }

    private void PerformMovement()
    {
        if(!animator.GetBool("CanWalk")) return;
        if(movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("Walking", true);
        }

        else
        {
            animator.SetBool("Walking", false);
        }
        characterController.Move(movement * movementSpeed * Time.deltaTime);
    }

    public void PerformAttack(int attackIndex)
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.Play(attackAnimation[attackIndex]);

            int damage = attackDamages;
            lastAttackTime = Time.time;

            if (enemyController != null)
            {
                StartCoroutine(enemyController.TakeDamage(damage));
            }
        }
        else
        {
            Debug.Log("Cannot perform attack yet");
        }
    }

    public void PerformBlock(InputAction.CallbackContext obj)
    {
        if (obj.phase == InputActionPhase.Performed && animator.GetBool("CanWalk"))
        {
            animator.SetBool("Blocking", true);
            animator.SetBool("CanWalk", false);
        }

        else if (obj.phase == InputActionPhase.Canceled && animator.GetBool("Blocking"))
        {
            animator.SetBool("Blocking", false);
            animator.SetBool("CanWalk", true);
        }
    }

    public IEnumerator TakeDamage(int damageTaken)
    {

        yield return new WaitForSeconds(0.5f);

        if (animator.GetBool("Blocking"))
        {
            damageTaken /= 2;

            if (blockSounds != null && blockSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, blockSounds.Length);
                AudioSource.PlayClipAtPoint(blockSounds[randomIndex], transform.position);
            }
        }
        else
        {
            PlayHitDamageAnimation();
        }

        currentHealth -= damageTaken;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void PlayHitDamageAnimation()
    {
        if (hitSounds != null && hitSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSounds.Length);
            AudioSource.PlayClipAtPoint(hitSounds[randomIndex], transform.position);
        }

        healthBar.SetHealth(currentHealth);
        animator.Play("HitDamageAnimation");
    }

    void Die()
    {
        OnDeath?.Invoke(playerIndex);
    }
}