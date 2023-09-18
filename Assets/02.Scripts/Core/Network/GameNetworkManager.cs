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
        new Vector3(0, -3.5f, 0) // �� ��° �÷��̾��� ���� ��ġ
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

        Debug.Log("���� ����");

        // ��� PhotonView�� �����ɴϴ�
        PlayerAirplane[] playerAirplanes = FindObjectsOfType<PlayerAirplane>();

        // �� PhotonView�� ����
        foreach (PlayerAirplane playerAirplane in playerAirplanes)
        {
            if (playerAirplane.gameObject.activeSelf == true)
                // �ش� ���� ������Ʈ�� �����մϴ�
                Destroy(playerAirplane.gameObject);
        }
    }
}


