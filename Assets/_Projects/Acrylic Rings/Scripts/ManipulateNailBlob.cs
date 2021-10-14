using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateNailBlob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshFilter _nailBlob;
    [SerializeField] private MeshFilter _nailFinished;
    [SerializeField] private Transform _toolRaycastPoint;
    [SerializeField] private Transform _marker;

    [Header("Settings")]
    [SerializeField] private float _radius;
    [SerializeField] private float _strength;
    [SerializeField] private AnimationCurve _falloffCurve;

    [Space]
    [SerializeField] private float _flowStrength;
    [SerializeField] private float _flowDuration;
    [SerializeField] private AnimationCurve _flowCurve;

    [Space]
    [SerializeField] private float _startFlowDuration;

    private AnimationCurve _startFalloff = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

    private Vector3[]    _blobVertices;
    private Vector3[]    _nailVertices;

    private Vector3      _startPos;
    private Vector3      _lastHitPos;
    private bool         _manipulating;
    private bool         _wasFlowing;
    private Mesh         _mesh;
    private MeshCollider _meshCollider;
    private Transform    _transform;

    private Coroutine    _afterTouch;

    private Dictionary<int, float> _pointsNormalizedPos; // int = vertex index , float = 0 to 1 represent the lerp value between _nailBlob to _nailFinished
    private Dictionary<int, float> _distanceToStrength;

    private void Awake()
    {
        InputManager.inst.OnClickCallback    += StartManipulate;
        InputManager.inst.OnDragCallback     += Manipulate;
        InputManager.inst.OnClickEndCallback += EndManipulate;

        _mesh         = GetComponent<MeshFilter>().mesh;
        _meshCollider = GetComponent<MeshCollider>();

        _blobVertices = _nailBlob.sharedMesh.vertices;
        _nailVertices = _nailFinished.sharedMesh.vertices;

        InitDictionaries();

        _marker.localScale = Vector3.one * _radius;

        _transform = transform;
    }

    //private void Start()
    //{
    //    StartCoroutine(StartFlow());
    //}

    private void InitDictionaries()
    {
        _pointsNormalizedPos = new Dictionary<int, float>();

        for (int i = 0; i < _mesh.vertexCount; i++)
            _pointsNormalizedPos.Add(i, 0f);

        _distanceToStrength = new Dictionary<int, float>();

        float max = Mathf.NegativeInfinity;
        float min = Mathf.Infinity;

        for (int i = 0; i < _mesh.vertexCount; i++)
        {
            float dist = Vector3.Distance(_blobVertices[i], _nailVertices[i]);

            if (dist > max)
                max = dist;
            if (dist < min)
                min = dist;
        }

        for (int i = 0; i < _mesh.vertexCount; i++)
        {
            float dist = Vector3.Distance(_blobVertices[i], _nailVertices[i]);
            _distanceToStrength.Add(i, Remap(dist, min, max, 2f, 1f));
        }
    }

    private void StartManipulate(Vector2 startPos)
    {
        RaycastHit hit;
        Ray ray = new Ray(_toolRaycastPoint.position, _toolRaycastPoint.forward);

        if(Physics.SphereCast(ray, _radius, out hit, Mathf.Infinity, LayerMask.GetMask("Nail")))
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Nail")))
        {
            _manipulating    = true;
            _startPos        = hit.point;
            _marker.position = hit.point;
            _wasFlowing      = false;
        }
        else
        {
            _manipulating = false;
        }
    }

    private void Manipulate(Vector2 currentPos)
    {
        RaycastHit hit;
        bool wasHit = false;
        Ray ray     = new Ray(_toolRaycastPoint.position, _toolRaycastPoint.forward);

        if (Physics.SphereCast(ray, _radius, out hit, Mathf.Infinity, LayerMask.GetMask("Nail")))
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Nail")))
            wasHit = true;

        if (_manipulating && wasHit)
        {
            ManipulatePoints(hit.point);
            _marker.position = hit.point;
            _lastHitPos      = hit.point;
            _wasFlowing      = false;

            if (_afterTouch != null)
            {
                StopCoroutine(_afterTouch);
                _afterTouch = null;
            }
        }
        else if(wasHit)
        {
            _manipulating    = true;
            _startPos        = hit.point;
            _marker.position = hit.point;
        }
        else if (!wasHit)
        {
            if (_afterTouch == null)
            {
                _afterTouch = StartCoroutine(AfterTouch());
                _wasFlowing = true;
            }
        }
    }

    private void EndManipulate(Vector2 endPos)
    {
        _manipulating = false;

        if (!_wasFlowing)
        {
            if (_afterTouch != null)
                StopCoroutine(_afterTouch);

            _afterTouch = StartCoroutine(AfterTouch());
        }
    }

    private void ManipulatePoints(Vector3 hitPos)
    {
        MovePoints(hitPos, _falloffCurve, _strength);

        //Vector3[] vertices = _mesh.vertices;
        //float dist;
        //float influence;

        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    dist      = Vector3.Distance(hitPos, _transform.TransformPoint(vertices[i]));
        //    influence = _falloff.Evaluate(Mathf.Clamp(1 - (dist / _radius), 0f, 1f));

        //    _pointsNormalizedPos[i] += influence * _strength * 0.001f; 

        //    vertices[i] = Vector3.Lerp(_blobVertices[i], _nailVertices[i], _pointsNormalizedPos[i]);
        //}

        //_mesh.vertices = vertices;
        //_mesh.RecalculateNormals();
        //_meshCollider.sharedMesh = _mesh;
    }

    private void MovePoints(Vector3 pos, AnimationCurve curve, float strength)
    {
        Vector3[] vertices = _mesh.vertices;
        float dist;
        float influence;

        for (int i = 0; i < vertices.Length; i++)
        {
            dist = Vector3.Distance(pos, _transform.TransformPoint(vertices[i]));
            influence = curve.Evaluate(Mathf.Clamp(1 - (dist / _radius), 0f, 1f));

            _pointsNormalizedPos[i] = Mathf.Clamp(influence * strength * _distanceToStrength[i] * 0.001f + _pointsNormalizedPos[i], 0f, 1f);

            vertices[i] = Vector3.Lerp(_blobVertices[i], _nailVertices[i], _pointsNormalizedPos[i]);
        }

        _mesh.vertices = vertices;
        _mesh.RecalculateNormals();
        _meshCollider.sharedMesh = _mesh;
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private IEnumerator AfterTouch()
    {
        float timer = 0f;

        while (timer < _flowDuration)
        {
            if (timer < _flowDuration)
            {
                MovePoints(_lastHitPos, _flowCurve, _flowStrength * (1f - timer / _flowDuration));
            }

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }


    /// <summary>
    /// Use this coroutine when the powder blob is placed on the nail for the first time
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartFlow()
    {
        float timer = 0f;

        while (timer < _startFlowDuration)
        {
            MovePoints(_lastHitPos, _startFalloff, _flowStrength * (1f - timer / _startFlowDuration));

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    /// <summary>
    /// return 0 to 1 as avrage percentage of nail completion
    /// </summary>
    /// <returns></returns>
    public float CalculatePercantage()
    {
        float sum = 0f;

        for (int i = 0; i < _pointsNormalizedPos.Count; i++)
        {
            sum += _pointsNormalizedPos[i];
        }

        return sum / _pointsNormalizedPos.Count;
    }
}
