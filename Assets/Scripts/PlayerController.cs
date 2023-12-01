using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;

    private BULLYTInput input = null;

    private Vector2 moveVector;

    private bool attackPressed = false;

    public float speed = 5f; // Adjusted speed for better movement feel
    public float acceleration = 10f;
    private Vector2 currentVelocity;

    public AudioSource attackAudio;

    public bool isAttacking = false;
    private float velocity;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        input = new BULLYTInput();

        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMoveCancelled;

        input.Player.AttackSelect.performed += OnAttack;
        input.Player.AttackSelect.canceled += OnAttackCancelled;
    }

    private void OnDestroy()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMoveCancelled;

        input.Player.AttackSelect.performed -= OnAttack;
        input.Player.AttackSelect.canceled -= OnAttackCancelled;
    }

    private void OnMove(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>().normalized; // Normalize for consistent speed in all directions
    }

    private void OnMoveCancelled(InputAction.CallbackContext value) { moveVector = Vector2.zero; }

    private void OnAttack(InputAction.CallbackContext value)
    {
        attackPressed = value.ReadValue<float>() >= 1;
    }

    private void OnAttackCancelled(InputAction.CallbackContext value) { attackPressed = false; }

    void Update()
    {
        CharacterControl();
    }

    void CharacterControl()
    {
        if (isAttacking)
            return;

        if (attackPressed)
        {
            isAttacking = true;
            attackAudio.Play();
            StartCoroutine(ResetAttack());
        }

        Vector2 moveData = moveVector * speed;

        // Update the player's position using Rigidbody2D
        rigidBody2D.MovePosition(rigidBody2D.position + moveData * Time.fixedDeltaTime);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }
}
