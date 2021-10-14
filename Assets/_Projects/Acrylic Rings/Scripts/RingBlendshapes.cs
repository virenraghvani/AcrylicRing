using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBlendshapes : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;


    float blendWeight;
    public bool startBlendShape;

    private void Start()
    {
        blendWeight = 100;
    }

    private void Update()
    {
        if (!startBlendShape)
            return;

        if (blendWeight >= 0)
        {
            //if (blendWeight <= 50)
            //{
            //    blendWeight += .6f;     // Nail file time for first half
            //}
            //else
            //{
            //    blendWeight += 1f;     // Nail file time for second half
            //}

            blendWeight -= .5f;     // Nail file time for second half

            // Filing progrerss
            for (int i = 0; i < 11; i++)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(i, blendWeight);
            }
        }
    }
}
