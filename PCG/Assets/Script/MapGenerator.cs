using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    public int xWidth;
    public int zWidth;
    public float waveLength;
    public float amplitude;
    public float ground;
    public GameObject block_Dirt;

    private struct Point
    {
        public int x;
        public int z;
    }
    
    private Tile[,] map; // 타일을 설치하면서 왼쪽과 위에 설치한 타일을 확인하기 위해 사용하는 맵
    private bool[,] isTile; // 타일을 설치하면서 왼쪽과 위에 설치한 타일을 확인하기 위해 사용하는 맵

    // Start is called before the first frame update
    void Start()
    {
        map = new Tile[xWidth, zWidth];
        isTile = new bool[xWidth, zWidth];

        /*
        Queue<Point> Q = new Queue<Point>();

        int[] colorCount = { 2, 2, 2, 2 };

        GeneratorTile(0, 0, colorCount);

        Q.Enqueue(initPoint(1,0));
        Q.Enqueue(initPoint(0,1));



        while (Q.Count != 0)
        {
            Point tmpPoint = Q.Dequeue();
            int x = tmpPoint.x;
            int z = tmpPoint.z;
            // 타일이 설치되어 있지 않으면 설치 하기
            if(x < xWidth && z < zWidth && isTile[x,z] == false)
            {
                
                Q.Enqueue(initPoint(x + 1, z));
                Q.Enqueue(initPoint(x, z + 1));

                if(x == 0 || z == 0 || x == xWidth-1 || z == zWidth-1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        colorCount[i] = 2;
                    }
                    GeneratorTile(x, z, colorCount);   
                }
                else
                {
                    int[] checkIndex = { 2, 2, 2, 2 };
                    if (x - 1 > 0)
                    {
                        checkIndex[0] = map[x - 1, z].colorCount[1]; // 0
                        checkIndex[3] = map[x - 1, z].colorCount[2]; // 3
                    }
                    if (z - 1 > 0)
                    {
                        checkIndex[3] = map[x, z - 1].colorCount[0]; // 3
                        checkIndex[2] = map[x, z - 1].colorCount[1]; // 2
                    }

                    if (checkIndex[0] == 2 || checkIndex[2] == 2)
                    {
                        checkIndex[1] = (int)Random.Range(0, 3);
                    }
                    else
                    {
                        checkIndex[1] = (int)Random.Range(0, 2);
                    }


                    GeneratorTile(x, z, checkIndex);
                }

                isTile[x,z] = true;
            }
            
        }

        for (int x = 0; x < xWidth; x++)
        {
            for (int z = 0; z < zWidth; z++)
            {
                float xCoord = x / waveLength;
                float zCoord = z / waveLength;
                int y = (int)(Mathf.PerlinNoise(xCoord, zCoord) * amplitude + ground);
                int randomNum = (int)Random.Range(0, block_Dirt.Length);
                Instantiate(block_Dirt[randomNum], new Vector3(x, y, z), Quaternion.identity);
            }
        }
        */

        // 테두리 벽 채우기
        int[] blackCount = { 2, 2, 2, 2 };
        for(int j = 0; j < xWidth; j++)
        {
            GeneratorTile(j, zWidth-1, blackCount);
            GeneratorTile(xWidth-1, j, blackCount);
            GeneratorTile(j, 0, blackCount);
            GeneratorTile(0, j, blackCount);
        }

        // 쭉 돌면서 랜덤하게 타일 배치
        bool bCompleteToGenerateTile = false;
        bool bGenerateTile = true;
        int[] xlist = { 0, 0, -1, 1 };
        int[] zlist = { 1, -1, 0, 0 };
        int[,] tileList = { { 0, 1 }, { 2, 3 }, { 0, 3 }, { 1, 2 } };
        int[,] otherList = { { 3, 2 }, { 1, 0 }, { 1, 2 }, { 0, 3 } };
        while (!bCompleteToGenerateTile)
        {
            Debug.Log("한 바퀴~");
            bCompleteToGenerateTile = true;
            
            for (int i = 1; i < xWidth-1; i++)
            {
                for(int j = 1; j < zWidth-1; j++)
                {
                    bGenerateTile = true;
                    if (!isTile[i, j] && Random.Range(0f, 10f) > 0.2f)
                    {
                        bGenerateTile = false;
                        bCompleteToGenerateTile = false;
                    }
                    if (!isTile[i, j] && bGenerateTile)
                    {
                        Debug.Log(i+ "," + j + " 생성");
                        bool bBlackTile = false;
                        bool[] bSetColor = new bool[4] { false, false, false, false };
                        int[] colorCount = new int[4] { 0, 0, 0, 0 };
                        // 상(x, z+1), 하(x, z-1), 좌(x-1,z), 우(x+1, z) 타일 배치 확인
                        for (int k = 0; k < 4; k++)
                        {
                            // 타일이 있는 경우
                            if (isTile[i + xlist[k], j + zlist[k]])
                            {
                                // 상(0, 1), 하(2, 3), 좌(0, 3), 우(1, 2)
                                if (bSetColor[tileList[k, 0]] && colorCount[tileList[k, 0]] != map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 0]]) 
                                {
                                    map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 0]] = colorCount[tileList[k, 0]];
                                    map[i + xlist[k], j + zlist[k]].setCube();
                                }
                                if (bSetColor[tileList[k, 1]] && colorCount[tileList[k, 1]] != map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 1]])
                                {
                                    map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 1]] = colorCount[tileList[k, 1]];
                                    map[i + xlist[k], j + zlist[k]].setCube();
                                }
                                colorCount[tileList[k, 0]] = map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 0]];
                                colorCount[tileList[k, 1]] = map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 1]];
                                Debug.Log(i+ "," + j+"|"+ k + " : " + map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 0]] + ", " + map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 1]]);
                                bSetColor[tileList[k, 0]] = true;
                                bSetColor[tileList[k, 1]] = true;
                                if(map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 0]] == 2 || map[i + xlist[k], j + zlist[k]].colorCount[otherList[k, 1]] == 2)
                                    bBlackTile = true;
                            }
                            // 타일이 없고 색 설정이 안되어 있으면 랜덤하게 설정
                            else
                            {
                                int rangeNum = 2;
                                if (bBlackTile)
                                    rangeNum = 3;
                                if (!bSetColor[otherList[k, 0]])
                                    colorCount[otherList[k, 0]] = Random.Range(0, rangeNum);
                                if (!bSetColor[otherList[k, 1]])
                                    colorCount[otherList[k, 1]] = Random.Range(0, rangeNum);
                            }
                        }
                        //타일 배치 시 bCompleteToGenerateTile 를 false로
                        bCompleteToGenerateTile = false;
                        GeneratorTile(i, j, colorCount);
                    }
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Point initPoint(int x, int z)
    {
        Point newPoint = new Point();
        newPoint.x = x;
        newPoint.z = z;

        return newPoint;
    }


    void GeneratorTile(int x, int z, int[] colorCount)
    {
        GameObject TObject = block_Dirt;
        Tile newTile = TObject.GetComponent<Tile>();
        for(int i = 0; i < colorCount.Length; i++)
        {
            newTile.colorCount[i] = colorCount[i];
        }
        newTile.setCube();

        map[x, z] = new Tile();
        map[x, z].colorCount = colorCount;
        isTile[x, z] = true;
        Instantiate(TObject, new Vector3(x, 0, z), Quaternion.identity);
       
    }
}
