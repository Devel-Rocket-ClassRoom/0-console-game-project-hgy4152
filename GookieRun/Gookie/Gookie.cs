using Framework.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
public class Gookie : GameObject
{
    private int _direction;
    private int startPos;
    private int currPos;
    private bool isJump;
    private bool isTop;
    private bool isSlide;


    private float _jumpTimer;

    private int body = 3;
    private int height = 3;
    private int jumpCount;


    public Gookie(Scene scene) : base(scene)
    {
        Name = "Gookie";

        startPos = 9;
    }

    public override void Draw(ScreenBuffer buffer)
    {
        buffer.FillRect(1, currPos, 2, body, '|');

    }

    public override void Update(float deltaTime)
    {
        
        _jumpTimer += deltaTime;
        currPos = startPos + _direction;

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
            _direction += 1;
            isSlide = true;
        }

        else if(isSlide && Input.IsKeyUp(ConsoleKey.DownArrow))
        {
            body = 3;
            isSlide = false;
            _direction = 0;


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
            if (isSlide)
            {
                _direction = 1;
            }
        }
        

        

    }


}