using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Master : MonoBehaviourPun {
    public GameObject[] playersArray = new GameObject[4];
    public List<GameObject> playerEyes = new List<GameObject>();

    public GameObject resultText;
    public GameObject scorePanels;
    public GameObject background;
    public GameObject winningPanel;
    public GameObject winningText;
    public GameObject winningDescription;
    public GameObject manaUI;
    public Goal goal;

    public Sprite scoreEmpty;
    public Sprite scoreFull;
    private int[] points = new int[2];

    public bool playerWon;

    private int voteResetCount = 0;
    public int lastWinner;

    private GameObject _ball;
    [HideInInspector]
    public GameObject ball {
        get {
            if (_ball == null)
                _ball = GameObject.FindGameObjectWithTag("Ball");
            return _ball;
        }
    }
    private Rigidbody2D _ballRigidBody;
    [HideInInspector]
    public Rigidbody2D ballRigidBody {
        get {
            if (_ballRigidBody == null)
                _ballRigidBody = ball.GetComponent<Rigidbody2D>();
            return _ballRigidBody;
        }
    }

    void Update() {
        if (ball) {
            foreach (GameObject playerEye in playerEyes) {
                if (playerEye != null)
                    playerEye.transform.rotation = Quaternion.LookRotation(Vector3.forward, ball.transform.position - playerEye.transform.position);
            }
        }
        if (playerWon && Input.GetKeyDown(KeyCode.Space)) {
            winningText.GetComponent<Text>().text = "Restarting...";
            winningDescription.GetComponent<Text>().text = "waiting for other player(s)";
            photonView.RPC("VoteReset", RpcTarget.All);
        }
    }

    [PunRPC]
    void VoteReset() {
        voteResetCount++;
        if (voteResetCount == PhotonManager.playerAmount) {
            Master master = GetComponent<Master>();
            master.FullReset();
        }
    }

    //Returns true if a player won
    public bool AddPoint(int player) {
        points[player]++;
        Transform scoreObject = scorePanels.transform.GetChild(player).GetChild(points[player] - 1);
        scoreObject.GetComponent<Image>().sprite = scoreFull;
        if (points[player] == scorePanels.transform.GetChild(player).childCount) {
            background.GetComponent<SpriteRenderer>().sortingOrder *= -1;
            winningText.GetComponent<Text>().text = "Player " + (player + 1) + " wins!";
            winningDescription.GetComponent<Text>().text = "Press 'space' for rematch";
            winningPanel.SetActive(true);
            playerWon = true;
            return true;
        } else {
            resultText.GetComponent<Text>().text = "Player " + (player + 1) + " scores!";
            return false;
        }
    }

    public void Reset(int winner) {
        for (int n = 0; n < playersArray.Length; n++) {
            if (playersArray[n] == null)
                continue;
            playersArray[n].transform.position = Util.playerStartPos(n);
            if (playersArray[n].GetComponent<Controller>() != null) {
                playersArray[n].GetComponent<Controller>().jumptimer = 0.5f;
                playersArray[n].GetComponent<Controller>().carefulServing = true;
            }
            if (playersArray[n].GetComponent<SlimeMagic>() != null) {
                playersArray[n].GetComponent<SlimeMagic>().mana = 100f;
            }
        }
        resultText.GetComponent<Text>().text = "";
        ball.transform.position = new Vector2(Util.playerStartPos(winner).x, 0);
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().gravityScale = Util.ballGravityScale;
        Controller.disabled = false;
        lastWinner = winner;
    }

    public void FullReset() {
        for (int n = 0; n < playersArray.Length; n++) {
            if (playersArray[n] == null)
                continue;
            playersArray[n].GetComponent<SlimeMagic>().mana = 100f;
        }

        points = new int[2];
        Reset(0);
        playerWon = false;
        voteResetCount = 0;
        background.GetComponent<SpriteRenderer>().sortingOrder = -Mathf.Abs(background.GetComponent<SpriteRenderer>().sortingOrder);
        winningPanel.SetActive(false);
        for (int n = 0; n < scorePanels.transform.childCount; n++) {
            Transform scorePanel = scorePanels.transform.GetChild(n);
            for (int i = 0; i < scorePanel.transform.childCount; i++) {
                Transform score = scorePanel.transform.GetChild(i);
                score.GetComponent<Image>().sprite = scoreEmpty;
            }
        }
        if (goal.scoreCoroutine != null)
            StopCoroutine(goal.scoreCoroutine);
    }
}
