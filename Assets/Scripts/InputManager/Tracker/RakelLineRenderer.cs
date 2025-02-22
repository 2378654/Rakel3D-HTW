using CodiceApp;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RakelLineRenderer : MonoBehaviour
{   
    private float canvaspositionZ;
    private float rakelpositionX;
    private float rakelpositionY;
    private float rakelpositionZ;
    
    private OilPaintEngine OilPaintEngine;
    
    private float rakelLength;
    private float rakelWidth;
    private Color_ current_color;
    private Color paint_color, no_paint_color;
    private BoxCollider _box;
    private GameObject _rakel;
    
    private LineRenderer _line;
    void Start()
    {
        rakelLength = new RakelConfiguration().Length; //Rakel Length
        rakelWidth = new RakelConfiguration().Width * 0.2f;// Rakel Width
        
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        _line.material = new Material(Shader.Find("Sprites/Default"));
        
        //Changing the Width of the LineRenderer
        _line.startWidth = rakelWidth; 
        _line.endWidth = rakelWidth;

        _box = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
        _rakel = GameObject.Find("RenderedRakel");
    }

    // Update is called once per frame
    void Update()
    {
        //Current Position (current Solution)
        float posX = (_rakel.transform.position.x + 0.41f) * 15f; //Testing
        float posY = (_rakel.transform.position.y - 1.58f) * 16f; //Testing
        float posZ = (_rakel.transform.position.z);
  
        //float posX = (_rakel.transform.position.x - 0.02f) * 4.9f; //At School
        //float posY = (_rakel.transform.position.y - 1.57f) * 6f; //At School
        
        Vector3 center = new Vector3(posX,posY,0); // CenterPosition (Anchor Point)
        Vector3 up = _rakel.transform.up; // Vector for Rotation
            
        //Startpoint and Endpoint
        Vector3 startPoint = center - (up * (rakelLength / 2)); 
        Vector3 endPoint = center + (up * (rakelLength / 2));  

        //Update LineRenderer Position
        _line.SetPosition(0, startPoint);  //Startpoint
        _line.SetPosition(1, endPoint);    //Endpoint
        
        _box.size = new Vector3(_box.size.x, _box.size.y, 3.65f);
        _box.transform.position = new Vector3(posX,posY,posZ-1);
        _box.transform.rotation = new Quaternion();

        
        OilPaintEngine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        current_color = OilPaintEngine.GetCurrentColor();
        
        Vector3 color_vector = Colors.GetColor(current_color);
        
        paint_color = new Color(color_vector.x, color_vector.y, color_vector.z, 1.0f);
        no_paint_color = new Color(color_vector.x * 0.5f, color_vector.y* 0.5f, color_vector.z* 0.5f, 1.0f);
        Debug.Log(paint_color);
        rakelpositionZ = _rakel.transform.localPosition.z;
        canvaspositionZ = GameObject.Find("Wall").transform.localPosition.z;
        
        //Check if Rakel is on Wall --> show line renderer with selected color
        if (rakelpositionZ >= canvaspositionZ) //0.085f
        {
            //GameObject.Find("DistanceToCanvas").GetComponent<TextMeshProUGUI>().color = paint_color;
            _line.enabled = true;
            _line.startColor = paint_color;
            _line.endColor = paint_color;
        }
        else
        {
            // turn line renderer of when to far away, preventing confusion
            _line.enabled = false;
            //GameObject.Find("DistanceToCanvas").GetComponent<TextMeshProUGUI>().color = Color.black;
        }
    }
}
