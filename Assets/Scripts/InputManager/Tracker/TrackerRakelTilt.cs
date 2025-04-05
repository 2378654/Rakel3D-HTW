using UnityEngine;
using TMPro;
public class TrackerRakelTilt : FloatValueSource
{
    private float tilt;
    private TextMeshProUGUI _text;
    private float _rotationY;
    public override void Update()
    {
        //only get positive Values
        _rotationY = Mathf.Abs(GameObject.Find("RenderedRakel").transform.eulerAngles.y- 180);
        
        if (_rotationY > 79)
        {
            _rotationY = 79;
        }
        
        if (_rotationY > 90)
        {
            _rotationY = 180-_rotationY;
        }
        Value = _rotationY; 
    }
}