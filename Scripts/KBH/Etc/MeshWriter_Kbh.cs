using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshWriter_Kbh : MonoBehaviour
{
    CanvasRenderer canvasRenderer;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material mat;
    
    
    public void Awake()
    {
        Mesh mesh = this.mesh;
        canvasRenderer = GetComponent<CanvasRenderer>();
        canvasRenderer.materialCount = 1;
        canvasRenderer.SetMaterial(mat, 0);
        canvasRenderer.SetMesh(mesh);  
    }

    private void Update()
    {
        
    }
}
