using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.XR;

public class TrackerStrokeState : StrokeStateSource
{
    private GraphicsRaycaster GraphicsRaycaster;
    
    public TrackerStrokeState()
    {
        GraphicsRaycaster = GameObject.Find("UI").GetComponent<GraphicsRaycaster>(); ;
        
    }
    private float rakelpositionX;
    private float rakelpositionY;
    private float rakelpositionZ;
    private float canvaspositionX;
    private float canvaspositionY;
    private float canvaspositionZ;
    private float objectLength;
        
    private LineRenderer _line;
    
    public override void Update()
    {
        objectLength = new RakelConfiguration().Length;
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        
        rakelpositionZ = GameObject.Find("RenderedRakel").transform.localPosition.z;
        canvaspositionZ = GameObject.Find("Wall").transform.localPosition.z;
        //canvaspositionZ = GameObject.Find("Wall").GetComponent<Rigidbody>().transform.localPosition.z;

        //rakelpositionX = (GameObject.Find("RenderedRakel").transform.position.x + 0.45f) * 15f;
        //rakelpositionY = (GameObject.Find("RenderedRakel").transform.position.y - 1.58f) * 16f;
        
        // Berechne die beiden Endpunkte
        float posX = (GameObject.Find("RenderedRakel").transform.position.x + 0.45f) * 15f;
        float posY = (GameObject.Find("RenderedRakel").transform.position.y - 1.58f) * 16f;
        
        // Hol die aktuelle Position und Rotation des Objekts
        Vector3 center = new Vector3(posX,posY,-5); // aktueller Mittelpunkt
        Vector3 forward = GameObject.Find("RenderedRakel").transform.up; // aktueller Vorwärts-Vektor basierend auf Rotation

        
        Vector3 startPoint = center - (forward * (objectLength / 2)); // Startpunkt
        Vector3 endPoint = center + (forward * (objectLength / 2));   // Endpunkt

        // Aktualisiere die Positionen des LineRenderers
        _line.SetPosition(0, startPoint);  // Start der Linie
        _line.SetPosition(1, endPoint);    // Ende der Linie
        if (rakelpositionZ >= canvaspositionZ &&
            !GraphicsRaycaster.UIBlocking(GameObject.Find("RenderedRakel").transform.position))
        {
            StrokeBegin = true;
            if (StrokeBegin)
            {
                InStroke = true;
            }
        }
        else
        {
            InStroke = false;
        }
    }
}
    

