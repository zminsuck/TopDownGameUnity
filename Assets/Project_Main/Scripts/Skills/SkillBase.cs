using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("스킬 정보")]
    public string skillName = "스킬";
    public float cooldown = 3f;
    public float staminaCost = 20f;
    public float damage = 30f;

    private float _cooldownTimer = 0f;

    protected Transform Owner;
    protected PlayerController PlayerController;

    protected virtual void Awake()
    {
        Owner = transform;
        PlayerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_cooldownTimer > 0f)
            _cooldownTimer -= Time.deltaTime;
    }

    // 외부에서 스킬 발동 요청
    public bool TryUse()
    {
        if (!IsReady()) return false;

        _cooldownTimer = cooldown;
        OnUse();
        return true;
    }

    // 쿨다운 완료 여부
    public bool IsReady() => _cooldownTimer <= 0f;

    // 쿨다운 진행률 (UI용 0~1)
    public float CooldownRatio => Mathf.Clamp01(_cooldownTimer / cooldown);

    // 각 스킬이 구현할 실제 동작
    protected abstract void OnUse();
}