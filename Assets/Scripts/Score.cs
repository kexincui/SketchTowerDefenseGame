using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int health=3;
    public int numOfStars;

    public Image[] stars;
    public Sprite fullStar;
    public Sprite emptyStar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health > numOfStars) {
            health = numOfStars;
        }
        for (int i = 0; i < stars.Length; i++) {
            if (i < health) {
                stars[i].sprite = fullStar;
            } else {
                stars[i].sprite = emptyStar;
            }
            if (i < numOfStars) {
                stars[i].enabled = true;
            } else {
                stars[i].enabled = false;
            }
        }
    }
}
