using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class Map : GameObject
{
    private const float k_MoveInterval = 0.1f; // 자동으로 움직이는 시간

    private float _moveTimer;

    private int Top = 1;
    private int Bottom = 14;
    private int fieldLength = 100;

    public bool isJelly = false;

    // 맵 움직임을 위한 값을 리스트로
    private readonly LinkedList<int> _field = new LinkedList<int>();

    // 장애물 만들기 위한 요소값을 리스트로
    private LinkedList<(int Y, int Width, int Height)> _obs = new LinkedList<(int, int, int)>();

    public int FieldPos => _field.First.Value;


    public Map(Scene scene) : base(scene)
    {
        Name = "Map";

        // 시작지점 설정
        _field.AddFirst(15);

        _obs.AddFirst((0, 0, 0));


    }

    public override void Draw(ScreenBuffer buffer)
    {
        // 천장 바닥
        buffer.DrawHLine(0, Top, fieldLength, '=');
        buffer.DrawHLine(0, Bottom, fieldLength, '=');

        var node = _field.First;

        var createObs = _obs.First;

        var lastNode = 0;
        var itemNode = 0;

        isJelly = false;

        while (node != null)
        {
            int obsX = fieldLength + node.Value;
            int obsY = createObs.Value.Y;
            int obsW = createObs.Value.Width;
            int obsH = createObs.Value.Height;

            // node.value가 -될 때 부터 출력되니까 절댓값
            // 이전 노드랑 현재 노드랑 일정 거리이상 차이 나면 출력
            if (Math.Abs(node.Value - lastNode) >= 20)
            {

                // 장애물 생성
                buffer.FillRect(obsX, obsY, obsW, obsH, '*');

                // 큰 젤리 생성
                if (obsH < 8)
                {
                    buffer.FillRect(obsX, obsY - 3, 2 , 2, '*', ConsoleColor.Red);
                }
                else
                {
                    buffer.FillRect(obsX, obsY + obsH + 1, 2, 2, '*', ConsoleColor.Red);
                }

                lastNode = node.Value;
                createObs = createObs.Next;

                // 충돌 판정
                if(!isJelly && obsX <= 2 && 0 < obsX)
                    isJelly = true;

            }
            else if(Math.Abs(node.Value - itemNode) >= 5) // 일반 젤리 생성
            {
                // 장애물 간격이 20이니
                // 장애물(1) + 4 위치 부터 5간격으로 생성 시 상대 기준으로 5, 10, 15 에 생성되서 일정하게 뽑을 수 있음
                buffer.SetCell(obsX + 4, Bottom - 1, '*', ConsoleColor.Red);
                itemNode = node.Value;


                if (!isJelly && obsX <= 2 && 0 < obsX)
                    isJelly = true;


            }



            node = node.Next;


        }



    }

    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;

        // 자동 이동
        if (_moveTimer > k_MoveInterval)
        {
            Move();
            CreateObstarcle();

            _moveTimer = 0f;
        }

        
    }

    private void Move()
    {
        var newObs = FieldPos - 1;
        
        _field.AddFirst(newObs);

    }

    private void CreateObstarcle()
    {
        Random rnd = new Random();
        int pickNum = rnd.Next(1, 4);

        switch (pickNum)
        {
            case 1:
                // 천장 장애물
                _obs.AddLast((Top + 1, 2, 9));
                break;

            case 2:
                // 바닥 장애물 - 1단 점프
                _obs.AddLast((Bottom - 2, 2, 2));
                break;

            case 3:
                // 2단 점프
                _obs.AddLast((Bottom - 5, 2, 5));
                break;
            default:
                break;

        }

        
    }


    public bool CrashReport(int playerY)
    {
        if(Bottom - 1 - playerY <= 0) // 큰 젤리 구분해야함
        {
            return true;
        }


        return false;
    }

}