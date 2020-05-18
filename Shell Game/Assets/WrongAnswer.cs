using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongAnswer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
  
    }
    void OnMouseDown()
    {
        GameObject cupMain = GameObject.FindGameObjectWithTag("CupMain");
        string state = cupMain.GetComponent<MoveController>().state;
        int WrongAnswer = cupMain.GetComponent<MoveController>().WRONG_ANSWER;
        
        if(state == "CLICK"){
            if(Input.GetMouseButtonDown(0)){                  
                cupMain.GetComponent<MoveController>().choose = WrongAnswer;
            }
        }
    }

}
