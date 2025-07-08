using UnityEngine;

public class TrackerStrokeState : StrokeStateSource
{
    private BoxCollider _boxColliderIndikator;
    private MeshCollider _meshColliderCanvas;
    private GraphicsRaycaster GraphicsRaycaster;
    private ButtonInteraction _interaction;
    private OilPaintEngine _oilpaintengine;
    private GameObject _top, _bot;
    public TrackerStrokeState()
    {
        _boxColliderIndikator = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
        GraphicsRaycaster = GameObject.Find("UI").GetComponent<GraphicsRaycaster>(); ;
        _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
        _oilpaintengine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        _top = GameObject.Find("Top");
        _bot = GameObject.Find("Bottom");
    }

    public override void Update()
    {
        /*float _rakelpositionZ = _boxColliderIndikator.transform.position.z + 1.2f;
        _meshColliderCanvas = GameObject.Find("Canvas").GetComponent<MeshCollider>();
        float pressure = _interaction.GetPressure();
        float _canvaspositionZ = _meshColliderCanvas.transform.position.z;
        
        if (_rakelpositionZ > _canvaspositionZ && pressure > 0)
        {
            StrokeBegin = _rakelpositionZ > _canvaspositionZ && pressure > 0 && !GraphicsRaycaster.UIBlocking((_top.transform.position + _bot.transform.position)/2);
            if (StrokeBegin)
            {
                InStroke = true;
            }
        }

        if (_rakelpositionZ < _canvaspositionZ)
        {
            InStroke = false;
        }*/
        _meshColliderCanvas = GameObject.Find("Canvas").GetComponent<MeshCollider>();
        float rakelpositionZ = _boxColliderIndikator.transform.position.z + 1.25f;
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