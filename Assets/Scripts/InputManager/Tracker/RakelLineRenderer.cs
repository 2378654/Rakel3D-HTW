using CodiceApp;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RakelLineRenderer : MonoBehaviour
{   
    private float canvaspositionZ;
    
    private float _rakelpositionZ;
    
    private OilPaintEngine OilPaintEngine;
    
    private float rakelLength;
    private float rakelWidth;
    private Color_ current_color;
    private Color paint_color, no_paint_color;
    private BoxCollider _box;
    private GameObject _rakel;
    
    private LineRenderer _line;

    private float _rakelRotationX;
    private float _rakelRotationY;
    private float _rakelRotationZ;
    
    void Start()
    {
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
        rakelLength = OilPaintEngine.Config.RakelConfig.Length;
        
        Vector3 worlUp = Vector3.up;
        Vector3 rakelUp = GameObject.Find("RenderedRakel").transform.up;
        _rakelRotationZ = Vector3.SignedAngle(rakelUp, worlUp, Vector3.forward);
        _rakelRotationX = _rakel.transform.eulerAngles.x;
        _rakelRotationY = _rakel.transform.eulerAngles.y;
        
        //2 Tracker
        float top_x = GameObject.Find("TOP").transform.position.x;
        float bot_x = GameObject.Find("BOTTOM").transform.position.x;
        
        float top_y = GameObject.Find("TOP").transform.position.y + rakelLength;
        float bot_y = GameObject.Find("BOTTOM").transform.position.y;
        
        
        //Used for productive Usage
        float _offsetX = 0f;
        float _offsetY = -1.54f;
        float _offsetZ = -0.1f;
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

        //float posX = (_rakel.transform.localPosition.x + offset.x)* 8f; //8
        float posX = (top_x + bot_x) / 2;
        posX *= 8f;
        
        //float posY = (_rakel.transform.localPosition.y + offset.y)* 9.2f; //9.2
        float posY = (top_y + bot_y) / 2;
        posY = (posY - 1.54f) * 9.2f;
        
        
        float posZ = (_rakel.transform.localPosition.z + offset.z); // posZ - offset, so the buttons aren't clicked till rakel is on the wall -2.6f
        
        Vector3 center =  new (posX ,posY,posZ); // CenterPosition (Anchor Point)
        Vector3 up = _rakel.transform.up; // Vector for Rotation
        
        //Startpoint and Endpoint
        Vector3 startPoint = center - (up * (rakelLength / 2)); 
        Vector3 endPoint = center + (up * (rakelLength / 2));  
        
        Vector3 transformedStartPoint = _line.transform.TransformPoint(startPoint);
        Vector3 transformedEndPoint = _line.transform.TransformPoint(endPoint);

        
        //Update LineRenderer Position
        _line.SetPosition(0, startPoint);  //Startpoint
        _line.SetPosition(1, endPoint);    //Endpoint
        _box.transform.eulerAngles = new Vector3(0,0,-_rakelRotationZ+90);
        _box.size = new Vector3(4, _box.size.y , _box.size.z); // x, y, 3.65
        _box.transform.position = new Vector3(posX,posY,posZ); 

    }
}
