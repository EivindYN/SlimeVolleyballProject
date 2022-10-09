using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks {
    public static int id;
    public static byte playerAmount = 2;

    public GameObject ballPrefab;
    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.SendRate = 50;
        PhotonNetwork.SerializationRate = 50;
    }

    //Code can be improved
    float resetTimer = 0;
    float resetDuration = 1f;
    float manaTimer = 0;
    float manaDuration = 1f;
    float offlineTimer = 0;
    float offlineDuration = 1f;
    float shaderpornTimer = 0;
    float shaderpornDuration = 1f;

    public static bool manaMode;
    public static bool offlineMode;
    public static bool shaderpornMode;

    public Material shaderMat;
    void Update() {
        if (Input.GetKey(KeyCode.R)) {
            resetTimer += Time.deltaTime;
            if (resetTimer > resetDuration) {
                resetTimer = 0;
                photonView.RPC("StartGame", RpcTarget.All);
            }
        } else {
            resetTimer = 0;
        }
        if (Input.GetKey(KeyCode.M)) {
            manaTimer += Time.deltaTime;
            if (manaTimer > manaDuration) {
                manaTimer = 0;
                photonView.RPC("EnableMana", RpcTarget.All);
            }
        } else {
            manaTimer = 0;
        }
        if (Input.GetKey(KeyCode.O)) {
            offlineTimer += Time.deltaTime;
            if (offlineTimer > offlineDuration) {
                offlineTimer = 0;
                id = 1;
                offlineMode = true;
                PhotonNetwork.Instantiate("Player", Util.playerStartPos(1), Quaternion.identity);
                photonView.RPC("StartGame", RpcTarget.All);
            }
        } else {
            offlineTimer = 0;
        }
        if (Input.GetKey(KeyCode.P)) {
            shaderpornTimer += Time.deltaTime;
            if (shaderpornTimer > shaderpornDuration) {
                shaderpornTimer = 0;
                GameObject.FindGameObjectWithTag("Ball").GetComponent<SpriteRenderer>().material = shaderMat;
            }
        } else {
            shaderpornTimer = 0;
        }
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message) {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = playerAmount });
    }
    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        id = PhotonNetwork.PlayerList.Length - 1;
        PhotonNetwork.Instantiate("Player", Util.playerStartPos(id), Quaternion.identity);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.PlayerList.Length == playerAmount && id == 0)
            photonView.RPC("StartGame", RpcTarget.All);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) {
        base.OnPlayerLeftRoom(otherPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinRandomRoom();
    }

    [PunRPC]
    void StartGame() {
        Master master = GetComponent<Master>();
        if (master.ball == null)
            Instantiate(ballPrefab, new Vector3(Util.playerStartPos(0).x, 0), Quaternion.identity);
        master.FullReset();
    }
    [PunRPC]
    void EnableMana() {
        Master master = GetComponent<Master>();
        master.manaUI.SetActive(true);
        manaMode = true;
    }
}
