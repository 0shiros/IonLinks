using System;
using UnityEngine;

public class LineRendererAnimation : MonoBehaviour
{
    private CreateLinks createLinks;
    [SerializeField] private Texture[] textures;
    [SerializeField] private float fps;
    
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
        foreach (LineRenderer lineRenderer in createLinks.lineRenderers)
        {
            float fpsCounter = 0;
            int animationStep = 0;
            
            fpsCounter += Time.deltaTime;
            if (fpsCounter >= 1f / fps)
            {
                animationStep++;
                
                if (animationStep == textures.Length)
                {
                    animationStep = 0;
                }
                
                lineRenderer.material.SetTexture("ElectricJoint", textures[animationStep]);
                fpsCounter = 0;
            }
        }
    }
}
