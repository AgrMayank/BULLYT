using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BULLYTInput input = null;

    private Vector2 moveVector;

    private bool attackPressed = false;

    public float stepSize = 0.1f;

    public float speed = 1;
    public float acceleration = 2;
    Vector2 currentVelocity;

    public AudioSource attackAudio;

    public bool isAttacking = false;
    private float velocity;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;

    private void Awake()
    {
        input = new();

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
        moveVector = value.ReadValue<Vector2>();

        if (Mathf.Abs(moveVector.x) > Mathf.Abs(moveVector.y))
        {
            // x has a greater absolute value, so set y to 0
            moveVector = new Vector2(moveVector.x, 0);
        }
        else
        {
            // y has a greater absolute value, so set x to 0
            moveVector = new Vector2(0, moveVector.y);
        }
    }

    private void OnMoveCancelled(InputAction.CallbackContext value) { moveVector = Vector2.zero; }

    private void OnAttack(InputAction.CallbackContext value)
    {
        attackPressed = value.ReadValue<float>() >= 1;
    }

    private void OnAttackCancelled(InputAction.CallbackContext value) { attackPressed = false; }

    void Update()
    {
        // if (HealthController.Instance.m_playerLost)
        //     return;

        CharacterControl();
    }

    #region NEW Input System

    void CharacterControl()
    {
        if (isAttacking)
            return;

        if (attackPressed)
        {
            // RumbleManager.Instance.RumblePulse();
            isAttacking = true;
            attackAudio.Play();

            // if (animator)
            // {
            //     animator.SetTrigger("Attack");
            // }

            StartCoroutine(ResetAttack());
        }

        Vector2 moveData = moveVector * stepSize;

        if (moveData == Vector2.zero)
        {
            Debug.Log("ZERO - ");
            velocity = 0;
        }
        else
        {
            Debug.Log("NMON  ZERO - ");

            velocity = Mathf.Clamp01(velocity + Time.deltaTime * acceleration);
            rigidBody2D.velocity = Vector2.SmoothDamp(rigidBody2D.velocity, moveData * speed, ref currentVelocity, acceleration, speed);
            spriteRenderer.flipX = rigidBody2D.velocity.x >= 0 ? true : false;
        }
    }

    #endregion

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }
}