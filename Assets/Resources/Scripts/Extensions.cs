using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static float Between(this float num, float min, float max) {
        return Mathf.Max(Mathf.Min(num, max), min);
    }
}
