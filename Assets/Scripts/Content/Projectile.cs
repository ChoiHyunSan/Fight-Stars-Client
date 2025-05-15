using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed = 0f;
    public int _id = 0;

    void FixedUpdate()
    {
        // ¿Ãµø
        transform.position += (Vector3)_direction * Time.fixedDeltaTime * _speed;
    }

    public void Initialize(int projectileId, Vector2 direction, float speed)
    {
        _id = projectileId;
        _direction = direction.normalized;
        _speed = speed;
    }
}
