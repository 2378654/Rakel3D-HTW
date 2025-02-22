using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToCanvas : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Update()
    { 
       float rakel_Z = GameObject.Find("RenderedRakel").transform.position.z;
       float canvas_Z = GameObject.Find("Wall").transform.position.z;
       float distance = canvas_Z - rakel_Z;

       if (distance <= 0)
       {
           text.SetText("Rakel on Wall");
       }
       else
       {
           text.SetText("Current Distance to Canvas: " + distance);
       }
       
    }
}