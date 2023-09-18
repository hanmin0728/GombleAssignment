using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using Photon.Pun;

public class UIManager : MonoSingleTon<UIManager>
{

    [SerializeField] private TextMeshProUGUI textDelayTime = null;

    private int countTime = 3;


    public TextMeshProUGUI winText;
    public TextMeshProUGUI countText;

    public GameObject DisconnectPanel;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private Button retryBtn;

    private void Start()
    {
        //retryBtn.onClick.AddListener(() =>
        //{
        //    Debug.Log("!");
        //    winPanel.SetActive(false);
        //    PhotonNetwork.Disconnect();
        //});
    }

    public void RoomFull()
    {
        textDelayTime.gameObject.SetActive(true);
        StartCoroutine(ContinueDelay());
    }

    private IEnumerator ContinueDelay()
    {
        while (countTime > 0)
        {
            textDelayTime.text = string.Format("{0}", countTime);
            countTime--;
            yield return new WaitForSecondsRealtime(1f);

            if (countTime == 0)
            {
                textDelayTime.text = string.Format("게임 시작");
                yield return new WaitForSecondsRealtime(1f);
                GameManager.Instance.isFullRoom = true;
                textDelayTime.gameObject.SetActive(false);
            }
        }

        GameManager.Instance.SetGameTimeScale(1f);

        countTime = 3;
    }

    public void OnWinPanel(string name)
    {
        winPanel.SetActive(true);

        if (name == PhotonNetwork.NickName)
        {
            winText.text = "패배하셨습니다.";
            winText.color = Color.red;
        }
        else
        {
            winText.text = "승리를 축하드립니다";
            winText.color = Color.green;
        }
        GameManager.Instance.isFullRoom = false;


        StartCoroutine(CntText());
    }

    private IEnumerator CntText()
    {
        while (countTime > 0)
        {
            countTime--;
            yield return new WaitForSecondsRealtime(1f);

            if (countTime == 0)
            {
                Setting();
            }
        }

        countTime = 3;
    }
    public void Setting()
    {
        PhotonNetwork.Disconnect();
        winPanel.SetActive(false);
        GameManager.Instance.isFullRoom = false;
    }
}
