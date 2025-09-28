using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }

}
