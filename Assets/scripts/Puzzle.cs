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

    enum Facing
    {
        LEFT, RIGHT, UP, DOWN, STAY
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

    class Node
    {
        public int[,] Arr { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int F { get; private set; }
        public string Way { get; private set; }
        public Facing CanFace { get; private set; }

        public Node(int[,] a, string way, Facing canFace, int x, int y, int f)
        {
            F = f;
            Way = way;
            CanFace = canFace;
            X = x;
            Y = y;
            Arr = new int[4, 4];
            Array.Copy(a, Arr, a.Length);
        }
        public bool CanMoveLeft(int cost)
        {
            return CanFace != Facing.LEFT && Y > 0 && cost > HerStic();
        }

        public bool CanMoveRight(int cost)
        {
            return CanFace != Facing.RIGHT && Y < 3 && cost > HerStic();
        }

        public bool CanMoveUp(int cost)
        {
            return CanFace != Facing.UP && X > 0 && cost > HerStic();
        }

        public bool CanMoveDown(int cost)
        {
            return CanFace != Facing.DOWN && X < 3 && cost > HerStic();
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
                if (Arr[1, 0] != 12) sum++;
                if (Arr[1, 3] != 5) sum++;
                if (Arr[2, 0] != 11) sum++;
                if (Arr[2, 3] != 6) sum++;
                if (Arr[3, 0] != 10) sum++;
                if (Arr[3, 1] != 9) sum++;
                if (Arr[3, 2] != 8) sum++;
                if (Arr[3, 3] != 7) sum++;
            }
            else
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
                if (Arr[3, 3] != 0) sum++;
            }
            return sum + F;
        }

        public void MoveLeft()
        {
            (Arr[X, Y], Arr[X, Y - 1]) = (Arr[X, Y - 1], Arr[X, Y]);
            Y--;
            CanFace = Facing.RIGHT;
            Way += "l";
            F++;
        }

        public void MoveRight()
        {
            (Arr[X, Y], Arr[X, Y + 1]) = (Arr[X, Y + 1], Arr[X, Y]);
            Y++;
            CanFace = Facing.LEFT;
            Way += "r";
            F++;
        }

        public void MoveUp()
        {
            (Arr[X, Y], Arr[X - 1, Y]) = (Arr[X - 1, Y], Arr[X, Y]);
            X--;
            CanFace = Facing.DOWN;
            Way += "u";
            F++;
        }

        public void MoveDown()
        {
            (Arr[X, Y], Arr[X + 1, Y]) = (Arr[X + 1, Y], Arr[X, Y]);
            X++;
            CanFace = Facing.UP;
            Way += "d";
            F++;
        }
        public bool CheckFinish()
        {
            if (Program.Checker == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Arr[0, i] != i + 1 || Arr[3, i] != 12 - i) return false;
                }
                for (int i = 0; i < 3; i++)
                {
                    if (Arr[1, i] != 4 + i) return false;
                }
                return Arr[1, 3] != 5 ? false : true;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Arr[0, i] != i + 1 || Arr[1, i] != 4 + i || Arr[2, i] != 8 + i || Arr[3, i] != 12 + i) return false;
                }
                return true;
            }
        }
    }

    class Program
    {
        public static int[,] Puzzle = new int[4, 4];
        public static int PosX, PosY, Checker, Cost;

        public static int CountStart()
        {
            int sum = 0;
            for (int q = 0; q < 12; q++)
            {
                int row = q / 4;
                int col = q % 4;
                int counter = Puzzle[row, col];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if ((row < i && Puzzle[i, j] < counter && Puzzle[i, j] != 0))
                        {
                            sum++;
                        }
                        else if (row == i && col < j && Puzzle[i, j] < counter && Puzzle[i, j] != 0)
                        {
                            sum++;
                        }
                    }
                }
            }
            return sum;
        }
        public static void MoveLeft()
        {
            (Puzzle[PosX, PosY], Puzzle[PosX, PosY - 1]) = (Puzzle[PosX, PosY - 1], Puzzle[PosX, PosY]);
            PosY--;
        }

        public static void MoveRight()
        {
            (Puzzle[PosX, PosY], Puzzle[PosX, PosY + 1]) = (Puzzle[PosX, PosY + 1], Puzzle[PosX, PosY]);
            PosY++;
        }

        public static void MoveUp()
        {
            (Puzzle[PosX, PosY], Puzzle[PosX - 1, PosY]) = (Puzzle[PosX - 1, PosY], Puzzle[PosX, PosY]);
            PosX--;
        }
        public static void MoveDown()
        {
            (Puzzle[PosX, PosY], Puzzle[PosX + 1, PosY]) = (Puzzle[PosX + 1, PosY], Puzzle[PosX, PosY]);
            PosX++;
        }

        static void InitPuzzle()
        {
            Console.Write("Nhap cac gia tri cho puzzle : ");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Puzzle[i, j] = int.Parse(Console.ReadLine());
                }
            }
            Console.Write("Nhap chi phi toi da cua thuat toan = ");
            Cost = int.Parse(Console.ReadLine());

            bool checkedData = true;
            int sum = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sum += Puzzle[i, j];
                    if (Puzzle[i, j] > 15)
                    {
                        checkedData = false;
                        break;
                    }
                }
            }
            if (sum != 120 || checkedData == false)
            {
                Console.WriteLine("Nhap sai du lieu vui long nhap lai");
                InitPuzzle();
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Puzzle[i, j] == 0)
                    {
                        PosX = i;
                        PosY = j;
                        return;
                    }
                }
            }
        }
        static void Print()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(Puzzle[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        static bool CheckFinish()
        {
            int counter1 = 0, counter2 = 0;
            for (int i = 0; i < 4; i++)
            {
                if (Puzzle[0, i] == i + 1) counter1++;
                if (Puzzle[3, i] == 12 - i) counter1++;
            }
            for (int i = 0; i < 3; i++)
            {
                if (Puzzle[1, i] == 4 + i) counter1++;
            }
            if (Puzzle[1, 3] == 5) counter1++;

            if (counter1 == 16) return true;



            for (int i = 0; i < 4; i++)
            {
                if (Puzzle[0, i] == i + 1) counter2++;
                if (Puzzle[1, i] == 4 + i) counter2++;
                if (Puzzle[2, i] == 8 + i) counter2++;
                if (Puzzle[3, i] == 12 + i) counter2++;
            }
            if (counter2 == 16) return true;
            return false;
        }


        static void main()
        {
            int step = 0;
            long numOfNode = 0;
            bool check = CheckFinish();
            InitPuzzle();
            string way = "";
            Node nd = new Node(Puzzle, "", Facing.STAY, PosX, PosY, 0);
            List<Node> vt = new List<Node>();
            vt.Add(nd);
            Checker = CountStart() % 2;
            Console.WriteLine("Trang thai ban dau : ");
            Print();
            Console.WriteLine();

            while (!check && vt.Count != 0)
            {
                List<Node> open = new List<Node>();
                int i = vt.Count - 1;
                if (vt[i].CheckFinish())
                {
                    way = vt[i].Way;
                    check = true;
                    break;
                }
                else
                {
                    if (vt[i].CanMoveUp(Cost))
                    {
                        Node newNode = new Node(vt[i].Arr, vt[i].Way, vt[i].CanFace, vt[i].X, vt[i].Y, vt[i].F);
                        newNode.MoveUp();
                        open.Add(newNode);
                    }
                    if (vt[i].CanMoveDown(Cost))
                    {
                        Node newNode = new Node(vt[i].Arr, vt[i].Way, vt[i].CanFace, vt[i].X, vt[i].Y, vt[i].F);
                        newNode.MoveDown();
                        open.Add(newNode);
                    }
                    if (vt[i].CanMoveRight(Cost))
                    {
                        Node newNode = new Node(vt[i].Arr, vt[i].Way, vt[i].CanFace, vt[i].X, vt[i].Y, vt[i].F);
                        newNode.MoveRight();
                        open.Add(newNode);
                    }
                    if (vt[i].CanMoveLeft(Cost))
                    {
                        Node newNode = new Node(vt[i].Arr, vt[i].Way, vt[i].CanFace, vt[i].X, vt[i].Y, vt[i].F);
                        newNode.MoveLeft();
                        open.Add(newNode);
                    }
                }
                vt.RemoveAt(i);

                open.Sort((x, y) => x.HerStic().CompareTo(y.HerStic()));
                for (int j = 0; j < open.Count; j++)
                {
                    if (open[j].HerStic() == open[open.Count - 1].HerStic())
                        vt.Add(open[j]);
                }

                numOfNode++;
            }
            if (!check)
            {
                Console.WriteLine("Thuat toan that bai, khong tim duoc dich");
                return;
            }
            for (int i = 0; i < way.Length; i++)
            {
                if (way[i] == 'l')
                {
                    MoveLeft();
                    Print();
                    Console.WriteLine("di chuyen sang trai\n");
                }
                else if (way[i] == 'r')
                {
                    MoveRight();
                    Print();
                    Console.WriteLine("di chuyen sang phai\n");
                }
                else if (way[i] == 'u')
                {
                    MoveUp();
                    Print();
                    Console.WriteLine("di chuyen len tren\n");
                }
                else if (way[i] == 'd')
                {
                    MoveDown();
                    Print();
                    Console.WriteLine("di chuyen xuong duoi\n");
                }
            }
            Console.WriteLine("Thuat toan AKT");
            Console.WriteLine("So buoc di = " + way.Length);
            Console.WriteLine("So phep toan da thuc hien = " + numOfNode);
            Console.WriteLine("Thoi gian tinh toan = " + (DateTime.Now - DateTime.UtcNow));



        }
            }
        }
 

    



