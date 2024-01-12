using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Perception : MonoBehaviour
{
    [SerializeField] protected string tagName = "";
    [SerializeField] protected float distance = 1;
    [SerializeField] protected float maxAngle = 45;

    public string TagName {get {return tagName;}}
    public float Distance {get {return distance;}}
    public float MaxAngle {get {return maxAngle;}}


    public abstract GameObject[] GetGameObjects();
}
