using UnityEngine;

public class TrackerStrokeState : StrokeStateSource
{
    private BoxCollider _boxColliderIndikator;
    private MeshCollider _meshColliderCanvas;
    private float _tolerance = 0.01f;
    private bool _wasInStroke = false;
    private GraphicsRaycaster GraphicsRaycaster;
    
    public TrackerStrokeState()
    {
        _boxColliderIndikator = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
        GraphicsRaycaster = GameObject.Find("UI").GetComponent<GraphicsRaycaster>(); ;
    }

    public override void Update()
    {
        float _rakelpositionZ = _boxColliderIndikator.transform.position.z + 1.1f;
        _meshColliderCanvas = GameObject.Find("Canvas").GetComponent<MeshCollider>();
        float _canvaspositionZ = _meshColliderCanvas.transform.position.z;

        /*bool isCurrentlyInStroke = rakelpositionZ > canvaspositionZ;

        StrokeBegin = !InStroke && isCurrentlyInStroke;
        if (StrokeBegin)
        {
            InStroke = true;
        }

        if (rakelpositionZ < canvaspositionZ)
        {
            InStroke = false;
        }
        */
        if (_rakelpositionZ > _canvaspositionZ)
        {
            StrokeBegin = _rakelpositionZ > _canvaspositionZ && !GraphicsRaycaster.UIBlocking(_boxColliderIndikator.transform.position);
            if (StrokeBegin)
            {
                InStroke = true;
            }
        }

        if (_rakelpositionZ < _canvaspositionZ)
        {
            InStroke = false;
        }

    }
}