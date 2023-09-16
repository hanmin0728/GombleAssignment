using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : PlayerBase
{
    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb2D.velocity = movement * moveSpeed;
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
