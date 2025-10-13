using UnityEngine;

enum Claw { Up, Down, Idle, Grab};
public class Move : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] Input inputSystem;
    [SerializeField] Transform claw;
    bool prevPressed = false;
    //bool goDown = false;
    public float goingDownPeriod = 5.0f;
    float currentTime = 0.0f;
    Claw currentState = Claw.Idle;
    bool clawPressed = false;


    void Update()
    {
        bool currentlyPressed = inputSystem.Pressed;
        Vector3 dir = new Vector3(inputSystem.direction.x, 0, inputSystem.direction.y);

        //If claw is pressed once, you cannot press it again, unless it is back to its orignal y position
        if (currentlyPressed && !prevPressed)
        {
            currentState = Claw.Down;
        }

        float deltaTime = Time.deltaTime;
        float distance = speed * deltaTime;

        //Goes down then up, but ideally should be down -> grab (atleast tries) -> up -> idle 
        if (currentState == Claw.Down)
        {
            currentTime += deltaTime;
            //Debug.Log("claw state: down" );
            dir.y = -1;
            if (currentTime > goingDownPeriod)
            {

                ResetClawState(Claw.Grab);
            }
        }
        else if (currentState == Claw.Grab)
        {
            currentTime += deltaTime;
            dir.y = 0;
            if (currentTime > goingDownPeriod)
            {
                ResetClawState(Claw.Up);
            }
        }
        else if (currentState == Claw.Up)
        {
            currentTime += deltaTime;
            dir.y = 1;
            if (currentTime > goingDownPeriod)
            {
                ResetClawState(Claw.Idle);
            }
        }
        else
        {
            dir.y = 0;
        }

        this.transform.position += dir * distance;
        prevPressed = currentlyPressed;
    }

    void ResetClawState(Claw nextState)
    {
        //Debug.Log("claw transition");
        currentState = nextState;
        currentTime = 0.0f;
    }
}
