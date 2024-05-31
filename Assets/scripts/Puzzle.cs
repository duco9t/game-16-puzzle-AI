﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public NumberBox boxPrefabs;
    public NumberBox[,] boxes = new NumberBox[4, 4];
    public Sprite[] sprites;

    void Start()
    {
        Init();
        for (int i = 0; i < 999; i++)
            Shuffle();
    }

    void Init()
    {
        int HeuristicManhattan(int currentX, int currentY, int targetX, int targetY)
        {
            return Mathf.Abs(targetX - currentX) + Mathf.Abs(targetY - currentY);
        }

        int n = 0;
        float scale = 2.0f; // Scale factor của NumberBox
        Vector2 startPos = new Vector2(-9.5f, -3.0f); // Tọa độ ban đầu

        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                Vector2 position = startPos + new Vector2(x * scale, y * scale); // Điều chỉnh vị trí khởi tạo theo scale và tọa độ ban đầu
                NumberBox box = Instantiate(boxPrefabs, position, Quaternion.identity);
                box.transform.localScale = new Vector3(scale, scale, 1); // Đảm bảo rằng scale của NumberBox được cập nhật
                box.SetStartPosition(startPos); // Thiết lập tọa độ bắt đầu cho NumberBox
                box.Init(x, y, n + 1, sprites[n], ClickToSwap);
                boxes[x, y] = box;
                n++;
            }
        }
    }

    void ClickToSwap(int x, int y)
    {
        int dx = getDx(x, y);
        int dy = getDy(x, y);
        Swap(x, y, dx, dy);
    }

    void Swap(int x, int y, int dx, int dy)
    {
        var from = boxes[x, y];
        var target = boxes[x + dx, y + dy];

        boxes[x, y] = target;
        boxes[x + dx, y + dy] = from;

        from.UpdatePox(x + dx, y + dy);
        target.UpdatePox(x, y);
    }

    int getDx(int x, int y)
    {
        if (x < 3 && boxes[x + 1, y].IsEmpty())
            return 1;

        if (x > 0 && boxes[x - 1, y].IsEmpty())
            return -1;

        return 0;
    }

    int getDy(int x, int y)
    {
        if (y < 3 && boxes[x, y + 1].IsEmpty())
            return 1;

        if (y > 0 && boxes[x, y - 1].IsEmpty())
            return -1;

        return 0;
    }

    void Shuffle()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (boxes[i, j].IsEmpty())
                {
                    Vector2 pos = getValidMove(i, j);
                    Swap(i, j, (int)pos.x, (int)pos.y);
                }
            }
        }
    }

    private Vector2 lastMove;

    Vector2 getValidMove(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);

        do
        {
            int n = Random.Range(0, 4);
            if (n == 0)
                pos = Vector2.left;
            else if (n == 1)
                pos = Vector2.right;
            else if (n == 2)
                pos = Vector2.up;
            else
                pos = Vector2.down;
        } while (!(isValidRange(x + (int)pos.x) && isValidRange(y + (int)pos.y)) || isRepeatMove(pos));

        lastMove = pos;

        return pos;
    }

    bool isValidRange(int n)
    {
        return n >= 0 && n <= 3;
    }

    bool isRepeatMove(Vector2 pos)
    {
        return pos * -1 == lastMove;
    }
}