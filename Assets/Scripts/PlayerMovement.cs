using UnityEngine;
using UnityEngine.InputSystem; // Bắt buộc phải có dòng này để dùng hệ thống mới

public class PlayerMovement : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    public CharacterController controller;
    public Animator animator;
    public float walkSpeed = 2f;
    public float runSpeed = 5.0f;
    public float gravity = -9.81f;
    public float turnTime = 0.1f;

    private float turnSmoothVelocity;
    private Vector3 velocity;

    void Update()
    {
        // 1. LẤY ĐẦU VÀO TỪ BÀN PHÍM (Hệ thống Input System mới)
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.y = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.y = -1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.x = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x = 1;
        }

        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        // 2. XỬ LÝ DI CHUYỂN VÀ XOAY
        if (direction.magnitude >= 0.1f)
        {
            // Kiểm tra phím Shift để chạy
            bool isRunning = Keyboard.current.leftShiftKey.isPressed;
            float targetSpeed = isRunning ? runSpeed : walkSpeed;

            // Xoay nhân vật mượt mà theo hướng đi
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Di chuyển nhân vật
            controller.Move(direction * targetSpeed * Time.deltaTime);

            // Cập nhật Animator: 1 = Walk, 2 = Run (Khớp với Blend Tree của bạn)
            animator.SetFloat("Speed", isRunning ? 2f : 1f, 0.1f, Time.deltaTime);
        }
        else
        {
            // Trạng thái đứng yên (Idle)
            animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }

        // 3. XỬ LÝ TRỌNG LỰC (Giúp nhân vật không bị bay lơ lửng)
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}