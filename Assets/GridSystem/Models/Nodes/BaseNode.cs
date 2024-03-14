using System;
using UnityEngine;

public abstract class BaseNode : MonoBehaviour
{
    public abstract string Id { get; }


    private bool _isObstacle;

    public bool IsObstacle
    {
        get => _isObstacle;
        set => _isObstacle = value;
    }

    private int _health;

    public int Health
    {
        get => _health;
        set => _health = value;
    }


    public void TakeDamage(Action onDied = null, int damage = 1)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(this.gameObject);// you can use pooling system here
            onDied?.Invoke();
        }
    }


    public abstract string PoolInstanceId { get; protected internal set; }
}