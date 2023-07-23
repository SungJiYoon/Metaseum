using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0f"; // ����

    public GameObject[] playerPrefab; // �÷��̾� ������
    private GameObject player;
    private string playerName;
    private string roomName;

    void Start()
    {
        player = playerPrefab[(int)DataManager.instance.currentCharacter];
        playerName = DataManager.instance.inputNickName.text;
        roomName = DataManager.instance.inputRoomName.text;

        // ���� ���̵� �Ҵ�
        PhotonNetwork.NickName = playerName;

        // ���� ���� �����鿡�� �ڵ����� ���� �ε�
        PhotonNetwork.AutomaticallySyncScene = true;

        // ���� ������ �������� ���� ���
        PhotonNetwork.GameVersion = version;

        // Photon ������ �����մϴ�.
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // ������ ������ ����Ǹ� ���� �����ϰų� �����մϴ�.
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // �뿡 ������ ����� ���� Ȯ��
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
            // $ => String.Format() : ""�ȿ��ִ°� ���ڿ��� ��ȯ�����.
        }

        // ĳ���� ���� ������ �迭�� ����
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        // ĳ���͸� ����
        PhotonNetwork.Instantiate(player.name, points[idx].position, points[idx].rotation, 0);
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
}

