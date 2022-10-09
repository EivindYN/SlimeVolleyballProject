using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviourPun
{
    private Rigidbody2D rigidBody2D;

    public float jumptimer = 0.5f;
    public float jumpduration = 0.5f;
    float startY;
    public float x;
    float y;

    public GameObject ball;

    public bool carefulServing;

    public float[] limits;

    float speed;

    public static bool disabled;

    public float velocityX;
    public float velocityY;

    SlimePlayer slimePlayer;


    // Start is called before the first frame update
    void Start() {
        rigidBody2D = GetComponent<Rigidbody2D>();
        startY = transform.position.y;
        speed = GetComponent<SlimePlayer>().speed;
        slimePlayer = GetComponent<SlimePlayer>();
    }

    // Update is called once per frame
    void Update() {
        if (disabled)
            return;

        bool jump = Util.pressedJump(slimePlayer.playerID);
        bool left = Util.pressingLeft(slimePlayer.playerID);
        bool right = Util.pressingRight(slimePlayer.playerID);
        if (carefulServing) {
            left = false;
            right = false;
            bool pressedLeft = Util.pressedLeft(slimePlayer.playerID);
            bool pressedRight = Util.pressedRight(slimePlayer.playerID);
            if (pressedLeft || pressedRight)
                carefulServing = false;
        }

        y = 0;
        if (jumptimer < jumpduration) {
            jumptimer += Time.deltaTime;
            if (jumptimer > jumpduration)
                jumptimer = jumpduration;
            float t = jumptimer;
            y = 12.5f * t - 25f * t * t;
        } else if (jump){
            jumptimer = 0;
            velocityY = 0;
        }
            
        x = 0;
        x -= left ? speed * Time.deltaTime: 0;
        x += right ? speed * Time.deltaTime: 0;

        float newX = (transform.position.x + x).Between(limits[0], limits[1]);
        float newY = startY + y;
        velocityX = (newX - transform.position.x) / Time.deltaTime;
        velocityY = (newY - transform.position.y) / Time.deltaTime;
        transform.position = new Vector2(newX, newY);

        if (ball)
            GetComponent<CircleCollider2D>().enabled = ball.transform.position.y > transform.position.y;
    }
}
