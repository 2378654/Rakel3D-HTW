using UnityEngine;
public class TrackerRakelPositionY : FloatValueSource
{
    public override void Update()
    {

        //Value = (GameObject.Find("RenderedRakel").transform.position.y - 1.58f) * 16f; //Testaufbau
        Value = (GameObject.Find("RenderedRakel").transform.position.y - 1.57f) * 6f; //Vor Ort
    }
}
