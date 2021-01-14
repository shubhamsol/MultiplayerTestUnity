using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] GameObject startButton;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        print("joined MAster");
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        MenuManager.Instance.OpenMenu("title");
        print("joined lobby");
        PhotonNetwork.NickName = "PLAYER" + UnityEngine.Random.Range(0, 1000).ToString("000");
    }
    
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text)) return;
        PhotonNetwork.CreateRoom(roomNameInput.text);
        MenuManager.Instance.OpenMenu("loading");
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
        

    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        startButton.SetActive(PhotonNetwork.IsMasterClient);
        Player[] players = PhotonNetwork.PlayerList;
        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }    
        foreach (Player player in players)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = message;
        MenuManager.Instance.OpenMenu("Error");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");

    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");

    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("roomlist updated");
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        foreach(RoomInfo room in roomList)
        {
            if (room.RemovedFromList) continue;
            
            print(room.Name);
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

    }
}
