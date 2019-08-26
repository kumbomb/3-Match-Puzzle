using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    //info for swipe
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    private Board board;
    private GameObject otherDot;

    public bool isMatched = false;

    //locate for swipe
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPos;

    public float swipeAngle = 0;


    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;

    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        //세개이상 맞으면 흰색으로
        if(isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }

        targetX = column;
        targetY = row;
        SwapXY();
    }

    //돌 교체
    void SwapXY()
    {
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move towards the target
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, .4f);
        }
        else
        {
            //directly set the position
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
            board.allDots[column, row] = this.gameObject;
        }
        if (Mathf.Abs(targetX - transform.position.y) > .1)
        {
            //move towards the target
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, .4f);
        }
        else
        {
            //directly set the position
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
            board.allDots[column, row] = this.gameObject;
        }

    }

    //터치시
    private void OnMouseDown()
    {
        //요렇게 하면 world 좌표가 아님
        //firstTouchPosition = Input.mousePosition;
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
    }
    //터치 종료시
    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();

    }
    //터치간의 각도 계산
    void CalculateAngle()
    {
        //각도 값으로 표현하기 위해 180 / PI를 곱함
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
                                 finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        //Debug.Log(swipeAngle);
        MovePieces();
    }
    
    //좌표값 변경
    void MovePieces()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            //right swap
            otherDot = board.allDots[column+1,row];
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            //up swap
            otherDot = board.allDots[column, row+1];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135)  && column > 0)
        {
            //left swap
            otherDot = board.allDots[column - 1, row];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //down swap
            otherDot = board.allDots[column , row - 1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
    }

    //매치 판단
    void FindMatches()
    {
        //좌우 벗어나지않고
        if(column > 0 && column < board.width - 1)
        {
            //양 옆의 돌 확인
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            //세개가 같은 돌이면
            if (leftDot1.tag == rightDot1.tag && rightDot1.tag == this.gameObject.tag)
            {
                leftDot1.GetComponent<Dot>().isMatched = true;
                rightDot1.GetComponent<Dot>().isMatched = true;
                isMatched = true;
            }
        }
        if (row > 0 && row < board.height - 1)
        {
            //양 옆의 돌 확인
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            //세개가 같은 돌이면
            if (upDot1.tag == downDot1.tag && downDot1.tag == this.gameObject.tag)
            {
                upDot1.GetComponent<Dot>().isMatched = true;
                downDot1.GetComponent<Dot>().isMatched = true;
                isMatched = true;
            }
        }
    }
}
