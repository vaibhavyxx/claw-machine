using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] Input inputSystem;
    bool prevPressed = false;
    bool goDown = false;
    public float goingDownPeriod = 5.0f;
    float currentTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        bool currentlyPressed = inputSystem.Pressed;
        Vector3 dir = new Vector3(inputSystem.direction.x, 0, inputSystem.direction.y);

        if(currentlyPressed && !prevPressed)
        {
            goDown = true;
        }
        float deltaTime = Time.deltaTime;
        float distance = speed * deltaTime;
        currentTime += deltaTime;

        if (goDown)
        {
            dir.y = -1;
            if(currentTime > goingDownPeriod)
            {
                goDown = false;
                currentTime = 0.0f;
            }
        }
        this.transform.position += dir * distance;
        //rb.linearVelocity = inputSystem.direction * speed;

        prevPressed = currentlyPressed;
    }
}
