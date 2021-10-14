using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercantageDisplay : MonoBehaviour
{
    [SerializeField] private ManipulateNailBlob _manipulateNailBlob;
    
    private Text _text;

    void Start()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        _text.text = (_manipulateNailBlob.CalculatePercantage() * 100f).ToString("0");
    }
}
