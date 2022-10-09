using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Util : MonoBehaviour
{
    public static float ballGravityScale = 2;
    public static Vector2 ballAway = new Vector2(0, -100);
    private static float[] playerStartX = new float[] {
        -5.4f, 5.4f, -1.5f, 1.5f
    };
    public static Vector3 playerStartPos(int playerID) {
        return new Vector3(playerStartX[playerID], -3);
    }
    public static float[][] limits = new float[][] {
        new float[]{-7.96f, -0.98f}, new float[]{0.98f, 7.96f}
    };
    public static Color[] playerColors = new Color[] {
        new Color(0,1f,0), new Color(1f, 0, 0)
    };
    public static Vector2[] playerEyesLocalPosition = new Vector2[] {
        new Vector2(2.5f, 2.5f), new Vector2(-2.5f, 2.5f)
    };
    public static void Console(string txt) {
        GameObject.Find("TestText").GetComponent<Text>().text = txt;
    }
    public static bool pressedJump(int playerID) {
        if (!PhotonManager.offlineMode) {
            return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        } else {
            if (playerID == 0) {
                return Input.GetKeyDown(KeyCode.W);
            } else if (playerID == 1) {
                return Input.GetKeyDown(KeyCode.UpArrow);
            } else {
                throw new System.Exception();
            }
        }
    }
    public static bool pressingLeft(int playerID) {
        if (!PhotonManager.offlineMode) {
            return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        } else {
            if (playerID == 0) {
                return Input.GetKey(KeyCode.A);
            } else if (playerID == 1) {
                return Input.GetKey(KeyCode.LeftArrow);
            } else {
                throw new System.Exception();
            }
        }
    }
    public static bool pressingRight(int playerID) {
        if (!PhotonManager.offlineMode) {
            return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        } else {
            if (playerID == 0) {
                return Input.GetKey(KeyCode.D);
            } else if (playerID == 1) {
                return Input.GetKey(KeyCode.RightArrow);
            } else {
                throw new System.Exception();
            }
        }
    }
    public static bool pressedLeft(int playerID) {
        if (!PhotonManager.offlineMode) {
            return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        } else {
            if (playerID == 0) {
                return Input.GetKeyDown(KeyCode.A);
            } else if (playerID == 1) {
                return Input.GetKeyDown(KeyCode.LeftArrow);
            } else {
                throw new System.Exception();
            }
        }
    }
    public static bool pressedRight(int playerID) {
        if (!PhotonManager.offlineMode) {
            return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        } else {
            if (playerID == 0) {
                return Input.GetKeyDown(KeyCode.D);
            } else if (playerID == 1) {
                return Input.GetKeyDown(KeyCode.RightArrow);
            } else {
                throw new System.Exception();
            }
        }
    }

    //Code can be improved
    public static bool pressedSpell1(int playerID) {
        if (!PhotonManager.offlineMode) {
            return Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Comma);
        } else {
            if (playerID == 0) {
                return Input.GetKeyDown(KeyCode.Alpha1);
            } else if (playerID == 1) {
                return Input.GetKeyDown(KeyCode.Comma);
            } else {
                throw new System.Exception();
            }
        }
    }
    public static bool pressedSpell2(int playerID) {
        if (!PhotonManager.offlineMode) {
            return Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Period);
        } else {
            if (playerID == 0) {
                return Input.GetKeyDown(KeyCode.Alpha2);
            } else if (playerID == 1) {
                return Input.GetKeyDown(KeyCode.Period);
            } else {
                throw new System.Exception();
            }
        }
    }
    public static bool pressedSpell3(int playerID) {
        if (!PhotonManager.offlineMode) {
            return Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Minus);
        } else {
            if (playerID == 0) {
                return Input.GetKeyDown(KeyCode.Alpha3);
            } else if (playerID == 1) {
                return Input.GetKeyDown(KeyCode.Minus);
            } else {
                throw new System.Exception();
            }
        }
    }
}
