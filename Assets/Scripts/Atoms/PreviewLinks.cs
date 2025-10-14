using System.Collections.Generic;
using UnityEngine;

public class PreviewLinks : MonoBehaviour
{
    private List<LineRenderer> previewLineRenderers;
    private CreateLinks createLinks;

    void Start()
    {
        LineRenderer[] getPreviewLineRenderers = GetComponentsInChildren<LineRenderer>();
        previewLineRenderers = new(getPreviewLineRenderers);
        createLinks = transform.parent.GetComponentInChildren<CreateLinks>();
    }

    void Update()
    {
        PreviewJoints();
    }
    
    private void PreviewJoints()
    {
        if (createLinks.canCreateLink && createLinks.pickAndDrop.isPicking)
        {
            for (int i = 0; i < createLinks.quantityOfLinkGenerate; i++)
            {
                previewLineRenderers[i].positionCount = createLinks.quantityOfLinkGenerate;
                previewLineRenderers[i].SetPosition(0, transform.position);
                previewLineRenderers[i].SetPosition(1, createLinks.nearestHits[i].transform.position);
            }
        }
        else
        {
            for (int i = 0; i < createLinks.quantityOfLinkGenerate; i++)
            {
                previewLineRenderers[i].positionCount = 0;
            }
        }
    }
}
