using TMPro;
using UnityEngine;

public class TrackerStrokeState : StrokeStateSource
{
    private BoxCollider _boxColliderIndikator = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
    private MeshCollider _meshColliderCanvas;
    private GraphicsRaycaster GraphicsRaycaster;
    private ButtonInteraction _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
    private DistanceToCanvas _distanceToCanvas;
    private GameObject _renderedRakel;
    private TextMeshProUGUI _text;
    private OilPaintEngine _oilPaintEngine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
    public TrackerStrokeState()
    {
        GraphicsRaycaster = GameObject.Find("UI").GetComponent<GraphicsRaycaster>(); ;
        _renderedRakel = GameObject.Find("RenderedRakel");
        _distanceToCanvas =  GameObject.Find("DistanceController").GetComponent<DistanceToCanvas>();
        _text = GameObject.Find("PressureText").GetComponent<TextMeshProUGUI>();
    }
        
    private bool _wasPreviouslyInStroke;
    public override void Update()
    {
        float canvaspositionZ = GameObject.Find("Canvas").GetComponent<MeshCollider>().transform.position.z;
		float currentOffset = _distanceToCanvas.canvasOffset;
        float rakelpositionZ = _boxColliderIndikator.transform.position.z + currentOffset;
		float pressure = _interaction.GetPressure();
        
        
        bool isTouchingCanvas;
        if (_interaction.WallController)
        {
            isTouchingCanvas = rakelpositionZ > canvaspositionZ;
        }
        else
        {
            isTouchingCanvas = rakelpositionZ > canvaspositionZ && pressure > 0;
        }
        
        bool isBlockedByUI = GraphicsRaycaster.UIBlocking(_renderedRakel.transform.position);
        bool isCurrentlyInStroke = isTouchingCanvas && !isBlockedByUI;

        if (_interaction.WallController)
        {
            if (rakelpositionZ > canvaspositionZ)
            {
                StrokeBegin = !_wasPreviouslyInStroke && isCurrentlyInStroke;
                if (StrokeBegin)
                {
                    InStroke = true;
                }
            }

            if (rakelpositionZ < canvaspositionZ)
            {
                InStroke = false;
            }
            //InStroke = isCurrentlyInStroke;
            _wasPreviouslyInStroke = isCurrentlyInStroke;
        }
        else
        {
            if (rakelpositionZ > canvaspositionZ && pressure > 0)
            {
                StrokeBegin = !_wasPreviouslyInStroke && isCurrentlyInStroke;
                if (StrokeBegin)
                {
                    InStroke = true;
                }
            }

            if (rakelpositionZ < canvaspositionZ || pressure < 0)
            {
                InStroke = false;
            }
            //InStroke = isCurrentlyInStroke;
            _wasPreviouslyInStroke = isCurrentlyInStroke;
        }
    }
}