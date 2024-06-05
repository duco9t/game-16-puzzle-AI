using System;
using System.Collections;
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
                    int n = UnityEngine.Random.Range(0, 4);
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

    public enum Facing { LEFT, RIGHT, UP, DOWN, STAY }
    public class Node
    {
        public int[,] Arr;
        public int X;
        public int Y;
        public int F;
        public string Way;
        public Facing CanFace;

        public Node(int[,] a, string way, Facing canFace, int x, int y, int f)
        {
            F = f;
            Way = way;
            CanFace = canFace;
            X = x;
            Y = y;
            Arr = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Arr[i, j] = a[i, j];
                }
            }
        }

        public bool CanMoveLeft(int cost)
        {
            return CanFace != Facing.LEFT && Y > 0 && cost > HerStic();
        }

        public bool CanMoveRight(int cost)
        {
            return CanFace != Facing.RIGHT && Y < 2 && cost > HerStic();
        }

        public bool CanMoveUp(int cost)
        {
            return CanFace != Facing.UP && X > 0 && cost > HerStic();
        }

        public bool CanMoveDown(int cost)
        {
            return CanFace != Facing.DOWN && X < 2 && cost > HerStic();
        }

        public int HerStic()
        {
            int sum = 0;
            if (Program.Checker == 1)
            {
                if (Arr[0, 0] != 1) sum++;
                if (Arr[0, 1] != 2) sum++;
                if (Arr[0, 2] != 3) sum++;
                if (Arr[0, 3] != 4) sum++;
                if (Arr[1, 0] != 5) sum++;
                if (Arr[1, 1] != 6) sum++;
                if (Arr[1, 2] != 7) sum++;
                if (Arr[1, 3] != 8) sum++;
                if (Arr[2, 0] != 9) sum++;
                if (Arr[2, 1] != 10) sum++;
                if (Arr[2, 2] != 11) sum++;
                if (Arr[2, 3] != 12) sum++;
                if (Arr[3, 0] != 13) sum++;
                if (Arr[3, 1] != 14) sum++;
                if (Arr[3, 2] != 15) sum++;
                if (Arr[3, 3] != 16) sum++;
            }
            
            return sum + F;
        }

        public void MoveLeft()
        {
            int temp = Arr[X, Y];
            Arr[X, Y] = Arr[X, Y - 1];
            Arr[X, Y - 1] = temp;
            Y--;
            CanFace = Facing.RIGHT;
            Way += "l";
            F++;
        }

        public void MoveRight()
        {
            int temp = Arr[X, Y];
            Arr[X, Y] = Arr[X, Y + 1];
            Arr[X, Y + 1] = temp;
            Y++;
            CanFace = Facing.LEFT;
            Way += "r";
            F++;
        }

        public void MoveUp()
        {
            int temp = Arr[X, Y];
            Arr[X, Y] = Arr[X - 1, Y];
            Arr[X - 1, Y] = temp;
            X--;
            CanFace = Facing.DOWN;
            Way += "u";
            F++;
        }

        public void MoveDown()
        {
            int temp = Arr[X, Y];
            Arr[X, Y] = Arr[X + 1, Y];
            Arr[X + 1, Y] = temp;
            X++;
            CanFace = Facing.UP;
            Way += "d";
            F++;
        }

        public bool CheckFinish()
        {
            if (Program.Checker == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Arr[0, i] != i + 1 || Arr[2, i] != 7 - i) return false;
                }
                return Arr[1, 0] != 8 || Arr[1, 2] != 4 ? false : true;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Arr[i, 0] != (i * 3) + 1 || Arr[i, 2] != (i * 3) + 3) return false;
                }
                return Arr[0, 1] != 2 || Arr[1, 1] != 5 || Arr[2, 1] != 8 ? false : true;
            }
        }

        public class Program
        {
            public static int Checker = 1;

            public static void Main()
            {
                int[,] puzzle = new int[3, 3] { { 1, 2, 3 }, { 8, 0, 4 }, { 7, 6, 5 } };
                Node root = new Node(puzzle, "", Facing.STAY, 1, 1, 0);

            }

        }



    }
}


  
