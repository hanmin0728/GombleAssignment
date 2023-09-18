using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField NickNameInput;
    public GameObject DisconnectPanel;
    public GameObject WinPanel;


    Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(0, -3.5f, 0) // 두 번째 플레이어의 시작 위치
    };

    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        DisconnectPanel.SetActive(false);
        Spawn();
    }

    public void Spawn()
    {
        Vector3 spawnPosition = spawnPositions[0];
        PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

        WinPanel.SetActive(false);
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectPanel.SetActive(true);

        Debug.Log("게임 종료");

        // 모든 PhotonView를 가져옵니다
        PlayerAirplane[] playerAirplanes = FindObjectsOfType<PlayerAirplane>();

        // 각 PhotonView에 대해
        foreach (PlayerAirplane playerAirplane in playerAirplanes)
        {
            if (playerAirplane.gameObject.activeSelf == true)
                // 해당 게임 오브젝트를 삭제합니다
                Destroy(playerAirplane.gameObject);
        }
    }
}


