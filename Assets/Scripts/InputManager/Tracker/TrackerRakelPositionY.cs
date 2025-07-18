using UnityEngine;
public class TrackerRakelPositionY : FloatValueSource
{
    private Vector3 _top = GameObject.Find("Top").transform.position;
    private Vector3 _bot = GameObject.Find("Bottom").transform.position;
    private readonly RakelLineRenderer _lineRenderer = GameObject.Find("LineRenderer").GetComponent<RakelLineRenderer>();
    public override void Update()
    {
        _top = GameObject.Find("Top").transform.position;
        _bot = GameObject.Find("Bottom").transform.position;
        Vector3 center = (_top + _bot) / 2f;
        
        Value = (center.y + _lineRenderer.offsetY) * _lineRenderer.multY;
    }
}