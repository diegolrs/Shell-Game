using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject ball;

    // Variables used on Exchange position
    private GameObject cup1, cup2;
    private Rigidbody2D rb, rb2; 
    private float initialx1, initialx2;
    private int dir = 1; // Set direction of move
    private bool config = false; // Set the cups to exchange
    private string cur1, cur2; // Get the name of the objects

    // Set speed based in level
    private float initialSpeed, speed;
    private Vector3 nullVelocity, initialVelocity;

    private bool isChanging = false; // See if the move its still being doing
    private int initialLevel, level, curMove = 0; // Set initial speed and quant of moves
    private int numberOfCups; // Set number of cups on game
    public string state; // state of the game
    private int frames = 0, maxFrames = 60*3; // set delay
    private float initialY, showAnswerY; // set y position when shows answer

    public int WAITING_ANSWER, WRONG_ANSWER, RIGHT_ANSWER; // States for answer
    public int choose; // See the choosen choose
    void Start()
    { 
        ball = GameObject.FindGameObjectWithTag("Ball");
        numberOfCups = 3;

        initialY = GameObject.FindGameObjectWithTag("CupMain").transform.position.y;
        showAnswerY = initialY + 2;

        initialLevel = 5;
        level = initialLevel;
        initialSpeed = (float) initialLevel;
        speed = initialSpeed + level;
        initialVelocity = new Vector3(speed,0,0);
        nullVelocity = new Vector3(0,0,0);

        WAITING_ANSWER = -1;
        WRONG_ANSWER = 0;
        RIGHT_ANSWER = 1;
        choose = WAITING_ANSWER;
        state = "INITIAL";
    }

    void OnMouseDown()
    {
        if(state == "CLICK"){
            if(Input.GetMouseButtonDown(0)){
                choose = RIGHT_ANSWER;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = initialSpeed + level;
        initialVelocity = new Vector3(speed,0,0);
        Debug.Log(speed);

        if(state == "INITIAL") {
            frames++;
            float curX = GameObject.FindGameObjectWithTag("CupMain").transform.position.x;
            GameObject.FindGameObjectWithTag("CupMain").transform.position = new Vector2(curX, showAnswerY);
            setBally(0);

            // Delay to start move after show the ball
            if(frames >= maxFrames - (int)(maxFrames/3) ){
                GameObject.FindGameObjectWithTag("CupMain").transform.position = new Vector2(curX, initialY);
                setBally(20);
            }

            if(frames >= maxFrames){
                state = "NORMAL";
                frames = 0;
            }
        }


        if(state == "NORMAL"){

            if(isChanging)
                ExchangePosition(cur1, cur2);
            else
                resetVelocity();

            if(!isChanging && curMove < level){
                    int c1 = Random.Range(0, numberOfCups);
                    int c2 = Random.Range(0, numberOfCups);

                    while(c1 == c2)
                        c1 = Random.Range(0,3);

                    cur1 = "Cup" + c1;
                    cur2 = "Cup" + c2;

                    config = true;
                    isChanging = true;
                    curMove++;       
            }  

            if(curMove == level){
                state = "CLICK"; // Wait for answer
            }
        }  

        if(choose != WAITING_ANSWER)
        {
            frames++;

            float curX = GameObject.FindGameObjectWithTag("CupMain").transform.position.x;
            GameObject.FindGameObjectWithTag("CupMain").transform.position = new Vector2(curX, showAnswerY);
            setBally(0);

            // Delay to start move after show the ball
            if(frames >= maxFrames - (int)(maxFrames/3) ){
                GameObject.FindGameObjectWithTag("CupMain").transform.position = new Vector2(curX, initialY);
                setBally(20);
            }

            if(frames >= maxFrames)
            {
                if(choose == RIGHT_ANSWER)
                {
                    level++;
                    curMove = 0;
                    state = "NORMAL";
                    choose = WAITING_ANSWER;     
                    GameObject.Find("Canvas").GetComponent<Score>().score++;  
                    changeZ();          
                } else if(choose == WRONG_ANSWER)
                {
                    level = initialLevel;
                    curMove = 0;
                    state = "NORMAL";
                    choose = WAITING_ANSWER;
                    GameObject.Find("Canvas").GetComponent<Score>().score = 0;
                    changeZ(); 
                }

                frames = 0;
            }

        }

    }

    // Used to fix bug that cup with ball is allways on the front
    void changeZ(){
        float tempx = GameObject.FindGameObjectWithTag("CupMain").transform.position.x;
        float tempy = GameObject.FindGameObjectWithTag("CupMain").transform.position.y;
        float tempz = (float) Random.Range(0,3);
        
        GameObject.FindGameObjectWithTag("CupMain").transform.position = new Vector3(tempx, tempy, tempz);
    }

    // Setting velocity to null
    void resetVelocity()
    {  
        GameObject.Find("Cup0").GetComponent<Rigidbody2D>().velocity = nullVelocity;
        GameObject.Find("Cup1").GetComponent<Rigidbody2D>().velocity = nullVelocity;
        GameObject.Find("Cup2").GetComponent<Rigidbody2D>().velocity = nullVelocity;  
    }
    void Config(string name1, string name2) 
    {
        cup1 = GameObject.Find(name1); 
        cup2 = GameObject.Find(name2);

        // Get the inital position
        initialx1 = cup1.transform.position.x;
        initialx2 = cup2.transform.position.x;

        rb = cup1.GetComponent<Rigidbody2D>();
        rb2 = cup2.GetComponent<Rigidbody2D>(); 
        
        changeZ();
        resetVelocity();
    }

    void ExchangePosition(string name1, string name2) 
    {    

        if(config) {
           Config(name1, name2);
           config = false;
        }

        // current x
        float x = cup1.transform.position.x;

        // See if is close enough from the other
        float distanceToGoal = x - initialx2;
            if(distanceToGoal < 0)
                distanceToGoal *= -1;

        // Fix floating point's bug
        if(distanceToGoal < 0.2)
        {
            cup1.transform.position = new Vector3 (initialx2, transform.position.y, transform.position.z);
            cup2.transform.position = new Vector3 (initialx1, transform.position.y, transform.position.z);
       
            isChanging = false;
           
            dir = 0;
            rb.velocity = nullVelocity;
            rb2.velocity = nullVelocity;

            // Update local variable x with the new value
            // We should do that because x is verified again
             x = cup1.transform.position.x;
        }

        // Set the directions
        if(x > initialx2) 
            dir = -1;
        else if( x < initialx2) 
            dir = 1;
        else 
            dir = 0;

        if(dir == 0){
            resetVelocity();
        } else {
            rb.velocity = initialVelocity * dir;
            rb2.velocity = initialVelocity * -1*dir;
              // Apply force
            rb.AddForce ( new Vector2 (speed, 0));
            rb2.AddForce ( new Vector2 (speed, 0));
        }

        ChangePositionBall();
    }

    void setBally(float newY){
        float tempx = ball.transform.position.x;
        float tempz = ball.transform.position.z;

        ball.transform.position = new Vector3(tempx, newY,tempz);
    }

    // Change y from ball to bug fix in high speed
    void ChangePositionBall()
    {
        float xcupMain = GameObject.FindGameObjectWithTag("CupMain").transform.position.x;
        ball.transform.position = new Vector3(xcupMain, ball.transform.position.y,10);
    }
}
