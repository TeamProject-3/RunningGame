using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    
    [SerializeField] float speed = 3.0f;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(0.01f,0,0);
    }
}
