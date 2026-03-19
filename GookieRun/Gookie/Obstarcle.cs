using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class Obstarcle : GameObject
{
    private const float k_MoveInterval = 0.10f; // 자동으로 움직이는 시간

    private float _moveTimer;
    private float _obsTimer;
    private float Gap;
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

        _obs.AddFirst((8, 2, 3));

        obsX = Enumerable.Range(1, 100).Where(x => x % 2 == 0).ToList();

    }

    public override void Draw(ScreenBuffer buffer)
    {
        // 천장 바닥
        buffer.DrawHLine(0, 1, 100, '=');
        buffer.DrawHLine(0, 12, 100, '=');

        var node = _field.First;

        var createObs = _obs.First;


        while (node != null)
        {
            buffer.DrawHLine(100 + node.Value, 2, block, '=');

            buffer.FillRect(100 + node.Value,
                                createObs.Value.Item1,
                                createObs.Value.Item2,
                                createObs.Value.Item3, '*');



            buffer.DrawHLine(100 + node.Value, 11, block, '=');

            node = node.Next;
            createObs = createObs?.Next;

        }




    }

    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;
        _obsTimer += deltaTime;
        Gap += deltaTime;
        // 자동 이동
        if (_moveTimer > k_MoveInterval)
        {
            Move();
            
            _moveTimer = 0f;
        }

        // 장애물 추가 : 이동 2회당 1 장애물
        if (_obsTimer > k_MoveInterval)
        {
            int pickNum = rnd.Next(1, 4);

            switch (pickNum)
            {
                case 1:
                    // 천장 장애물
                    _obs.AddLast((2, 2, 6));
                    break;

                case 2:
                    // 바닥 장애물 - 1단 점프
                    _obs.AddLast((8, 2, 3));
                    break;

                case 3:
                    // 2단 점프
                    _obs.AddLast((5, 2, 5));
                    break;
                default:
                    break;

            }
            
            _obsTimer = 0f;
        }
        
    }

    private void Move()
    {
        var newObs = FieldPos - 1;
        
        _field.AddFirst(newObs);

    }

}