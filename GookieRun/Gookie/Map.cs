using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

class Map : GameObject
{
    private const float k_MoveInterval = 0.06f; // 자동으로 움직이는 시간
    

    public List<Item> map = new List<Item>();

    private float _moveTimer;

    private int Top = 1;
    private int Bottom = 14;
    private int fieldLength = 100;
    private int node = 0;
    private int jelly = 0;

    // 맵 움직임을 위한 값을 리스트로
    private readonly LinkedList<int> _field = new LinkedList<int>();


    public int FieldPos => _field.First.Value;


    public Map(Scene scene) : base(scene)
    {
        Name = "Map";

        // 시작지점 설정
        _field.AddFirst(15);
        node = fieldLength;
    }

    public override void Draw(ScreenBuffer buffer)
    {

        // 천장 바닥
        buffer.DrawHLine(0, Top, fieldLength, '=');
        buffer.DrawHLine(0, Bottom, fieldLength, '=');

        foreach (var obj in map)
        {
            buffer.FillRect(obj.X, obj.Y, obj.Width, obj.Height, obj.C, obj.Color);

        }

    }

    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;


        // 자동 이동
        if (_moveTimer > k_MoveInterval)
        {
            Move();

            _moveTimer = 0f;

            node++;
            jelly++;

            if(node >= 20)
            {
                CreateObstarcle();
                CreateJelly(map.Last().Height, map.Last().Name);
                    
                jelly = 0;

                node = 0;
            }

            else if(jelly > 5)
            {
                CreateJelly();
                jelly = 0;
            }
        }



        
    }

    private void Move()
    {
        // 움직일 때 마다 안에 든 x 좌표 모두 감소
        for (int i = map.Count - 1; i >= 0; i--)
        {
            var item = map[i];

            item.X -= 1;

            map[i] = item;

            // 맵 밖으로 넘어간거 제거
            if (map[i].X + map[i].Width < 0)
            {
                map.RemoveAt(i);
            }
        }

    }

    private void CreateObstarcle()
    {
        Random rnd = new Random();
        int pickNum = rnd.Next(1, 11);

        // 랜덤 배치 할거면 Y값 로직 정해서 넣기
        switch (pickNum)
        {
            case 1:
            case 2:
                // 천장
                AddItem(fieldLength, Top + 1, 3, 9, "Hang", '*');
                break;

            case 3:
            case 4:
            case 5:
                // 바닥 장애물 - 1단 점프
                AddItem(fieldLength, Bottom - 3, 3, 3, "Jump", '*');
                break;

            case 6:
            case 7:
                // 2단 점프
                AddItem(fieldLength, Bottom - 5, 3, 5, "Double", '*');
                break;

            default:
                AddItem(fieldLength, Bottom, 0, 0, "null", '*');
                break;

        }
    }
    private void CreateJelly(int h = 0, string name = "Hang")
    {
        Random rnd = new Random();
        int pickNum = rnd.Next(1, 11);
        int jellY = Bottom - 2;


        // Y 값 로직
        if( h != 0 && name != "Hang")
        {
            jellY = Bottom - 2 - h;
        }


        // 랜덤 배치 할거면 Y값 로직 정해서 넣기
        switch (pickNum)
        {
            case 1:
                // 큰 젤리
                AddItem(fieldLength + 1, jellY, 2, 2, "Big", 'J', ConsoleColor.Red);
                break;

            default:
                // 일반 젤리 
                AddItem(fieldLength + 1, jellY, 1, 1, "Normal", 'J', ConsoleColor.Red);
                break;

        }

    }




    


    private void AddItem(int x, int y, int width, int height, string name, char c, ConsoleColor color = ConsoleColor.White)
    {

        Item item = new Item()
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Name = name,
            C = c,
            Color = color
        };


        map.Add(item);

    }




}