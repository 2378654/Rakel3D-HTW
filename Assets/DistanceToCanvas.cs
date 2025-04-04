using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToCanvas : MonoBehaviour
{
    public TextMeshProUGUI text;
    private LineRenderer _line;

    private void Start()
    {
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
    }
    
    private BoxCollider _boxColliderIndikator;
    private MeshCollider _meshColliderCanvas;
    void Update()
    { 
        _boxColliderIndikator = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
        _meshColliderCanvas = GameObject.Find("Canvas").GetComponent<MeshCollider>();
        
        float rakel_Z = (_boxColliderIndikator.transform.position.z + 1.38f);
        float canvas_Z = _meshColliderCanvas.transform.position.z;
        float distance = canvas_Z - rakel_Z;
       
        //Debug.Log("Rakel Z:" + rakel_Z);
        if (distance <= 0)
        {
            _line.enabled = true;
            text.SetText("Rakel on Wall");
        }
        else if (distance < 1)
        {
            _line.enabled = true;
            text.SetText("Current Distance to Canvas: " + distance);
        }
        else
        {
            _line.enabled = false;
            text.SetText("Current Distance to Canvas: " + distance);
        }
       
       
    }
}