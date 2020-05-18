using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score, highscore;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        highscore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(score > highscore)
            highscore = score;

        GameObject.Find("Score").GetComponent<Text>().text = "Score: " + score;
        GameObject.Find("Highscore").GetComponent<Text>().text = "Highscore: " + highscore;
    }
}
