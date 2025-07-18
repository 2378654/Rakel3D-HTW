using TMPro;
using UnityEngine;
public class TrackerRakelPositionX : FloatValueSource
{
    
    private float _rakelPositionX;
    private Transform _top = GameObject.Find("Top").transform;
    private Transform _bot = GameObject.Find("Bottom").transform;
    private float _newOffsetX;
    private readonly RakelLineRenderer _lineRenderer = GameObject.Find("LineRenderer").GetComponent<RakelLineRenderer>();
    public override void Update()
    {
        Vector3 _topPosition = _top.position;
        Vector3 _botPosition = _bot.position;

        Vector3 center = (_topPosition + _botPosition) / 2f;
        
        //Because of the camera angle we probably need to check the x position and calculate the x mult of that
        //otherwise the position of the stroke will be different
        
        //Lerping betweeen offset range depending on the current position
        _newOffsetX = Mathf.Lerp(_lineRenderer.offsetX - 0.05f, _lineRenderer.offsetX + 0.05f, _lineRenderer.transform.position.x);

        //if to far to the left we need to change the offset 
        //cant in Lerp because it would change the stroke position in other positions
        if (_lineRenderer.transform.position.x < -9)
        {
            _newOffsetX = 0.05f;
        }
        
        
        Value = (center.x + _newOffsetX) * _lineRenderer.multX;
    }
}