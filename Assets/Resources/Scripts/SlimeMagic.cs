using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMagic : MonoBehaviourPun {
    SlimePlayer slimePlayer;
    Master master;
    float _mana = 100f;
    public float mana {
        get{
            return _mana;
        }
        set {
            _mana = value;
            if (slimePlayer.playerID == 0) {
                RectTransform rectT = master.manaUI.transform.GetChild(0).GetComponent<RectTransform>();
                rectT.anchorMin = new Vector2(0, 0);
                rectT.anchorMax = new Vector2(0.49f * _mana/100f, 0);
            } else if (slimePlayer.playerID == 1) {
                RectTransform rectT = master.manaUI.transform.GetChild(1).GetComponent<RectTransform>();
                rectT.anchorMin = new Vector2(1f - 0.49f * _mana/100f, 0);
                rectT.anchorMax = new Vector2(1f, 0);
            }
        }
    }
    float manaRegen = 5f;
    float[] costs = new float[] { 35f, 20f, 50f };


    // Start is called before the first frame update
    void Start() {
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
        slimePlayer = GetComponent<SlimePlayer>();
    }

    // Update is called once per frame
    void Update() {
        if (!PhotonManager.manaMode)
            return;
        if (mana + manaRegen * Time.deltaTime > 100f) {
            mana = 100f;
        } else {
            mana += manaRegen * Time.deltaTime;
        }
        if (!photonView.IsMine)
            return;
        int playerId = slimePlayer.playerID;

        Vector3 ballPos = master.ball.transform.position;
        Vector2 ballVel = master.ballRigidBody.velocity;

        //Code can be improved
        bool spell1 = Util.pressedSpell1(playerId);
        if (spell1 && mana > costs[0]) {
            photonView.RPC("Cast", RpcTarget.All, 0, ballPos, ballVel);
        }
        bool spell2 = Util.pressedSpell2(playerId);
        if (spell2 && mana > costs[1]) {
            photonView.RPC("Cast", RpcTarget.All, 1, ballPos, ballVel);
        }
        bool spell3 = Util.pressedSpell3(playerId);
        if (spell3 && mana > costs[2]) {
            photonView.RPC("Cast", RpcTarget.All, 2, ballPos, ballVel);
        }
    }

    [PunRPC]
    void Cast(int spellID, Vector3 ballPos, Vector2 ballVel) {
        mana -= costs[spellID];
        if (master.ballRigidBody.gravityScale == 0)
            return;
        master.ball.transform.position = ballPos;
        master.ballRigidBody.velocity = ballVel;
        switch (spellID) {
            case 0: master.ballRigidBody.velocity *= -1; break;
            case 1: float newVelY = Mathf.Max(master.ballRigidBody.velocity.y + 5f, 5f);
                    master.ballRigidBody.velocity = new Vector2(master.ballRigidBody.velocity.x, newVelY); break;
            case 2: master.ballRigidBody.velocity = new Vector2(0,master.ballRigidBody.velocity.y); break;
        }
    }
}
