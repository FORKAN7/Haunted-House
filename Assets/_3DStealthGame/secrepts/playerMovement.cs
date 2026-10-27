using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public InputAction MoveAction;

    public float walkSpeed = 1f;
    public float turnSpeed = 20f;

    private Rigidbody m_Rigidbody;
    private Animator m_Animator;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        MoveAction.Enable();
    }

    private void OnDisable()
    {
        MoveAction.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 input = MoveAction.ReadValue<Vector2>();

        float horizontal = input.x;
        float vertical = input.y;

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement = Vector3.ClampMagnitude(movement, 1f);

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        m_Animator.SetBool("IsWalking", isWalking);

        if (movement.sqrMagnitude < 0.0001f)
            return;

        Vector3 desiredForward = Vector3.RotateTowards(
            m_Rigidbody.rotation * Vector3.forward,
            movement.normalized,
            turnSpeed * Time.fixedDeltaTime,
            0f
        );

        m_Rigidbody.MoveRotation(
            Quaternion.LookRotation(desiredForward)
        );

        m_Rigidbody.MovePosition(
            m_Rigidbody.position
            + movement * walkSpeed * Time.fixedDeltaTime
        );
    }
}