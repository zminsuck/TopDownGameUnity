using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("스탯")]
    public float maxHp = 100f;
    public float damage = 10f;

    [Header("감지")]
    public float detectRange = 8f;    // 플레이어 감지 범위
    public float attackRange = 1.5f;  // 공격 사거리
    public float attackCooldown = 1.5f;

    [Header("이동")]
    public float moveSpeed = 3.5f;
    public float staggerDuration = 0.4f;

    // 상태
    private EnemyState _state = EnemyState.Idle;
    private float _hp;
    private float _attackTimer = 0f;
    private float _staggerTimer = 0f;

    // 컴포넌트
    private NavMeshAgent _agent;
    private Transform _player;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _hp = maxHp;

        _agent.speed = moveSpeed;
        _agent.stoppingDistance = attackRange * 0.9f;
        _agent.angularSpeed = 360f;
        _agent.acceleration = 12f;
    }

    private void Start()
    {
        // Player 태그로 자동 탐색
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        _attackTimer -= Time.deltaTime;

        switch (_state)
        {
            case EnemyState.Idle:    UpdateIdle();    break;
            case EnemyState.Chase:   UpdateChase();   break;
            case EnemyState.Attack:  UpdateAttack();  break;
            case EnemyState.Stagger: UpdateStagger(); break;
            case EnemyState.Dead:                     break;
        }
    }

    // ── 상태별 로직 ──────────────────────────────

    private void UpdateIdle()
    {
        if (GetDistToPlayer() <= detectRange)
            ChangeState(EnemyState.Chase);
    }

    private void UpdateChase()
    {
        float dist = GetDistToPlayer();

        if (dist <= attackRange)
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        if (dist > detectRange * 1.5f)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        _agent.SetDestination(_player.position);
        LookAtPlayer();
    }

    private void UpdateAttack()
    {
        _agent.ResetPath(); // 공격 중 이동 멈춤
        LookAtPlayer();

        float dist = GetDistToPlayer();

        // 플레이어가 멀어지면 다시 추격
        if (dist > attackRange * 1.2f)
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        // 쿨다운마다 공격
        if (_attackTimer <= 0f)
        {
            PerformAttack();
            _attackTimer = attackCooldown;
        }
    }

    private void UpdateStagger()
    {
        _staggerTimer -= Time.deltaTime;
        if (_staggerTimer <= 0f)
            ChangeState(EnemyState.Chase);
    }

    // ── 액션 ─────────────────────────────────────

    private void PerformAttack()
    {
        // 플레이어에게 데미지 전달
        IDamageable target = _player.GetComponent<IDamageable>();
        target?.TakeDamage(damage);

        Debug.Log($"{name} 공격! 데미지: {damage}");
    }

    public void TakeDamage(float dmg)
    {
        if (_state == EnemyState.Dead) return;

        _hp -= dmg;
        Debug.Log($"{name} 피격! 남은 HP: {_hp}");

        if (_hp <= 0f)
        {
            Die();
            return;
        }

        ChangeState(EnemyState.Stagger);
    }

    private void Die()
    {
        ChangeState(EnemyState.Dead);
        _agent.ResetPath();
        _agent.enabled = false;

        Debug.Log($"{name} 사망!");
        Destroy(gameObject, 1.5f);
    }

    // ── 유틸 ─────────────────────────────────────

    private void ChangeState(EnemyState newState)
    {
        _state = newState;

        if (newState == EnemyState.Stagger)
            _staggerTimer = staggerDuration;

        if (newState == EnemyState.Idle)
            _agent.ResetPath();
    }

    private void LookAtPlayer()
    {
        Vector3 dir = (_player.position - transform.position);
        dir.y = 0f;
        if (dir.magnitude > 0.1f)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * 10f
            );
    }

    private float GetDistToPlayer()
    {
        return Vector3.Distance(transform.position, _player.position);
    }

    // 감지 범위 Gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}