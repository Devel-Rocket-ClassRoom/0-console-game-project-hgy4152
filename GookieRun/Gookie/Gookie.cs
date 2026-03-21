using Framework.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
public class Gookie : GameObject
{
    public int _direction;
    private int startPos;
    private int currPos;

    private bool isJump;
    private bool isTop;
    private bool isSlide;


    private float _jumpTimer;

    public int body { get; private set; } = 4;
    private int height = 3;
    private int jumpCount;


    public Skill skill;
    public bool isSkill;

    public int CurrentPosition => currPos;
    public int StartPosition => startPos;

    public Gookie(Scene scene) : base(scene)
    {
        Name = "Gookie";

        startPos = 14;

        skill = new Skill(scene);
    }

    public override void Draw(ScreenBuffer buffer)
    {
        // 그릴 위치 보정해야하니 -body
        buffer.FillRect(1, currPos - body, 2, body, '|');


    }

    public override void Update(float deltaTime)
    {
        
        _jumpTimer += deltaTime;

        currPos = startPos + _direction;


        // 캐릭터와 스킬 갯수가 많으면 event로 관리해도 될듯
        isSkill = skill.Dash(deltaTime);

        // Action
        if (Input.IsKeyDown(ConsoleKey.Spacebar) && jumpCount < 2)
        {
            
            isJump = true;
            jumpCount++;

            height = _direction - 3;
        }

        // 점프
        if (isJump && _jumpTimer > 0.07)
        {
            Jump();
            _jumpTimer = 0;
        }
        // 체공
        else if(isTop)
        {
            if(_jumpTimer > 0.1)
                isTop = false;
        }
        // 낙하
        else if(!isJump && _jumpTimer > 0.05)
        {
            Fall();
            _jumpTimer = 0;

        }


        // 슬라이드
        if(Input.IsKeyDown(ConsoleKey.DownArrow) && !isJump && !isSlide)
        {
            body = 2;
            isSlide = true;
        }

        else if(isJump || Input.IsKeyUp(ConsoleKey.DownArrow)) // 점프하거나 손가락 땔 시
        {
            isSlide = false;
            body = 4;
        }

    }


    void Jump()
    {
        _direction -= 1;

        if(_direction <= height)
        {
            isJump = false;
            isTop = true;
        }
    }
    void Fall()
    {

        if(_direction < 0)
        {
            _direction += 1;
        }
        else if(currPos >= 6)
        {
            jumpCount = 0;

            // 낙하 슬라이드 보정
            if (isSlide && 10 > currPos)
            {
                _direction = 2;
            }
        }

    }

    public bool isBound(int x, int y, string name)
    {
        bool isCrash = false;

        isCrash = currPos - y > 0;

        // 천장 장애물
        if(name == "Hang")
        {
            isCrash = body != 2;
        }

        // 뒷통수에 걸리는 것도 맞게 할려면 x >= 0
        return x >= 0 && x <= 2 && isCrash ;
    }

}