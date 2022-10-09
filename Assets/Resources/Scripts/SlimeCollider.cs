using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeCollider : MonoBehaviourPun, IPunObservable {
    private Master master;
    private GameObject ball => master.ball;
    private Rigidbody2D ballRigidBody => master.ballRigidBody;
    void Start() {
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
    }

    float MAX_VELOCITY_X = 15;
    float MAX_VELOCITY_Y = 11;

    Vector3 ballPosition;
    Vector2 ballVelocity;

    public int touchedBallLast;

    void Update() {
        if (!photonView.IsMine)
            return;
        if (ball != null) {
            var dist = (ball.transform.position - transform.position).magnitude;
            var radius = 0.9f;
            var ballRadius = 0.25f;
            var dx = ball.transform.position.x - transform.position.x;
            var dy = ball.transform.position.y - transform.position.y;
            var ballVelocityX = ball.GetComponent<Rigidbody2D>().velocity.x;
            var ballVelocityY = ball.GetComponent<Rigidbody2D>().velocity.y;
            var velocityX = GetComponent<Controller>().velocityX;
            var velocityY = GetComponent<Controller>().velocityY;
            var dVelocityX = ballVelocityX - velocityY;
            var dVelocityY = ballVelocityY - velocityY;
            if (dist < radius + ballRadius && ball.transform.position.y > transform.position.y) {
                var newPosX = transform.position.x + (radius + ballRadius) * dx / dist;
                var newPosY = transform.position.y + (radius + ballRadius) * dy / dist;
                ball.transform.position = new Vector3(newPosX, newPosY);

                var something = (dx * dVelocityX + dy * dVelocityY) / dist;
                if (something <= 0) {
                    ballVelocityX += velocityX - 2.2f * dx * something / dist;
                    ballVelocityY += velocityY - 2.2f * dy * something / dist;
                    if (ballVelocityX < -MAX_VELOCITY_X) ballVelocityX = -MAX_VELOCITY_X;
                    else if (ballVelocityX > MAX_VELOCITY_X) ballVelocityX = MAX_VELOCITY_X;
                    if (ballVelocityY < -MAX_VELOCITY_Y) ballVelocityY = -MAX_VELOCITY_Y;
                    else if (ballVelocityY > MAX_VELOCITY_Y) ballVelocityY = MAX_VELOCITY_Y;
                    var newVel = new Vector2(ballVelocityX, ballVelocityY);
                    ball.GetComponent<Rigidbody2D>().velocity = newVel;
                }
                photonView.RPC("SetTouchedBallLast", RpcTarget.All, PhotonManager.id);
            }

        }
    }
    [PunRPC]
    void SetTouchedBallLast(int id) {
        touchedBallLast = id;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            if (touchedBallLast == PhotonManager.id && Mathf.Sign(ball.transform.position.x) == Mathf.Sign(transform.position.x)) {
                stream.SendNext(ball.transform.position);
                stream.SendNext(ballRigidBody.velocity);
            }
        } else {
            if (master.ballRigidBody.gravityScale == 0)
                return;
            Vector3 pos = (Vector3)stream.ReceiveNext();
            Vector2 vel = (Vector2)stream.ReceiveNext();
            ball.transform.position = pos;
            ballRigidBody.velocity = vel;
        }
    }
}
