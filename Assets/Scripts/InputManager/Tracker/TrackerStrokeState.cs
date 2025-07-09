using UnityEngine;

public class TrackerStrokeState : StrokeStateSource
{
    private BoxCollider _boxColliderIndikator;
    private MeshCollider _meshColliderCanvas;
    private GraphicsRaycaster GraphicsRaycaster;
    private ButtonInteraction _interaction;
    private OilPaintEngine _oilpaintengine;
    private GameObject _top, _bot;
    private DistanceToCanvas _distanceToCanvas;
    public TrackerStrokeState()
    {
        _boxColliderIndikator = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
        GraphicsRaycaster = GameObject.Find("UI").GetComponent<GraphicsRaycaster>(); ;
        _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
        _oilpaintengine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        _top = GameObject.Find("Top");
        _bot = GameObject.Find("Bottom");
        _distanceToCanvas =  GameObject.Find("DistanceController").GetComponent<DistanceToCanvas>();
    }

    public override void Update()
    {
        _meshColliderCanvas = GameObject.Find("Canvas").GetComponent<MeshCollider>();
        float currentOffset = _distanceToCanvas.canvasOffset;
        float rakelpositionZ = _boxColliderIndikator.transform.position.z + currentOffset;
        float _canvaspositionZ = _meshColliderCanvas.transform.position.z;
        float pressure = _interaction.GetPressure();
        bool isCurrentlyInStroke = rakelpositionZ > _canvaspositionZ && pressure > 0;
        
        StrokeBegin = !InStroke && isCurrentlyInStroke;

        if (StrokeBegin)
        {
            _oilpaintengine.BackupStroke();
        }
        
        InStroke = isCurrentlyInStroke;

    }
}