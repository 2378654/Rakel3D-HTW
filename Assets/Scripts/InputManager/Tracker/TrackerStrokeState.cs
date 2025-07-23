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
        if (!_interaction.uiActive)
        {
            _buttonCollisionIndikator = GameObject.Find("LineRenderer").GetComponent<ButtonCollision>();
            float currentOffset = _distanceToCanvas.canvasOffset;
            float rakelpositionZ = _boxColliderIndikator.transform.position.z + currentOffset;
            float pressure = _interaction.GetPressure();
        
        
            //using the FSR when using the squeegee controller so we only start painting when there is pressure
            //using the WallController we adjust the pressure via two buttons so we dont need to check if squeegee is applying pressure to wall
            //because the pressure is only managed with the buttons
            
            if (_interaction.wallController)
            {
                _isTouchingCanvas = rakelpositionZ > canvaspositionZ;
            }
            else
            {
                _isTouchingCanvas = rakelpositionZ > canvaspositionZ && pressure > 0.1f;
            }

            //bool isBlockedByUI = GraphicsRaycaster.UIBlocking(_renderedRakel.transform.position);
            bool isCurrentlyInStroke = _isTouchingCanvas; // && !isBlockedByUI;
            
            if (_interaction.wallController)
            {
                if (rakelpositionZ > canvaspositionZ)
                {
                    StrokeBegin = !_wasPreviouslyInStroke && isCurrentlyInStroke && _buttonCollisionIndikator.TouchingCanvas();
                    if (StrokeBegin)
                    {
                        Debug.Log("Stroke Begin");
                        InStroke = true;
                        _counter++; 
                        _strokeCounter.SetText(_counter.ToString());
                        _oilPaintEngine.BackupStroke();
                    }
                }
                if (rakelpositionZ < canvaspositionZ || !_buttonCollisionIndikator.TouchingCanvas())
                {
                    InStroke = false;
                }
                _wasPreviouslyInStroke = isCurrentlyInStroke;
            }
            else
            {
                if (rakelpositionZ > canvaspositionZ && pressure > 0.1f)
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

                if (rakelpositionZ < canvaspositionZ || pressure < 0.1f)
                {
                    InStroke = false;
                }
                _wasPreviouslyInStroke = isCurrentlyInStroke;
            }
        }
    }
}