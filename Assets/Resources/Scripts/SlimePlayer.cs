using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePlayer : MonoBehaviourPun, IPunObservable {
    public float speed = 7f;

    public int playerID;
    // Start is called before the first frame update
    void Start() {
        if (photonView.IsMine) {
            photonView.RPC("SetupPlayer", RpcTarget.AllBuffered, PhotonManager.id);
        } else {
            Destroy(GetComponent<Controller>());
        }
            
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(transform.position);
            
        } else {
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
    [PunRPC]
    void SetupPlayer(int id) {
        if (GetComponent<Controller>() != null)
            GetComponent<Controller>().limits = Util.limits[id % 2];
        GetComponent<SpriteRenderer>().color = Util.playerColors[id % 2];
        transform.GetChild(0).transform.localPosition = Util.playerEyesLocalPosition[id % 2];
        Master master = GameObject.Find("Master").GetComponent<Master>();
        master.playersArray[id] = gameObject;
        master.playerEyes.Add(transform.GetChild(0).gameObject);
        playerID = id;
    }
}
