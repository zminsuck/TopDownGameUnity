using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    [Header("콤보 설정")]
    public int maxCombo = 3;
    public float comboResetTime = 1.2f;   // 이 시간 안에 입력 없으면 콤보 초기화
    public float attackDuration = 0.35f;  // 한 타 지속 시간

    [Header("히트박스")]
    public float attackRange = 1.8f;      // 부채꼴 반지름
    public float attackAngle = 90f;       // 부채꼴 각도
    public LayerMask enemyLayer;

    [Header("이동 제한")]
    public float attackMoveMultiplier = 0.3f; // 공격 중 이동속도 배율

    // 상태
    private int _comboStep = 0;
    private float _comboTimer = 0f;
    private float _attackTimer = 0f;
    private bool _isAttacking = false;

    // 컴포넌트
    private PlayerController _pc;

    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // 공격 지속 시간 카운트
        if (_isAttacking)
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0f)
                _isAttacking = false;
        }

        // 콤보 리셋 타이머
        if (_comboStep > 0 && !_isAttacking)
        {
            _comboTimer -= Time.deltaTime;
            if (_comboTimer <= 0f)
                ResetCombo();
        }
    }

    public void TryAttack()
    {
        if (_isAttacking) return;

        _comboStep++;
        if (_comboStep > maxCombo)
            _comboStep = 1;

        _comboTimer = comboResetTime;
        _attackTimer = attackDuration;
        _isAttacking = true;

        PerformHit(_comboStep);
    }

    private void PerformHit(int step)
    {
        // 부채꼴 범위 안의 적 탐색
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider hit in hits)
        {
            // 부채꼴 각도 체크
            Vector3 dirToEnemy = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToEnemy);

            if (angle <= attackAngle * 0.5f)
            {
                // 데미지 전달
                IDamageable damageable = hit.GetComponent<IDamageable>();
                damageable?.TakeDamage(GetDamage(step));

                Debug.Log($"{step}타 히트! 대상: {hit.name}");
            }
        }
    }

    private float GetDamage(int step)
    {
        // 콤보 단계별 데미지
        return step switch
        {
            1 => 10f,
            2 => 12f,
            3 => 20f,  // 마지막 타 강타
            _ => 10f
        };
    }

    private void ResetCombo()
    {
        _comboStep = 0;
        _comboTimer = 0f;
    }

    public bool IsAttacking => _isAttacking;
    public float AttackMoveMultiplier => _isAttacking ? attackMoveMultiplier : 1f;

    // 에디터에서 히트박스 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 부채꼴 방향 표시
        Vector3 leftDir = Quaternion.Euler(0, -attackAngle * 0.5f, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, attackAngle * 0.5f, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftDir * attackRange);
        Gizmos.DrawRay(transform.position, rightDir * attackRange);
    }
}