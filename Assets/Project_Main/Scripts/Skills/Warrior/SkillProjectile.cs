using UnityEngine;

public class SkillProjectile : SkillBase
{
    [Header("프로젝타일 설정")]
    public float projectileSpeed = 15f;
    public float projectileRange = 20f;

    protected override void OnUse()
    {
        // Prefab 없이 코드로 직접 생성
        GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        proj.name = "Projectile";
        proj.transform.position = Owner.position + Owner.forward * 0.8f;
        proj.transform.rotation = Owner.rotation;
        proj.transform.localScale = Vector3.one * 0.3f;

        // 콜라이더 Trigger로 변경
        SphereCollider col = proj.GetComponent<SphereCollider>();
        col.isTrigger = true;

        // Rigidbody 추가
        Rigidbody rb = proj.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        // Projectile 스크립트 추가
        Projectile p = proj.AddComponent<Projectile>();
        p.Init(damage, projectileSpeed, projectileRange);

        Debug.Log($"[{skillName}] 발사!");
    }
}