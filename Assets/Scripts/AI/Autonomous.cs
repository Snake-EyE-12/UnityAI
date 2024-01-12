using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autonomous : Agent
{
    public Perception seekPerception;
    public Perception fleePerception;
    public Perception flockPerception;
    private void Update() {
        if(seekPerception != null) {
            var percievedObjects = seekPerception.GetGameObjects();
            if(percievedObjects.Length > 0) {
                movement.ApplyForce(Seek(percievedObjects[0]));
            }

        }
        if(fleePerception != null) {
            var percievedObjects = fleePerception.GetGameObjects();
            if(percievedObjects.Length > 0) {
                movement.ApplyForce(Flee(percievedObjects[0]));
            }

        }
        if(flockPerception != null) {
            var percievedObjects = flockPerception.GetGameObjects();
            if(percievedObjects.Length > 0) {
                movement.ApplyForce(Cohesion(percievedObjects));
                movement.ApplyForce(Seperation(percievedObjects, 3));
                movement.ApplyForce(Alignment(percievedObjects));
            }

        }


        transform.position = Utilities.Wrap(transform.position, new Vector3(-10, -10, -10), new Vector3(10, 10, 10));



        // foreach(var obj in percievedObjects) {
        //     //Debug.DrawLine(transform.position, obj.transform.position);

        // }
    }
    private Vector3 Seek(GameObject target) {
        Vector3 direction = target.transform.position - transform.position;
        return GetSteeringForce(direction);
    }
    private Vector3 Flee(GameObject target) {
        Vector3 direction = transform.position - target.transform.position;
        return GetSteeringForce(direction);
    }
    

    private Vector3 Cohesion(GameObject[] neighbors) {
        Vector3 positions = Vector3.zero;
        foreach(GameObject neighbor in neighbors) {
            positions += neighbor.transform.position;
        }
        Vector3 center = positions / neighbors.Length;
        Vector3 direction = center - transform.position;

        return GetSteeringForce(direction);
    }
    private Vector3 Seperation(GameObject[] neighbors, float radius) {
        Vector3 seperation = Vector3.zero;
        foreach(GameObject neighbor in neighbors) {
            Vector3 direction = (transform.position - neighbor.transform.position);
            if(direction.magnitude < radius) {
                seperation += direction / (direction.sqrMagnitude);
            }
        }
        Vector3 force = GetSteeringForce(seperation);
        return force;
    }
    private Vector3 Alignment(GameObject[] neighbors) {
        Vector3 velocities = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            velocities += neighbor.GetComponent<Agent>().movement.Velocity;
        }
        Vector3 avgVelocity = velocities / neighbors.Length;
        return GetSteeringForce(avgVelocity);
    }


    private Vector3 GetSteeringForce(Vector3 direction) {
        Vector3 desired = direction.normalized * movement.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.maxForce);
        return force;
    }
}
