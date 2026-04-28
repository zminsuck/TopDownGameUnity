using UnityEngine;

public class SkillSlash : SkillBase
{
    [Header("슬래시 설정")]
    public float slashRange = 3f;

    protected override void OnUse()
    {
        // 코드에서 직접 Enemy 레이어 가져오기
        int enemyLayer = LayerMask.GetMask("Enemy");

        Collider[] hits = Physics.OverlapSphere(
            Owner.position,
            slashRange,
            enemyLayer
        );

        foreach (Collider hit in hits)
        {
            hit.GetComponent<IDamageable>()?.TakeDamage(damage);
        }

        Debug.Log($"[{skillName}] 발동! {hits.Length}명 피격");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, slashRange);
    }
}