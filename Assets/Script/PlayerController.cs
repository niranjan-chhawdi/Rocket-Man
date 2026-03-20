using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float maxRunTime = 2f;
    public float runRechargeSpeed = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private float runTimeLeft;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        runTimeLeft = maxRunTime;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isRunning = shiftHeld && runTimeLeft > 0f && movement.sqrMagnitude > 0f;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        animator.SetFloat("Speed", movement.magnitude * currentSpeed);

        if (isRunning)
            runTimeLeft -= Time.deltaTime;
        else
            runTimeLeft = Mathf.Min(runTimeLeft + runRechargeSpeed * Time.deltaTime, maxRunTime);

        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isRunning = shiftHeld && runTimeLeft > 0f && movement.sqrMagnitude > 0f;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = movement.normalized * currentSpeed;
    }
}