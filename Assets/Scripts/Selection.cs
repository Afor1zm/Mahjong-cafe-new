using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public Renderer _component;
    public bool Selective = false;
    // Start is called before the first frame update
    void Start()
    {
        _component = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Selective == true)
        {
            _component.materials[0].color = new Color(0.55f, 1f, 0.55f);
            _component.materials[1].color = new Color(0.55f, 1f, 0.55f);
        }
        else
        {
            _component.materials[0].color = new Color(1f, 1f, 1f);
            _component.materials[1].color = new Color(1f, 1f, 1f);
        }
    }
}
