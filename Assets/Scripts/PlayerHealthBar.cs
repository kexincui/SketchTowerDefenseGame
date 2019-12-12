using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public int lives;
    public int maxLives;

    public void UpdateHealth(int currLives) {
        lives = currLives;
        float ratio = lives;
        ratio /= maxLives;
        transform.localScale = new Vector3(ratio, 1, 1);
    }
}
