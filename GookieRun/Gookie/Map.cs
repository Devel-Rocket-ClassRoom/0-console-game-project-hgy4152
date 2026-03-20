using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class Obstarcle : GameObject
{
    private const float k_MoveInterval = 0.04f; // 자동으로 움직이는 시간

    private float _moveTimer;

    private int Top = 1;
    private int Bottom = 14;
    private int fieldLength = 100;

    // 맵 움직임을 위한 값을 리스트로
    private readonly LinkedList<int> _field = new LinkedList<int>();

    // 장애물 만들기 위한 요소값을 리스트로
    private LinkedList<(int Y, int Width, int Height)> _obs = new LinkedList<(int, int, int)>();

    public int FieldPos => _field.First.Value;


    public Obstarcle(Scene scene) : base(scene)
    {
        Name = "Obstacle";

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

        while (node != null)
        {

            // node.value가 -될 때 부터 출력되니까 절댓값
            // 이전 노드랑 현재 노드랑 일정 거리이상 차이 나면 출력
            if(Math.Abs(node.Value - lastNode) >= 20)
            {
                buffer.FillRect(100 + node.Value,
                    createObs.Value.Y,
                    createObs.Value.Width,
                    createObs.Value.Height, '*');

                
                
                if(createObs.Value.Height < 8)
                {
                    buffer.FillRect(fieldLength + node.Value, createObs.Value.Y - 3, 2 , 2, '*', ConsoleColor.Red);
                }
                else
                {
                    buffer.FillRect(fieldLength + node.Value, 
                        createObs.Value.Y + createObs.Value.Height + 1, 2, 2, '*', ConsoleColor.Red);
                }

                lastNode = node.Value;
                createObs = createObs.Next;

            }
            else if(Math.Abs(node.Value - itemNode) >= 5) // 일반 젤리 생성
            {

                buffer.SetCell(fieldLength + node.Value + 4, Bottom - 1, '*', ConsoleColor.Red);
                itemNode = node.Value;

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

}