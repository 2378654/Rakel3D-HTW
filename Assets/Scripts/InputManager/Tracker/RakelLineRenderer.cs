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

    private float _rakelRotationZ;
    void Start()
    {
        rakelLength = new RakelConfiguration().Length; //Rakel Length
        rakelWidth = new RakelConfiguration().Width * 0.2f;// Rakel Width
        
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.useWorldSpace = true;
        
        //Changing the Width of the LineRenderer
        _line.startWidth = rakelWidth; 
        _line.endWidth = rakelWidth;

        _box = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
        _rakel = GameObject.Find("RenderedRakel");
        
        OilPaintEngine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        //Used for Testing
        //float posX = (_rakel.transform.position.x + 0.41f) * 15f; 
        //float posY = (_rakel.transform.position.y - 1.58f) * 16f; 
        
        Vector3 world_up = Vector3.up;
        Vector3 rakel_up = GameObject.Find("RenderedRakel").transform.up;
        float _rakelRotationZ = Vector3.SignedAngle(rakel_up, world_up, Vector3.forward);
        
        //Used for productive Usage
        float _offsetX = -0.1f;
        float _offsetY = -1.55f;
        float _offsetZ =0f;
        Vector3 offset =new (_offsetX, _offsetY, _offsetZ);
        
        float _minZ = -2.6f; //-2.7
        float _maxZ = -2.56f; //-2.68

        float rakelTilt = Mathf.Abs(GameObject.Find("RenderedRakel").transform.eulerAngles.y- 180);
       
        
        if (rakelTilt > 79)
        {
            rakelTilt = 79;
        }
        
        if (rakelTilt > 90)
        {
            rakelTilt = 180-rakelTilt;
        }

        offset.z = _minZ +(rakelTilt / 79f) * (_maxZ - _minZ);

        float posX = (_rakel.transform.position.x + offset.x)* 6.1f; //At School +0.34
        float posY = (_rakel.transform.position.y + offset.y)* 7.3f; //At School -1.56
        float posZ = (_rakel.transform.position.z + offset.z); // posZ - offset, so the buttons aren't clicked till rakel is on the wall -2.6f
        
        Vector3 center =  new (posX ,posY,posZ); // CenterPosition (Anchor Point)
        Vector3 up = _rakel.transform.up; // Vector for Rotation
        
        //Startpoint and Endpoint
        Vector3 startPoint = center - (up * (rakelLength / 2)); 
        Vector3 endPoint = center + (up * (rakelLength / 2));  
        
        Vector3 transformedStartPoint = _rakel.transform.TransformPoint(startPoint);
        Vector3 transformedEndPoint = _rakel.transform.TransformPoint(endPoint);

        
        //Update LineRenderer Position
        _line.SetPosition(0, startPoint);  //Startpoint
        _line.SetPosition(1, endPoint);    //Endpoint
        _box.transform.eulerAngles = new Vector3(0,0,_rakelRotationZ+90);
        _box.size = new Vector3(4, _box.size.y , _box.size.z); // x, y, 3.65
        _box.transform.position = new Vector3(posX,posY,posZ); 

    }
}
