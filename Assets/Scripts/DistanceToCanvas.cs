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
    private GameObject _uicover;
    public float canvasOffset;
    
    private void Start()
    {
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        _uicover = GameObject.Find("UICover");
        _boxColliderIndikator = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
    }
    
    private BoxCollider _boxColliderIndikator;
    private MeshCollider _meshColliderCanvas;
    void Update()
    { 
        _meshColliderCanvas = GameObject.Find("Canvas").GetComponent<MeshCollider>();
        float rakel_Z = (_boxColliderIndikator.transform.position.z + canvasOffset); //1.38f
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
            text.SetText("Distance: " + distance);
        }
        else if (distance < 21 && _uicover.activeSelf)
        {
            _line.enabled = true;
        }
        else
        {
            _line.enabled = false;
            text.SetText("Distance: " + distance);
        }
       
       
    }
}