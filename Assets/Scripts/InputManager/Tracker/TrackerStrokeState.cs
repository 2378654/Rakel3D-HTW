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
    
    private BoxCollider _boxColliderIndikator;
    private MeshCollider _meshColliderCanvas;

    private float _rakelpositionZ;
    private float _canvaspositionZ;
    public override void Update()
    {
        _boxColliderIndikator = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
        _meshColliderCanvas = GameObject.Find("Canvas").GetComponent<MeshCollider>();
        _rakelpositionZ = (_boxColliderIndikator.transform.position.z+1.14f);
        _canvaspositionZ = _meshColliderCanvas.transform.position.z;
        if (_rakelpositionZ > _canvaspositionZ)
        {
            StrokeBegin = true;
            if (StrokeBegin)
            {
                InStroke = true;
            }
            else
            {
                InStroke = false;
            }
        }

        if (_rakelpositionZ < _canvaspositionZ)
        {
            InStroke = false;
        }
    }
}
    

