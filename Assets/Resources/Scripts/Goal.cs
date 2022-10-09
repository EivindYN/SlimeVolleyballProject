using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviourPun
{
    public Coroutine scoreCoroutine;
    public Master master;
    GameObject ball => master.ball;
    void OnTriggerEnter2D(Collider2D other) {
        int winner = other.gameObject.transform.position.x > 0 ? 0 : 1;
        //Bedre men fikser ikke det grunnleggende problemet
        if (PhotonManager.id == 1 - winner || PhotonManager.offlineMode) 
            photonView.RPC("ScoreRPC", RpcTarget.All, winner);
        //StartCoroutine("Score", winner);
    }

    void Update() {
        if (ball) {
            if (ball.transform.position.y < -15f) {
                photonView.RPC("ScoreRPC", RpcTarget.All, -1);
            }
        }
    }

    [PunRPC]
    void ScoreRPC(int winner) {
        if (winner == -1) {
            master.Reset(master.lastWinner);
            master.resultText.GetComponent<Text>().text = "Dsync error occoured";
        } else {
            scoreCoroutine = StartCoroutine("Score", winner);
        }
    }

    IEnumerator Score(int winner) {
        ball.GetComponent<Rigidbody2D>().gravityScale = 0;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Controller.disabled = true;
        if (!master.AddPoint(winner)) {
            yield return new WaitForSecondsRealtime(1f);
            master.Reset(winner);
        }    
    }
}
