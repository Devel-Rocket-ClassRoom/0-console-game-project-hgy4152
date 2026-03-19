using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class Obstarcle : GameObject
{
    private const float k_MoveInterval = 0.10f; // 자동으로 움직이는 시간

    private float _moveTimer;
    private float _obsTimer;
    private readonly LinkedList<int> _field = new LinkedList<int>();

    private LinkedList<(int, int, int)> _obs = new LinkedList<(int, int, int)>();
    private int block = 4;

    Random rnd = new Random();
    List<int> obsX;

    public int FieldPos => _field.First.Value;

    public Obstarcle(Scene scene) : base(scene)
    {
        Name = "Obstacle";

        // 시작지점 설정
        _field.AddFirst(15);

        _obs.AddFirst((0, 0, 0));

        obsX = Enumerable.Range(1, 100).Where(x => x % 2 == 0).ToList();

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
            if(Math.Abs(node.Value - lastNode) >= 15)
            {
                buffer.FillRect(100 + node.Value,
                    createObs.Value.Item1,
                    createObs.Value.Item2,
                    createObs.Value.Item3, '*');

                lastNode = node.Value;
            }

            node = node.Next;
            createObs = createObs?.Next;

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
            _obsTimer = 0f;
        }

  

        
    }

    private void Move()
    {
        var newObs = FieldPos - 1;
        
        _field.AddFirst(newObs);

    }

    private void CreateObstarcle()
    {
        int pickNum = rnd.Next(1, 4);

        switch (pickNum)
        {
            case 1:
                // 천장 장애물
                _obs.AddLast((2, 2, 7));
                break;

            case 2:
                // 바닥 장애물 - 1단 점프
                _obs.AddLast((8, 2, 4));
                break;

            case 3:
                // 2단 점프
                _obs.AddLast((6, 2, 6));
                break;
            default:
                break;

        }

        }
    }

}