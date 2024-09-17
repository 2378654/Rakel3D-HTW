using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RakelLineRenderer : MonoBehaviour
{
    private float canvaspositionZ;
    private float rakelpositionX;
    private float rakelpositionY;
    private float rakelpositionZ;
    
    private float rakelLength;
    private float rakelWidth;
    public Material paint, noPaint;
        
    private LineRenderer _line;
    void Start()
    {
        rakelLength = new RakelConfiguration().Length; //Rakel Length
        rakelWidth = new RakelConfiguration().Width;// Rakel Width
        
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
       
        //Changing the Width of the LineRenderer
        _line.startWidth = rakelWidth; 
        _line.endWidth = rakelWidth;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Current Position
        float posX = (GameObject.Find("RenderedRakel").transform.position.x + 0.41f) * 15f;
        float posY = (GameObject.Find("RenderedRakel").transform.position.y - 1.58f) * 16f; 
        
        
        Vector3 center = new Vector3(posX,posY,-1); // CenterPosition (Anchor Point)
        Vector3 up = GameObject.Find("RenderedRakel").transform.up; // Vector for Rotation

        //Startpoint and Endpoint
        Vector3 startPoint = center - (up * (rakelLength / 2)); 
        Vector3 endPoint = center + (up * (rakelLength / 2));  

        //Update LineRenderer
        _line.SetPosition(0, startPoint);  // Start der Linie
        _line.SetPosition(1, endPoint);    // Ende der Linie
        
        
        rakelpositionZ = GameObject.Find("RenderedRakel").transform.localPosition.z;
        canvaspositionZ = GameObject.Find("Wall").transform.localPosition.z;
        
        //Check if Rakel is on Wall
        if (rakelpositionZ >= canvaspositionZ)
        {
            _line.material = paint;
        }
        else
        {
            _line.material = noPaint;
        }
    }
}
