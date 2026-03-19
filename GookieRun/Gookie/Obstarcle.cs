using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class Obstarcle : GameObject
{
    private const float k_MoveInterval = 0.04f; // 자동으로 움직이는 시간

    private float _moveTimer;

    // 맵 움직임을 위한 값을 리스트로
    private readonly LinkedList<int> _field = new LinkedList<int>();

    // 장애물 만들기 위한 요소값을 리스트로
    private LinkedList<(int, int, int)> _obs = new LinkedList<(int, int, int)>();

    public int FieldPos => _field.First.Value;
    public LinkedList<(int X, int Y, int Height)> ObsPos { get; private set;  }

    public Obstarcle(Scene scene) : base(scene)
    {
        Name = "Obstacle";

        // 시작지점 설정
        _field.AddFirst(15);

        _obs.AddFirst((0, 0, 0));

        ObsPos = new LinkedList<(int X, int Y, int Height)>();

    }

    public override void Draw(ScreenBuffer buffer)
    {
        // 천장 바닥
        buffer.DrawHLine(0, 1, 100, '=');
        buffer.DrawHLine(0, 12, 100, '=');

        var node = _field.First;

        var createObs = _obs.First;

        var lastNode = 0;

        while (node != null)
        {

            // node.value가 -될 때 부터 출력되니까 절댓값
            // 이전 노드랑 현재 노드랑 일정 거리이상 차이 나면 출력
            if(Math.Abs(node.Value - lastNode) >= 17)
            {
                buffer.FillRect(100 + node.Value,
                    createObs.Value.Item1,
                    createObs.Value.Item2,
                    createObs.Value.Item3, '*');

                // 장애물 위치 기록 x,y값 , y값 + 범위
                // x 가 범위안에 들어올 경우 y 검사하는 형식
                // 벽이 플레이어 위치를 감지하는 방식으로 바꿀 수 도 있음.
                ObsPos.AddLast((100 + node.Value, createObs.Value.Item1, createObs.Value.Item3));

                lastNode = node.Value;
                createObs = createObs.Next;

                
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
                _obs.AddLast((2, 2, 8));
                break;

            case 2:
                // 바닥 장애물 - 1단 점프
                _obs.AddLast((10, 2, 2));
                break;

            case 3:
                // 2단 점프
                _obs.AddLast((7, 2, 5));
                break;
            default:
                break;

        }

        
    }

}