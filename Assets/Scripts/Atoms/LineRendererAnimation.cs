using System;
using UnityEngine;

public class LineRendererAnimation : MonoBehaviour
{
    private CreateLinks createLinks;
    [SerializeField] private Texture[] textures;
    [SerializeField] private float fps;
    private float fpsCounter;
    private int animationStep;
    
    private void Start()
    {
        createLinks = GetComponent<CreateLinks>();
    }

    private void Update()
    {
        SwitchMaterialLineRenderer();
    }

    private void SwitchMaterialLineRenderer()
    {
        fpsCounter += Time.deltaTime;
        
        if (fpsCounter >= 1f / fps)
        {
            animationStep++;
                    
            if (animationStep == textures.Length)
            {
                animationStep = 0;
            }

            foreach (LineRenderer lineRenderer in createLinks.lineRenderers)
            {
                if (lineRenderer.positionCount > 0)
                {
                    lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);
                }
            }

            fpsCounter = 0;
        }
        
    }
}
