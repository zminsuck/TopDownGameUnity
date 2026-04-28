using UnityEngine;
using System.Collections;

public class SkillDash : SkillBase
{
    [Header("돌진 설정")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;
    public float hitRange = 1.2f;

    private CharacterController _cc;

    protected override void Awake()
    {
        base.Awake();
        _cc = GetComponent<CharacterController>();
    }

    protected override void OnUse()
    {
        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        int enemyLayer = LayerMask.GetMask("Enemy");
        float timer = dashDuration;
        Vector3 dir = Owner.forward;
        bool hasHit = false;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            _cc.Move(dir * dashSpeed * Time.deltaTime);

            if (!hasHit)
            {
                Collider[] hits = Physics.OverlapSphere(
                    Owner.position,
                    hitRange,
                    enemyLayer
                );

                foreach (Collider hit in hits)
                {
                    hit.GetComponent<IDamageable>()?.TakeDamage(damage);
                    hasHit = true;
                }
            }

            yield return null;
        }

        Debug.Log($"[{skillName}] 발동!");
    }
}