using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;
public class Skill : GameObject
{

    public bool isCool = true;

    public float CoolTime;
    private float GameTime;
    private float skillTime;

    private float persist;

    public float speed;

    public Skill(Scene scene) : base(scene)
    {
        Name = "skill";

        CoolTime = 10; // 기본값
        persist = 3;
        speed = 0;
    }
    public override void Draw(ScreenBuffer buffer)
    {
        // 스킬 ui
        buffer.DrawBox(1, 15, 5, 5);

        // 유니코드 안될수도 있음
        // 순차적으로 차는거 보여줌
        for (int i = 0; i < GameTime / (CoolTime / 3) - 1; i++)
        {
            if (GameTime / (CoolTime / 3) >= 1)
                buffer.WriteText(2, 18 - i, "███", ConsoleColor.Green);
        }
    }


    public override void Update(float deltaTime)
    {

        if (isCool)
        {
            GameTime += deltaTime;

            if (GameTime > CoolTime)
            {
                isCool = false;
            }

        }

    }
    public bool Dash(float deltaTime)
    {

        CoolTime = 10;
        persist = 3;

        if (!isCool)
        {
            skillTime += deltaTime;
            // 스킬 로직 //
            speed = 0.01f;

            if (skillTime > persist)
            {
                skillTime = 0;
                isCool = true;
                GameTime = 0;

            }

            return true;

        }

        return false;

    }
    public void Dead()
    {
        GameTime = 0;
    }
}