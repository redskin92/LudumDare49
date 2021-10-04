using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLookAt : MonoBehaviour
{
    public bool isReverse = true;

    private int max_Up = 10;

    private int counter = 0;

    private bool going_Up = true;

    // Update is called once per frame
    void Update()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            this.transform.LookAt(player.transform);
            /*var camera = Camera.main.gameObject.transform;
            var target_point = camera.transform.position;
            target_point.y = transform.position.y;
            var targetRotation = Quaternion.LookRotation (target_point - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Convert.ToSingle(Time.deltaTime * 2.0));
            Debug.Log("The rotation we are doin is " + targetRotation);*/
        }

       

    }
}
