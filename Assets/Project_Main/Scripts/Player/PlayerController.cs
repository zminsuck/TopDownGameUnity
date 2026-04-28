using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("이동")]
    public float moveSpeed = 6f;

    [Header("스태미나")]
    public float maxStamina = 100f;
    public float staminaRegen = 20f;
    public float dodgeCost = 25f;
    public float dodgeSpeed = 18f;
    public float dodgeDuration = 0.25f;

    [Header("HP")]
    public float maxHp = 100f;
    public float HpRatio => _hp / maxHp;
    private float _hp;

    private CharacterController _cc;
    private Camera _cam;
    private ComboAttack _comboAttack;
    private Vector2 _moveInput;
    private Vector3 _dodgeDir;
    private float _stamina;
    private float _dodgeTimer;
    private bool _isDodging;

    private InputAction _moveAction;
    private InputAction _dodgeAction;
    private InputAction _lookAction;
    private InputAction _attackAction;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _cam = Camera.main;
        _stamina = maxStamina;
        _hp = maxHp;
        _comboAttack = GetComponent<ComboAttack>();
        
        _moveAction = new InputAction("Move", InputActionType.Value);
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up",    "<Keyboard>/w")
            .With("Down",  "<Keyboard>/s")
            .With("Left",  "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        _dodgeAction = new InputAction("Dodge", InputActionType.Button);
        _dodgeAction.AddBinding("<Keyboard>/leftShift");

        _lookAction = new InputAction("Look", InputActionType.Value, "<Mouse>/position");

        _dodgeAction.performed += _ => TryDodge();
        _attackAction = new InputAction("Attack", InputActionType.Button);
        _attackAction.AddBinding("<Mouse>/leftButton");
        _attackAction.performed += _ => _comboAttack?.TryAttack();
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _dodgeAction.Enable();
        _lookAction.Enable();
        _attackAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _dodgeAction.Disable();
        _lookAction.Disable();
        _attackAction.Disable();
    }

    private void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        HandleStamina();
        HandleMovement();
        HandleAiming();
    }

    public void TakeDamage(float damage)
    {
        if (_isDodging) return;

        _hp -= damage;
        Debug.Log($"플레이어 피격! 남은 HP: {_hp}");

        if (_hp <= 0f)
        {
            Die();
        }
    }

    private void TryDodge()
    {
        if (_isDodging) return;
        if (_stamina < dodgeCost) return;

        _stamina -= dodgeCost;
        _isDodging = true;
        _dodgeTimer = dodgeDuration;

        Vector3 input = GetWorldMoveDir();
        _dodgeDir = input.magnitude > 0.1f ? input.normalized : transform.forward;
    }

    private void HandleMovement()
{
    if (_isDodging)
    {
        _dodgeTimer -= Time.deltaTime;
        _cc.Move(_dodgeDir * dodgeSpeed * Time.deltaTime);
        if (_dodgeTimer <= 0f) _isDodging = false;
        return;
    }

    Vector3 dir = GetWorldMoveDir();
    float currentSpeed = moveSpeed * (_comboAttack ? _comboAttack.AttackMoveMultiplier : 1f); // ← 수정
    if (dir.magnitude > 0.1f)
        _cc.Move(dir * currentSpeed * Time.deltaTime);

    _cc.Move(Vector3.down * 9.8f * Time.deltaTime);
}

    private void HandleAiming()
    {
        Vector2 mouseScreen = _lookAction.ReadValue<Vector2>();
        Ray ray = _cam.ScreenPointToRay(mouseScreen);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            Vector3 dir = worldPoint - transform.position;
            dir.y = 0f;

            if (dir.magnitude > 0.1f)
                transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void HandleStamina()
    {
        if (_isDodging) return;
        _stamina = Mathf.Min(_stamina + staminaRegen * Time.deltaTime, maxStamina);
    }

    private Vector3 GetWorldMoveDir()
    {
        Vector3 camForward = _cam.transform.forward;
        Vector3 camRight = _cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        return camForward.normalized * _moveInput.y
             + camRight.normalized * _moveInput.x;
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");

        // 이동 및 입력 비활성화
        _moveAction.Disable();
        _dodgeAction.Disable();
        _attackAction.Disable();
        _lookAction.Disable();

        // 2초 후 씬 리로드 (나중에 게임오버 UI로 교체)
        StartCoroutine(RespawnCoroutine());
    }

    private System.Collections.IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public float StaminaRatio => _stamina / maxStamina;
    public bool IsDodging => _isDodging;
}