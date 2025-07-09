using UnityEngine;
public class TrackerRakelPositionX : FloatValueSource
{
    
    private float _rakelPositionX;
    private Vector3 _top = GameObject.Find("Top").transform.position;
    private Vector3 _bot = GameObject.Find("Bottom").transform.position;
    private float _rakelSideOffset = -0.05f;
    private RakelLineRenderer _lineRenderer = GameObject.Find("LineRenderer").GetComponent<RakelLineRenderer>();
    public override void Update()
    {
        _top = GameObject.Find("Top").transform.position;
        _bot = GameObject.Find("Bottom").transform.position;
        Vector3 rakelDir = (_top - _bot).normalized;
        Vector3 rakelRight = Vector3.Cross(rakelDir, Vector3.forward).normalized;
        Vector3 center = (_top + _bot) / 2f;

        center += rakelRight * _rakelSideOffset;
        
        Value = (center.x + _lineRenderer.offsetX) * _lineRenderer.multX;
    }
}