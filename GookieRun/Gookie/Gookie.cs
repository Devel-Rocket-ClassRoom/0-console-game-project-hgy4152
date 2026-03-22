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
    private int xPos;
    private int Width;

    public bool isJump {  get; private set; }
    public bool isTop { get; private set; }
    public bool isSlide { get; private set; }
    public bool isSkill { get; private set; }
    public bool isDead { get; private set; }
    public bool isFall { get; private set; }
 

    private float _jumpTimer;
    private float deadTimer;

    public int body { get; private set; } = 3;
    private int height = 3;
    private int jumpCount;


    public Skill skill;


    public int CurrentPosition => currPos;
    public int StartPosition => startPos;

    public Gookie(Scene scene) : base(scene)
    {
        Name = "Gookie";

        startPos = 14;
        xPos = 5;
        Width = 2;
        skill = new Skill(scene);
    }

    public override void Draw(ScreenBuffer buffer)
    {
        // 그릴 위치 보정해야하니 -body
        buffer.FillRect(xPos, currPos - body, Width, body, '|');

    }

    public override void Update(float deltaTime)
    {

        _jumpTimer += deltaTime;

        currPos = startPos + _direction;

        if (isFall)
        {
            deadTimer += deltaTime;
            return;
        }

        // 캐릭터와 스킬 갯수가 많으면 event로 관리해도 될듯
        // 캐릭터가 보유한 스킬표기
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
            body = 3;
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

    public bool isBound(int x, int y, string name, string type)
    {
        bool isCrash = false;

        // 장애물 크기에 따라
        isCrash = currPos - y > 0;

        // 천장 장애물
        if(name == "Hang")
        {
            isCrash = body != 2;
        }
        else if(type == "Jelly" || type == "Potion")
        {
            // 히트박스
            isCrash = currPos >= y && currPos - body <= y;
        }



        return x >= 3 && x <= xPos && isCrash ;
    }


    public void OnDash(Skill skill)
    {
        xPos = 9;
    }
    public void OffDash(Skill skill)
    {
        xPos = 5;
    }


    public void Dead()
    {
        isFall = true;

        if (deadTimer > 0.1)
        {
            startPos += 2;
            xPos += 1;
            deadTimer = 0;
        }

        isDead = startPos > 16;
    }
}