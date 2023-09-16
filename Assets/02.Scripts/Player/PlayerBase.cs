using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class PlayerBase : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerDataSO;

    protected Collider2D col2D;
    protected Rigidbody2D rb2D;

    protected int hp;
    protected int attackPower;
    protected int moveSpeed;

    private void Start()
    {
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        SetPlayerInfo();

    }


    public void SetPlayerInfo()
    {
        hp = playerDataSO.maxHp;
        attackPower = playerDataSO.atkPower;
        moveSpeed = playerDataSO.moveSpeed;
    }


    /// <summary>
    /// ������ �Ծ����� �Ҹ��� �Լ�
    /// </summary>
    public abstract void OnDamage();


    /// <summary>
    /// ���� �ϸ� �Ҹ��� �Լ�
    /// </summary>
    public abstract void OnAttack();

    /// <summary>
    /// hp �ٲ������ �Ҹ��� �Լ�
    /// </summary>
    /// <param name="damage"></param>
    public virtual void SetDamge(int damage)
    {
        hp -= damage;

        Debug.Log(damage + "��ŭ�� ���ظ� �����̰� ���� hp��" + hp + "�Դϴ�"); 

        if (hp <= 0)
        {
            Debug.Log("HP�� 0�� �Ǿ� �׾����ϴ�.");
            OnDead();
        }
    }

    /// <summary>
    /// ������ �Ҹ��� �Լ�
    /// </summary>
    public abstract void OnDead();

}
