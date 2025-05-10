using UnityEngine;
public class TrackerRakelPositionY : FloatValueSource
{
    public override void Update()
    {
        //Value = GameObject.Find("RenderedRakel").transform.position.y;
        //Value = (GameObject.Find("RenderedRakel").transform.position.y - 1.58f) * 16f; //Testaufbau
        Value = (GameObject.Find("RenderedRakel").transform.localPosition.y - 1.54f) * 9.2f; //Vor Ort -1.56
    }
}