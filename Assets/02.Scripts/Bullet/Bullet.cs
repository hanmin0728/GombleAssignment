using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolAbleObject
{
    private void Awake()
    {
        
    }
    //public PhotonView PV;

    [SerializeField] private Rigidbody2D rb2D;

    [SerializeField] private float bulletSpeed;

    private void OnEnable()
    {
        rb2D = GetComponent<Rigidbody2D>();

        rb2D.AddForce(Vector2.up * bulletSpeed);
        StartCoroutine(PushTiming());
    }
    public IEnumerator PushTiming()
    {
        yield return new WaitForSeconds(7f);
        PoolManager.Push(PoolType.Bullet, gameObject);
    }

    public override void Init_Pop()
    {
    }

    public override void Init_Push()
    {
    }

}
