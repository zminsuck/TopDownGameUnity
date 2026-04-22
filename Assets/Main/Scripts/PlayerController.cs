using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 mousePos;

    void Awake()
    {
        // 리지드바디 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
    }

    // Input System의 'Send Messages' 방식: On + 액션이름
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnAim(InputValue value)
    {
        // 마우스의 스크린 좌표를 받아옴
        Vector2 mouseScreenPos = value.Get<Vector2>();
        
        // 카메라를 통해 월드 공간의 바닥 지점으로 변환
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // 바닥 평면 정의

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            mousePos = ray.GetPoint(rayDistance);
        }
    }

    void FixedUpdate()
    {
        // 1. 이동 처리 (Y축 속도는 유지하면서 X, Z축만 변경)
        Vector3 moveDelta = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
        rb.linearVelocity = new Vector3(moveDelta.x, rb.linearVelocity.y, moveDelta.z);

        // 2. 회전 처리 (캐릭터가 마우스 지점을 바라보게 함)
        LookAtMouse();
    }

    void LookAtMouse()
    {
        Vector3 lookDir = mousePos - transform.position;
        lookDir.y = 0; // 캐릭터가 위아래로 기울어지지 않게 함
        
        if (lookDir.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}