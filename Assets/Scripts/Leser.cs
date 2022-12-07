using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leser : MonoBehaviour
{
    GameObject player;
    
    void Start()
    {
        player = GameObject.Find("player"); //finding the player
        
    }

    
    void Update()
    {
        // moves laser towards player
        GetComponent<Rigidbody>().position = Vector3.MoveTowards(transform.position, player.transform.position, 600 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
        {
        if (other.tag == "Player")   // If the laser touch  the player
            FindObjectOfType<GameManager>().HitPlayer(); 
        Destroy(gameObject);   //hide the laser after launch
        }
    }
