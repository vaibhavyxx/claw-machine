using UnityEngine;


public class Move : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] Input inputSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(inputSystem.direction.x, 0, inputSystem.direction.y);
        float deltaTime = Time.deltaTime;
        float distance = speed * deltaTime;
        this.transform.position += dir * distance;
        //rb.linearVelocity = inputSystem.direction * speed;
    }
}
