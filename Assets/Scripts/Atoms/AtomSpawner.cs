using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AtomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject atomPrefab;
    [SerializeField] private int atomQuantity;
    [SerializeField] private int atomsPerRow = 3;
    private EdgeCollider2D receptacleEdge;
    
    public List<GameObject> atoms = new();

    private void Start()
    {
        receptacleEdge = GetComponent<EdgeCollider2D>();
        SpawnAtoms();
    }

    
    private void SpawnAtoms()
    {
        if (atomQuantity % atomsPerRow != 0)
        {
            Debug.LogWarning(
                $"Il faut que votre nombre d'atomes par ligne ({atomsPerRow}) " +
                $"soit un multiple du nombre total d'atomes ({atomQuantity}) " +
                $"pour éviter les atomes incomplets sur la dernière ligne. "
            );
            
            return;
        }
        
        Vector2 atomColliderSize = atomPrefab.GetComponent<CapsuleCollider2D>().size;
        
        //Be sure to know what is each point
        Vector2[] atomColliderPoints = receptacleEdge.points;
        
        Vector2 offSet = atomColliderSize / 1.5f;
        
        Vector2 spawnPoint = (Vector2)transform.TransformPoint(atomColliderPoints[1]) + offSet;
        
        float xSizeReceptacleCollider = atomColliderPoints[2].x - atomColliderPoints[1].x;
        float xSecureSizeReceptacleCollider = xSizeReceptacleCollider - offSet.x;
        
        if (xSecureSizeReceptacleCollider / atomsPerRow < atomColliderSize.x)
        {
            Debug.Log("Too much atoms by rows");
            return;
        }
        
        for (int i = 0; i < atomQuantity / atomsPerRow; i++)
        {
            for (int j = 0; j < atomsPerRow; j++)
            {
                float xPosition = spawnPoint.x + j * atomColliderSize.x * offSet.x;
                float yPosition = spawnPoint.y + i * atomColliderSize.y * offSet.y;
                Vector2 finalSpawnPoint = new Vector2(xPosition, yPosition);
                
                GameObject atom = Instantiate(atomPrefab, finalSpawnPoint, Quaternion.identity, transform);
                atoms.Add(atom);
            }
        }
    }
}
