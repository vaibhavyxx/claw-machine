using UnityEngine;

public class ManageHelix : MonoBehaviour
{
    [SerializeField] RenderHelix helix;
    [SerializeField] Transform start, end;  //for cable points
    float length = 0.0f;
    bool set = false;
    void Start()
    {
        length = Vector3.Distance(start.position, end.position);
       // helix.coilWidth = length/helix.coilsCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (helix.coilsCount > 0 && !set)
        {
            set = true;
            helix.pos = start.position;
            helix.coilWidth = length / helix.coilsCount;
        }
    }
}
