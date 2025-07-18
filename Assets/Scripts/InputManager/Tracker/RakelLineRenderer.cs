    using UnityEngine;
    using Vector3 = UnityEngine.Vector3;

public class RakelLineRenderer : MonoBehaviour
{   
    private float canvaspositionZ;
    
    private float _rakelpositionZ;
    private OilPaintEngine OilPaintEngine;
    
    private float _rakelLength;
    private float _rakelWidth;
    private Color_ _current_color;
    private Color _paint_color, _no_paint_color;
    private BoxCollider _box;
    private GameObject _rakel;
    
    private LineRenderer _line;

    public float multX, multY;
    public float offsetX, offsetY,offsetZ;
    
    private float _rakelRotationX;
    private float _rakelRotationY;
    private float _rakelRotationZ;
    
    private Transform _top, _bot;
    
    void Start()
    {
        _rakelWidth = new RakelConfiguration().Width * 0.2f;
        
        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        _line.material = new Material(Shader.Find("Sprites/Default"));
        
        //Changing the Width of the LineRenderer
        _line.startWidth = _rakelWidth; 
        _line.endWidth = _rakelWidth;

        _box = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
        _rakel = GameObject.Find("RenderedRakel");
        OilPaintEngine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        
        _top = GameObject.Find("Top").transform;
        _bot = GameObject.Find("Bottom").transform;
    }
    void Update()
    {
        _rakelLength = OilPaintEngine.Config.RakelConfig.Length;

        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);
        float _minZ = -2.6f;
        float _maxZ = -2.56f;

        float rakelTilt = Mathf.Abs(_rakel.transform.eulerAngles.y - 180);
        if (rakelTilt > 79) rakelTilt = 79;
        if (rakelTilt > 90) rakelTilt = 180 - rakelTilt;

        offset.z = _minZ + (rakelTilt / 79f) * (_maxZ - _minZ);
        
        Vector3 topPos = _top.position;
        Vector3 botPos = _bot.position;
        Vector3 rakelDir = (topPos - botPos).normalized;

        Vector3 center = (topPos + botPos) / 2f;
        Vector3 pos = new Vector3((center.x + offset.x) * multX, (center.y + offset.y) * multY, center.z + offset.z);
        
        Vector3 startPoint = pos + (rakelDir * (_rakelLength / 2));
        Vector3 endPoint = pos - (rakelDir * (_rakelLength / 2));

        _line.SetPosition(0, startPoint);
        _line.SetPosition(1, endPoint);
        
        Quaternion rakelRotation = Quaternion.LookRotation(Vector3.forward, rakelDir);
        _box.transform.rotation = rakelRotation;
        
        _box.size = new Vector3(_rakelWidth, 4, 0.01f); //Collider Length is static at 4, so multiple buttons can't be clicked if the Rakel is longer
        
        //_box.transform.position = pos + rakelDir;
        _box.transform.position = pos;
        _line.transform.rotation = Quaternion.LookRotation(Vector3.forward, rakelDir);
    }
}