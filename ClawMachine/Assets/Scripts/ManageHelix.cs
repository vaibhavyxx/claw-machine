using UnityEngine;

public class ManageHelix : MonoBehaviour
{
    [SerializeField] RenderHelix helix;
    [SerializeField] Transform start, end;  //for cable points
    public Vector3 direction;
    Vector3 prevStart;

    float length = 0.0f;
    bool set = false;
    void Start()
    {
        Vector3 delta = start.position - end.position;
        length = Vector3.Magnitude(delta);
        direction = Vector3.Normalize(delta);
       // helix.coilWidth = length/helix.coilsCount;
    }

    void Update()
    {

        if (helix.coilsCount > 0 && start.position != prevStart)
        {
            set = true;
            helix.pos = start.position;
            helix.coilWidth = length / helix.coilsCount;
        }
        prevStart = start.position;
    }
}
