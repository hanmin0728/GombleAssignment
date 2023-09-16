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
    /// 데미지 입었을때 불리는 함수
    /// </summary>
    public abstract void OnDamage();


    /// <summary>
    /// 공격 하면 불리는 함수
    /// </summary>
    public abstract void OnAttack();

    /// <summary>
    /// hp 바뀌었을떄 불리는 함수
    /// </summary>
    /// <param name="damage"></param>
    public virtual void SetDamge(int damage)
    {
        hp -= damage;

        Debug.Log(damage + "만큼의 피해를 받으셨고 현재 hp는" + hp + "입니다"); 

        if (hp <= 0)
        {
            Debug.Log("HP가 0이 되어 죽었습니다.");
            OnDead();
        }
    }

    /// <summary>
    /// 죽으면 불리는 함수
    /// </summary>
    public abstract void OnDead();

}
