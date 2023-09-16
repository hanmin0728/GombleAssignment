using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAirplane : PlayerBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePosTrm;
    private void Update()
    {
        Move();

        if (Input.GetMouseButtonDown(0))
        {
            OnAttack();
        }


    }
    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);
        Vector2 velocity = movement* moveSpeed;
        velocity.y = 0;
        rb2D.velocity = velocity;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            OnDamage();
        }
    }
    public override void OnAttack()
    {
        GameObject obj = PoolManager.Pop(PoolType.Bullet);
        obj.transform.position = firePosTrm.position; 
        Debug.Log("����");
        //�ִϸ��̼� ����

    }

    public override void OnDamage()
    {
        //�ǰ� �ִϸ��̼� ����
        SetDamge(attackPower);
    }

    public override void OnDead()
    {
        //���� �ִϸ��̼� ����
        //���� �̰���� �˷��ֱ�
    }
}
