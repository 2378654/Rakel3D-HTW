using PlasticPipe.PlasticProtocol.Messages;
using TMPro;
using UnityEngine;

public class TrackerStrokeState : StrokeStateSource
{
    
    private BoxCollider _boxColliderIndikator = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
    private ButtonCollision _buttonCollisionIndikator = GameObject.Find("LineRenderer").GetComponent<ButtonCollision>();
    private MeshCollider _meshColliderCanvas;
    private GraphicsRaycaster GraphicsRaycaster;
    private ButtonInteraction _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
    private DistanceToCanvas _distanceToCanvas = GameObject.Find("DistanceController").GetComponent<DistanceToCanvas>();
    private TextMeshProUGUI _strokeCounter = GameObject.Find("StrokeCounter").GetComponent<TextMeshProUGUI>();
    private OilPaintEngine _oilPaintEngine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
    float canvaspositionZ = GameObject.Find("Canvas").GetComponent<MeshCollider>().transform.position.z;
    private bool _wasPreviouslyInStroke;
    private bool _isTouchingCanvas;
    private float _counter = 0;
    public override void Update()
    {

        float currentOffset = _distanceToCanvas.canvasOffset;
        float rakelpositionZ = _boxColliderIndikator.transform.position.z + currentOffset;
        float pressure = _interaction.GetPressure();
        //if (!_interaction.uiActive)
        //{
            if (_interaction.wallController)
            {
                _isTouchingCanvas = rakelpositionZ > canvaspositionZ;
            }
            else
            {
                _isTouchingCanvas = rakelpositionZ > canvaspositionZ && pressure > 0;
            }

            //bool isBlockedByUI = GraphicsRaycaster.UIBlocking(_renderedRakel.transform.position);
            bool isCurrentlyInStroke = _isTouchingCanvas; // && !isBlockedByUI;
            
            if (_interaction.wallController)
            {
                if (rakelpositionZ > canvaspositionZ)
                {
                    StrokeBegin = !_wasPreviouslyInStroke && isCurrentlyInStroke;
                    if (StrokeBegin )//&& _buttonCollisionIndikator.TouchingCanvas())
                    {
                        InStroke = true;
                        _counter++;
                        _strokeCounter.SetText(_counter.ToString());
                        _oilPaintEngine.BackupStroke();

                    }
                }
                
                if (rakelpositionZ < canvaspositionZ)// || !_buttonCollisionIndikator.TouchingCanvas())
                {
                    InStroke = false;
                }
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
                        _counter++;
                        _strokeCounter.SetText(_counter.ToString());
                        _oilPaintEngine.BackupStroke();
                    }
                }

                if (rakelpositionZ < canvaspositionZ || pressure < 0)
                {
                    InStroke = false;
                }
                _wasPreviouslyInStroke = isCurrentlyInStroke;
            }
        //}
    }
}