using UnityEngine;

public class TrackerRakelPositionX : FloatValueSource
{
    private RakelLineRenderer _lineRenderer = GameObject.Find("LineRenderer").GetComponent<RakelLineRenderer>();

    public override void Update()
    {
        Value = _lineRenderer.RakelEdgePos.x;
    }
}