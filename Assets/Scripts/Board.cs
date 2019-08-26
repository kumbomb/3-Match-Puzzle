using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int width;
    public int height;
    public GameObject tilePrefab;
    //전체 타일에 2가지 타일을 갖도록
    //배경 될거, 보여줄거?
    private BackgroundTile[,] allTiles;
    public GameObject[] dots;
    public GameObject[,] allDots;


    // Start is called before the first frame update
    void Start()
    {
        //전체 타일, 돌을 기록할 배열 크기 확보
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //각 좌표에 맞게 사각형 모양 출력
                Vector2 tempPosition = new Vector2(i, j);
                //Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( "+i+", " +j + " )";

                //그 사각형 위치에 랜덤한 돌 생성
                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + " )";

                allDots[i, j] = dot;
            }
        }
    }


    // Update is called once per frame
    //void Update()
    //{

    //}


}
