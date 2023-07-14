using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> nearObjects = new List<GameObject>();
    public List<GameObject> ListOfNearObjects { get => nearObjects; }
}
