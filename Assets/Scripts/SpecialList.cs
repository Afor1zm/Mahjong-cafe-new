using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialList : MonoBehaviour
{
    public List<GameObject> _usedObjects;
    public List<string> _localList;
    public GameObject [ , , ] _chipsCoordinate;
    // Start is called before the first frame update
    void Start()
    {
        _usedObjects = new List<GameObject>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
