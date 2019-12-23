using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;      //Utilizar el codigo de pathfinding

public class EnemyAI : MonoBehaviour
{

    public Transform target;                        //Quien es el objetivo

    public float speed =  200f;                     //Velocidad de movimiento
    public float nextWaypointDistance = 3f;         //Que tan cerca se debe estar a un Waypoint hasta que se mueva al proximo
    public Transform enemyGFX;

    Path path;                                      //Camino Actual
    int currentWaypoint = 0;                        //Waypoint actual
    bool reachedEndOfPath = false;                   //Si llegamos al final del camino

    Seeker seeker;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath(){
        if(seeker.IsDone()){
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
        }    
    }

    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)            //Asegurarse de que hay un camino
            return;

        if (currentWaypoint >= path.vectorPath.Count){ //Asegurarse de que hay mas Waypoints en el camino
            reachedEndOfPath = true;
            return;
        }else {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;
        Vector2 force  = direction * speed * Time.deltaTime;

        rb2d.AddForce(force);

        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance){
            currentWaypoint ++;
        }

        if(force.x >= 0.01f){
            enemyGFX.localScale = new Vector3(-1f,1f,1f);
        } else if (force.x <= -0.01f){
            enemyGFX.localScale = new Vector3(1f,1f,1f);
        }
    }
}
