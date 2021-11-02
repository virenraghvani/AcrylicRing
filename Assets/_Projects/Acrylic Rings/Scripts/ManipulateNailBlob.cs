using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateNailBlob : MonoBehaviour
{
    public static Action<ManipulateNailBlob> Spawned = delegate { };
    public static Action<float> ProgressChanged = delegate { };
    public static Action OnCompleted = delegate { };

    [SerializeField] private List<MeshFilter> _interpolatedMeshes;
    [SerializeField] private float _radius;
    [SerializeField] private float _strength;
    [SerializeField] private AnimationCurve _falloffCurve;

    [SerializeField] public Transform _toolRaycastPoint;
    [SerializeField] public Transform _marker;
    //[SerializeField] public Transform _brush;

    [SerializeField] [Range(0, 1)] private float _horizontalStrength;
    [SerializeField] [Range(0, 1)] private float _verticalStrength;

    [SerializeField] [Range(0, 1)] private float _horizontalFlow;
    [SerializeField] [Range(0, 1)] private float _verticalFlow;

    private List<Vector3[]> _interpolatedVerticies;

    private Vector3 _startPos;
    private bool _manipulating;
    private Mesh _mesh;
    private MeshCollider _meshCollider;
    private Transform _transform;
    private float _currentManipulateForce = 0f;

    [Header("Perling Noise")]
    [SerializeField] private float _perlinOffset = 0.2f;
    [SerializeField] private float _perlinFrequency = 6f;
    [SerializeField] private float _perlinMagnitude = 0.1f;
    [SerializeField] private AnimationCurve _perlinPowerPerDistance;

    [Header("Progress Wave")]
    [SerializeField] private float _progressWaveMagnitude = 0.2f;
    [SerializeField] private AnimationCurve _progressWaveCurve;
    private List<Vector3> _progressWaveNormals;

    [Header("Strength Movement Modifier")]
    [SerializeField] private float _distanceToPowerRatio = 3f;

    [Header("Retraction")]
    [SerializeField] private float _retractionActivateMaxStrength = 0.3f;
    [SerializeField] private float _retractionTime = 2f;
    [SerializeField] private float _retractionForce = 40f;
    [SerializeField] private AnimationCurve _retractionForceToDistance;
    private float _retractionTimer;

    private float _autocompletePercentage = 0.3f;
    private float _autocompleteSpeed = 0.5f;
    private bool _isAutocompleting = false;

    private bool _hapticEnabled = false;
    private float _hapticFeedbackTimer = 0;
    private const float _hapticFeedbackMinTime = 0.1f;
    private const float _hapticFeedbackMinForceToTrigger = 0.05f;

    private Dictionary<int, Vector3> _pointsNormalizedPos; // int = vertex index , float = 0 to 1 represent the lerp value between 

    private AnimationCurve _startFalloff = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
    private Vector3 _lastHitPos;

    private Dictionary<int, float> _distanceToStrength;

    public bool IS_READY_FOR_POWDER;
    //public MoreMountains.NiceVibrations.NiceVibrationsDemoManager vibration;

    //  public AudioSource audioSource;
    public AudioClip audioClip;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _toolRaycastPoint = gameManager.toolRayCast;
        _marker = gameManager.marker;

        InitDictionaries();
        _marker.localScale = Vector3.one * _radius;
        _hapticEnabled = true;

        //vibration = GameObject.Find("HapticsManager").GetComponent<MoreMountains.NiceVibrations.NiceVibrationsDemoManager>();

        _progressWaveNormals = new List<Vector3>(_mesh.normals);

        //  StartManipulate(_nailBlob.transform.position);
        // Manipulate(_nailBlob.transform.position);

        Spawned.Invoke(this);
    }

    private void Awake()
    {
        InputManager.inst.OnClickCallback += StartManipulate;
        InputManager.inst.OnDragCallback += Manipulate;
        InputManager.inst.OnClickEndCallback += EndManipulate;

        InputManager.MouseDragStarted += OnDragStarted;
        InputManager.MouseDragEnded += OnDragEnded;

        _mesh = GetComponent<MeshFilter>().mesh;
        _meshCollider = GetComponent<MeshCollider>();
        _transform = transform;
    }

    private void Update()
    {
        HandleRetraction();
        HandleAutocomplete();
        HandleProgressCalculation();
        HandleHapticFeedback();
    }

    private void InitDictionaries()
    {
        _pointsNormalizedPos = new Dictionary<int, Vector3>();

        _interpolatedVerticies = new List<Vector3[]>();
        foreach (var mesh in _interpolatedMeshes)
        {
            _interpolatedVerticies.Add(mesh.sharedMesh.vertices);
        }

        for (int i = 0; i < _mesh.vertexCount; i++)
            _pointsNormalizedPos.Add(i, Vector3.zero);

        _distanceToStrength = new Dictionary<int, float>();

        float max = Mathf.NegativeInfinity;
        float min = Mathf.Infinity;

        for (int i = 0; i < _mesh.vertexCount; i++)
        {
            float dist = Vector3.Distance(_interpolatedVerticies[0][i], _interpolatedVerticies[_interpolatedVerticies.Count - 1][i]);

            if (dist > max)
                max = dist;
            if (dist < min)
                min = dist;
        }

        for (int i = 0; i < _mesh.vertexCount; i++)
        {
            float dist = Vector3.Distance(_interpolatedVerticies[0][i], _interpolatedVerticies[_interpolatedVerticies.Count - 1][i]);
            _distanceToStrength.Add(i, Mathf.Lerp(5f, 1f, Mathf.InverseLerp(min, max, dist)));
        }
    }

    private void OnDestroy()
    {
        InputManager.inst.OnClickCallback -= StartManipulate;
        InputManager.inst.OnDragCallback -= Manipulate;
        InputManager.inst.OnClickEndCallback -= EndManipulate;

        InputManager.MouseDragStarted -= OnDragStarted;
        InputManager.MouseDragEnded -= OnDragEnded;
    }

    private void StartManipulate(Vector2 startPos)
    {
        //if (InputManager.inst.IS_READY_TO_MOVE)
        //    return;

        RaycastHit hit;
        Ray ray = new Ray(_toolRaycastPoint.position, _toolRaycastPoint.forward);

        if (Physics.SphereCast(ray, _radius, out hit, Mathf.Infinity, LayerMask.GetMask("Nail")))
        {
            _manipulating = true;
            _startPos = hit.point;
            _marker.position = hit.point;

            //if (Game.Progress.Instance.SoundStatus)
            //{
            //    AudioController.instance.PlayAudio(audioClip);
            //}
        }
        else
        {
            _manipulating = false;
        }
    }

    private void Manipulate(Vector2 currentPos)
    {
        //if (!InputManager.inst.IS_READY_TO_MOVE)
        //    return;
        if (_isAutocompleting)
            return;

        RaycastHit hit;
        bool wasHit = false;
        Ray ray = new Ray(_toolRaycastPoint.position, _toolRaycastPoint.forward);

        if (Physics.SphereCast(ray, _radius, out hit, Mathf.Infinity, LayerMask.GetMask("Nail")))
        {
            wasHit = true;
        }
        else
        {
            _manipulating = false;
            _currentManipulateForce = 0f;
        }

        if (_manipulating && wasHit)
        {
            _currentManipulateForce = Mathf.Clamp01((hit.point - _lastHitPos).magnitude * _distanceToPowerRatio);

            ManipulatePoints(hit.point, _currentManipulateForce);

            //_marker.position = hit.point;
            //_brush.transform.position = new Vector3(_brush.transform.position.x, hit.point.y, _brush.transform.position.z);

            _lastHitPos = hit.point;

            //if (Game.Progress.Instance.SoundStatus)
            //{
            //    if (!AudioController.instance.IsPlaying())
            //        AudioController.instance.PlayAudio(audioClip);
            //}

            if (_currentManipulateForce >= _retractionActivateMaxStrength)
                _retractionTimer = _retractionTime;
        }
        else if (wasHit)
        {
            _manipulating = true;
            _startPos = hit.point;
            _lastHitPos = hit.point;
            _marker.position = hit.point;
            //_brush.transform.position = new Vector3(_brush.transform.position.x, hit.point.y, _brush.transform.position.z);
        }
    }

    private void EndManipulate(Vector2 endPos)
    {
        //if (InputManager.inst.IS_READY_TO_MOVE)
        //    return;

        _manipulating = false;
        _currentManipulateForce = 0f;

        //if (Game.Progress.Instance.SoundStatus)
        //{
        //    AudioController.instance.PauseAudio();
        //}
    }

    private void ManipulatePoints(Vector3 hitPos, float strenghtFactor)
    {
        MovePoints(hitPos, _falloffCurve, _strength * strenghtFactor, _verticalStrength, _horizontalStrength);
    }

    private void MovePoints(Vector3 pos, AnimationCurve curve, float strength, float vertInfluence, float horInfluence, bool flowing = false, bool autofinish = false)
    {
        Vector3[] vertices = _mesh.vertices;
        float dist;
        float vertStrength;
        float vertCompleteRatio;

        for (int i = 0; i < vertices.Length; i++)
        {
            dist = Vector3.Distance(pos, _transform.TransformPoint(vertices[i]));
            vertCompleteRatio = (_pointsNormalizedPos[i].x + _pointsNormalizedPos[i].y + _pointsNormalizedPos[i].z) / 3f;
            vertStrength = (flowing ? _retractionForceToDistance.Evaluate(vertCompleteRatio) * strength : strength);

            float xzInf = curve.Evaluate(Mathf.Clamp(1 - (dist / _radius), 0f, 1f)) * horInfluence;
            float yInf = curve.Evaluate(Mathf.Clamp(1 - (dist / _radius), 0f, 1f)) * vertInfluence;

            if (autofinish)
            {
                xzInf = horInfluence;
                yInf = vertInfluence;
            }

            float xNorm = Mathf.Clamp(xzInf * vertStrength * _distanceToStrength[i] * 0.001f + _pointsNormalizedPos[i].x, 0f, 1f);
            float zNorm = Mathf.Clamp(xzInf * vertStrength * _distanceToStrength[i] * 0.001f + _pointsNormalizedPos[i].z, 0f, 1f);
            float yNorm = Mathf.Clamp(yInf * vertStrength * _distanceToStrength[i] * 0.001f + _pointsNormalizedPos[i].y, 0f, 1f);

            Vector3 normPos = new Vector3(xNorm, yNorm, zNorm);

            _pointsNormalizedPos[i] = normPos;

            float newX = GetVertexInterpolatedPositionX(i, _pointsNormalizedPos[i].x);
            float newZ = GetVertexInterpolatedPositionZ(i, _pointsNormalizedPos[i].z);
            float newY = GetVertexInterpolatedPositionY(i, _pointsNormalizedPos[i].y);

            //Apply Perlin Noise
            newZ += Mathf.Lerp(0f, Mathf.PerlinNoise((newX + _perlinOffset) * _perlinFrequency, newY * _perlinFrequency) * _perlinMagnitude, _perlinPowerPerDistance.Evaluate(vertCompleteRatio));

            //Apply Progress Wave
            newZ += _progressWaveCurve.Evaluate(vertCompleteRatio) * _progressWaveMagnitude * _progressWaveNormals[i].y;
            newY -= _progressWaveCurve.Evaluate(vertCompleteRatio) * _progressWaveMagnitude * _progressWaveNormals[i].z;

            vertices[i] = new Vector3(newX, newY, newZ);
        }

        _mesh.vertices = vertices;
        _mesh.RecalculateNormals();
        _meshCollider.sharedMesh = _mesh;
    }

    private float GetVertexInterpolatedPositionX(int vertexIndex, float progress)
    {
        if (_interpolatedVerticies.Count == 2)
            return Mathf.Lerp(_interpolatedVerticies[0][vertexIndex].x, _interpolatedVerticies[1][vertexIndex].x, progress);

        if (progress < 0.5f)
        {
            return Mathf.Lerp(_interpolatedVerticies[0][vertexIndex].x, _interpolatedVerticies[1][vertexIndex].x, progress * 2);
        }
        else
        {
            return Mathf.Lerp(_interpolatedVerticies[1][vertexIndex].x, _interpolatedVerticies[2][vertexIndex].x, progress * 2 - 1f);
        }
    }

    private float GetVertexInterpolatedPositionY(int vertexIndex, float progress)
    {
        if (_interpolatedVerticies.Count == 2)
            return Mathf.Lerp(_interpolatedVerticies[0][vertexIndex].y, _interpolatedVerticies[1][vertexIndex].y, progress);

        if (progress < 0.5f)
        {
            return Mathf.Lerp(_interpolatedVerticies[0][vertexIndex].y, _interpolatedVerticies[1][vertexIndex].y, progress * 2);
        }
        else
        {
            return Mathf.Lerp(_interpolatedVerticies[1][vertexIndex].y, _interpolatedVerticies[2][vertexIndex].y, progress * 2 - 1f);
        }
    }

    private float GetVertexInterpolatedPositionZ(int vertexIndex, float progress)
    {
        if (_interpolatedVerticies.Count == 2)
            return Mathf.Lerp(_interpolatedVerticies[0][vertexIndex].z, _interpolatedVerticies[1][vertexIndex].z, progress);

        if (progress < 0.5f)
        {
            return Mathf.Lerp(_interpolatedVerticies[0][vertexIndex].z, _interpolatedVerticies[1][vertexIndex].z, progress * 2);
        }
        else
        {
            return Mathf.Lerp(_interpolatedVerticies[1][vertexIndex].z, _interpolatedVerticies[2][vertexIndex].z, progress * 2 - 1f);
        }
    }

    private void HandleRetraction()
    {
        if (_isAutocompleting)
            return;
        if (_retractionTimer <= 0)
            return;

        MovePoints(_lastHitPos, _startFalloff, -_retractionForce * (_retractionTimer / _retractionTime), _verticalFlow, _horizontalFlow, true);
        _retractionTimer -= Time.deltaTime;
    }

    private void HandleAutocomplete()
    {
        if (!_isAutocompleting && CalculatePercantage() >= _autocompletePercentage)
        {
            _isAutocompleting = true;
            _hapticEnabled = false;
        }

        if (!_isAutocompleting)
            return;

        MovePoints(Vector3.zero, _falloffCurve, _strength * _autocompleteSpeed, _verticalStrength, _horizontalStrength, false, true);
    }

    private void HandleProgressCalculation()
    {
        ProgressChanged.Invoke(CalculatePercantage());
    }

    public float CalculatePercantage()
    {
        float sum = 0f;

        for (int i = 0; i < _pointsNormalizedPos.Count; i++)
        {
            sum += _pointsNormalizedPos[i].y;
        }

        return sum / _pointsNormalizedPos.Count;
    }

    public void CompleteNail()
    {
        Vector3[] vertices = _mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = _interpolatedVerticies[_interpolatedVerticies.Count - 1][i];
        }
        _mesh.vertices = vertices;
        _mesh.RecalculateNormals();
        _meshCollider.sharedMesh = _mesh;
        _hapticEnabled = false;

        //AudioController.instance.StopAudio();

    }

    public void DisableMarker()
    {
        _marker.gameObject.SetActive(false);
    }

    private void HandleHapticFeedback()
    {
        if (!_hapticEnabled)
            return;

        _hapticFeedbackTimer -= Time.deltaTime;

        if (_hapticFeedbackTimer >= 0)
            return;
        if (_currentManipulateForce < _hapticFeedbackMinForceToTrigger)
            return;

        _hapticFeedbackTimer = _hapticFeedbackMinTime;

        //vibration.TriggerLightImpact();
    }

    private void OnDragStarted(Vector2 mousePosition)
    {
        _hapticEnabled = true;
    }

    private void OnDragEnded(Vector2 mousePosition)
    {
        _hapticEnabled = false;
    }
}
