using UnityEngine;
public class TrackerRakelPositionY : FloatValueSource
{
    private GameObject _top = GameObject.Find("Top");
    private GameObject _bot = GameObject.Find("Bottom");
    private readonly RakelLineRenderer _lineRenderer = GameObject.Find("LineRenderer").GetComponent<RakelLineRenderer>();
    public override void Update()
    {
        Vector3 _topPosition = _top.transform.position;
        Vector3 _botPosition = _bot.transform.position;
        
        Vector3 center = (_topPosition + _botPosition) / 2f;
        Value = (center.y + _lineRenderer.offsetY) * _lineRenderer.multY;
    }
}