using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolAbleObject
{
    [SerializeField] private Rigidbody2D rb2D;

    [SerializeField] private float bulletSpeed;

 
    public override void Init_Pop()
    {
        rb2D = GetComponent<Rigidbody2D>();

        rb2D.AddForce(Vector2.up * bulletSpeed);
    }

    public override void Init_Push()
    {
    }

}
