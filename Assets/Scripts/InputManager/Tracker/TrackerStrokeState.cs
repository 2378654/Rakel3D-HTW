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
    
    
    
    public override void Update()
    {
        rakelpositionZ = GameObject.Find("RenderedRakel").transform.localPosition.z;
        canvaspositionZ = GameObject.Find("Wall").transform.localPosition.z;
        
        //Check if Rakel on Wall
        if (rakelpositionZ >= canvaspositionZ &&
            !GraphicsRaycaster.UIBlocking(GameObject.Find("RenderedRakel").transform.position)) //0.085f
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
    

