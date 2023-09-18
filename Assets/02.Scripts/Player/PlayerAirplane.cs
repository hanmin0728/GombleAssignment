using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAirplane : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;

    #region 플레이어 데이터 관련
    [SerializeField] private PlayerDataSO playerDataSO;

    private int maxHP;
    private int hp;
    private int attackPower;
    private int moveSpeed;
    #endregion

    #region 플레이어 컴포넌트
    private Collider2D col2D;
    private Rigidbody2D rb2D;
    private SpriteRenderer sr;
    #endregion


    #region 총알 발사 관련
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePosTrm;

    [SerializeField] private float fireDelay = 0.5f;

    private bool isCanFire = true; //발사 가능상태
    #endregion

    #region 비쥬얼 관련 변수
    [SerializeField] private Sprite playerAirplaneSprite;
    [SerializeField] private Sprite enemyAirplaneSprite;
    [SerializeField] private TextMeshProUGUI nickNameText;

    [SerializeField] private Sprite enemyBulletSprite;

    #endregion

    #region 피격 관련 변수
    private bool isCanHit = true;
    private float invincibleTime = 0.3f;
    #endregion


    #region hp관련
    [SerializeField] private Image healthImage;
    [SerializeField] private TextMeshProUGUI hptext;
    [SerializeField] private GameObject heart;
    [SerializeField] private Transform heartTrm;
    #endregion

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        SetPlayerInfo();

        SetPlayerVisual();
    }

    private void Start()
    {
        if (PV.IsMine == false) Destroy(rb2D);

        StartCoroutine(Atmosphere());

        for (int i = 0; i < maxHP; i++)
        {
            GameObject obj = Instantiate(heart, transform.position, Quaternion.identity, heartTrm);
        }
    }
    public IEnumerator Atmosphere()
    {
        yield return new WaitForSeconds(1.5f);

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PV.RPC("RoomFull", RpcTarget.AllBuffered);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.isFullRoom == false)
        {
            return;
        }

        if (PV.IsMine)
        {
            Move();
        }

        if (PV.IsMine)
        {
            if (Input.GetMouseButtonDown(0) && isCanFire)
            {
                StartCoroutine(OnAttack());
            }
        }

    }
   
    public void SetPlayerInfo()
    {
        maxHP = playerDataSO.maxHp;
        hp = maxHP;
        attackPower = playerDataSO.atkPower;
        moveSpeed = playerDataSO.moveSpeed;
    }
    private void SetPlayerVisual()
    {
        nickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        nickNameText.color = PV.IsMine ? Color.green : Color.red;
        sr.sprite = PV.IsMine ? playerAirplaneSprite : enemyAirplaneSprite;

        healthImage.fillAmount = maxHP;
    }
    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);
        Vector2 velocity = movement * moveSpeed;
        velocity.y = 0;
        rb2D.velocity = velocity;
    }

    public IEnumerator OnAttack()
    {
        isCanFire = false;
        PV.RPC("SpawnBullet", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(fireDelay); // 딜레이 대기

        isCanFire = true; // 발사 가능 상태로 변경
    }


    [PunRPC]
    public void SpawnBullet()
    {
        GameObject obj = PoolManager.Pop(PoolType.Bullet);
        Vector2 direction = Vector2.up;
        if (PV == null || PV.IsMine == false)
        {
            direction = Vector2.down;
            obj.GetComponent<SpriteRenderer>().flipY = true;
            obj.GetComponent<SpriteRenderer>().sprite = enemyBulletSprite;
        }

        obj.transform.position = firePosTrm.position;
        obj.GetComponent<Rigidbody2D>().AddForce(direction * 100);
    }


    [PunRPC]
    public void OnDamage()
    {
        if (isCanHit)
        {
            StartCoroutine(SetDamge(attackPower));
        }
    }
    public IEnumerator SetDamge(int damage)
    {
        isCanHit = false;
        hp -= damage;

        healthImage.fillAmount -= 0.3f;

        if (heartTrm.childCount > 0)
        {
            Destroy(heartTrm.GetChild(0).gameObject);
        }

        hptext.text = hp.ToString();

        if (PV.IsMine && hp <= 0)
        {
            PV.RPC("OnDead", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
        yield return new WaitForSeconds(invincibleTime);
        isCanHit = true;
    }

    [PunRPC]
    public void OnDead(string name)
    {
        UIManager.Instance.OnWinPanel(name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ///불렛과 충돌 피격처리를 플레이어에서

        if (collision.gameObject.CompareTag("Bullet"))
        {
            PV.RPC("OnDamage", RpcTarget.AllBuffered);
            PoolManager.Push(PoolType.Bullet, collision.gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hptext.text);
          //  stream.SendNext(healthImage.fillAmount);
        }
        else
        {
            hptext.text = (string)stream.ReceiveNext();
        //    healthImage.fillAmount = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void RoomFull()
    {
        UIManager.Instance.RoomFull();
    }

  
}
