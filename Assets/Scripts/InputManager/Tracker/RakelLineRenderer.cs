using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RakelLineRenderer : MonoBehaviour
{
    [SerializeField] DistanceToCanvas distanceToCanvas;
    public float multX, multY;
    public float 
        offsetX, 
        offsetY, 
        offsetZ;

    private OilPaintEngine _oilPaintEngine;
    private float _rakelLength;
    private float _rakelWidth;
    private BoxCollider _box;
    private GameObject _rakel;
    private LineRenderer _line;
    private ButtonInteraction _interaction;
    private Transform _top, _bot;
    
    //Used in TrackerRakelPositionX and Y to calculate where to paint
    public Vector3 RakelEdgePos { get; private set; }

    void Start()
    {
        _interaction = GameObject.Find("Interaction").GetComponent<ButtonInteraction>();
        _rakelWidth = new RakelConfiguration().Width * 0.5f;

        _line = GameObject.Find("LineRenderer").GetComponent<LineRenderer>();
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.startWidth = _rakelWidth;
        _line.endWidth = _rakelWidth;

        _box = GameObject.Find("LineRenderer").GetComponent<BoxCollider>();
        _rakel = GameObject.Find("RenderedRakel");
        _oilPaintEngine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();

        _top = GameObject.Find("Top").transform;
        _bot = GameObject.Find("Bottom").transform;
    }

    void Update()
    {
        _rakelLength = _oilPaintEngine.Config.RakelConfig.Length;
        
        Vector3 topPos = _top.position;
        Vector3 botPos = _bot.position;
        Vector3 rakelUp = (topPos - botPos).normalized; 
        Vector3 wallNormal = Vector3.forward;           
        Vector3 rakelRight = Vector3.Cross(wallNormal, rakelUp).normalized; 
        
        bool isFlipped = Vector3.Dot(_rakel.transform.right, rakelRight) < 0;
        
        Vector3 center = (topPos + botPos) / 2f;
        
        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);
        float _minZ = -2.6f;
        float _maxZ = -2.56f;

        float rakelTilt = Mathf.Abs(_rakel.transform.eulerAngles.y - 180);
        if (rakelTilt > 79) rakelTilt = 79;
        if (rakelTilt > 90) rakelTilt = 180 - rakelTilt;
        offset.z = _minZ + (rakelTilt / 79f) * (_maxZ - _minZ);
        
        float bladeOffset = _rakelWidth / 2f;
        Vector3 sideOffset = rakelRight * bladeOffset * (isFlipped ? -1f : 1f);

        Vector3 pos = center + sideOffset;
        pos = new Vector3((pos.x + offset.x) * multX, (pos.y + offset.y) * multY, pos.z + offset.z);
        
        RakelEdgePos = pos;

        // Changing the Offset of the Z Coordinate if the Ui is turned of or on
        float savePosZ = pos.z;
        if (_interaction.uiActive)
            pos.z = -0.45f;
        else
            pos.z = -0.15f;

        // Placing the LineRenderer
        Vector3 startPoint = pos + (rakelUp * (_rakelLength / 2));
        Vector3 endPoint = pos - (rakelUp * (_rakelLength / 2));

        _line.SetPosition(0, startPoint);
        _line.SetPosition(1, endPoint);

        pos.z = savePosZ;
        
        Quaternion rakelRotation = Quaternion.LookRotation(Vector3.forward, rakelUp);
        _box.transform.rotation = rakelRotation;
        _box.size = new Vector3(_rakelWidth, 4, distanceToCanvas.canvasOffset);
        _box.transform.position = pos;
        
        _line.transform.rotation = Quaternion.LookRotation(Vector3.forward, rakelUp);
    }
}
