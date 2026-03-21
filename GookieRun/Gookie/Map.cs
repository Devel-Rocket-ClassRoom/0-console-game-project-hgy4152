using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

class Map : GameObject
{
    private const float MoveInterval = 0.04f; // 자동으로 움직이는 시간
    public float Speed{ get; private set;}

    public List<Item> mapObj = new List<Item>();

    private float _moveTimer;

    private int Top = 1;
    private int Bottom = 14;
    private int fieldLength = 100;
    private int node = 0;


    // 나중에 맵 조작해야하는 스킬 같은 것들 구현할 때
    public bool isFast = false;


    public Map(Scene scene) : base(scene)
    {
        Name = "Map";

        // 시작지점 설정
        node = fieldLength;
        Speed = MoveInterval;
       
    }

    public override void Draw(ScreenBuffer buffer)
    {

        // 천장 바닥
        buffer.DrawHLine(0, Top, fieldLength, '=');
        buffer.DrawHLine(0, Bottom, fieldLength, '=');

        foreach (var obj in mapObj)
        {
            buffer.FillRect(obj.X, obj.Y, obj.Width, obj.Height, obj.C, obj.Color);

            if(obj.destroy)
            {
                buffer.FillRect(obj.X, obj.Y, obj.Width, obj.Height, '`', ConsoleColor.Blue);
                buffer.WriteText(obj.X, obj.Y + obj.Height/2, "+50", ConsoleColor.Blue);
            }
        }

    }

    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;


        // 자동 이동
        if (_moveTimer > Speed)
        {
            Move();

            _moveTimer = 0f;


            if(node >= 20)
            {
                CreateObstarcle();

                var lastObs = mapObj[mapObj.Count - 1];
                CreateJelly(lastObs.Height, lastObs.Name);


                node = 0;
   
            }
            else if(node % 5 == 0)
            {
                CreateJelly();
            }
        }
    }

    private void Move()
    {
        // 움직일 때 마다 안에 든 x 좌표 모두 감소
        for (int i = mapObj.Count - 1; i >= 0; i--)
        {
            var item = mapObj[i];

            item.X -= 1;

            // 맵 밖으로 넘어간거 제거
            if (mapObj[i].X + mapObj[i].Width < 0)
            {
                mapObj.RemoveAt(i);
            }
        }

        node++;

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
                AddItem(fieldLength, Top + 1, 3, 9, "Hang", "Obstarcle", '*');
                break;

            case 3:
            case 4:
            case 5:
                // 바닥 장애물 - 1단 점프
                AddItem(fieldLength, Bottom - 3, 3, 3, "Jump", "Obstarcle", '*');
                break;

            case 6:
            case 7:
                // 2단 점프
                AddItem(fieldLength, Bottom - 5, 3, 5, "Double", "Obstarcle", '*');
                break;

            default:
                AddItem(fieldLength, Bottom, 0, 0, "null", "Obstarcle", '*');
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
                AddItem(fieldLength + 1, jellY - 1, 2, 2, "Big", "Jelly", 'J', ConsoleColor.Red);
                break;

            default:
                // 일반 젤리 
                AddItem(fieldLength + 1, jellY, 1, 1, "Normal", "Jelly", 'J', ConsoleColor.Red);
                break;

        }

    }

    private void AddItem(int x, int y, int width, int height, string name, string type, char c, ConsoleColor color = ConsoleColor.White)
    {

        Item item = new Item()
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Name = name,
            Type = type,
            C = c,
            Color = color
        };


        mapObj.Add(item);

    }


    public void mapReaction(bool isSkill, Skill skill)
    {
        if(isSkill)
        {
            Speed = skill.speed;
        }
        else
        {
            Speed = MoveInterval;
        }
    }

}