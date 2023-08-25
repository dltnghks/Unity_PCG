using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    static int pieceAmount = 4;
    // Start is called before the first frame update
    public int[] colorCount = new int[pieceAmount];
    public GameObject[] piece = new GameObject[pieceAmount];
    public Material[] pieceMesh = new Material[3];
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCube()
    {
        for(int i = 0; i < pieceAmount; i++)
        {
            piece[i].GetComponent<MeshRenderer>().material = pieceMesh[colorCount[i]];
        }
    }
}
