using UnityEngine;

public class ArrowLookAt : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
            this.transform.LookAt(player.transform);
    }
}
