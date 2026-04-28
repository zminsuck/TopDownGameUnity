using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private float _range;
    private float _traveled;
    private LayerMask _enemyLayer;

    public void Init(float damage, float speed, float range)
    {
        _damage = damage;
        _speed = speed;
        _range = range;
        _enemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        float step = _speed * Time.deltaTime;
        transform.Translate(Vector3.forward * step);
        _traveled += step;

        if (_traveled >= _range)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _enemyLayer) == 0) return;

        other.GetComponent<IDamageable>()?.TakeDamage(_damage);
        Destroy(gameObject);
    }
}