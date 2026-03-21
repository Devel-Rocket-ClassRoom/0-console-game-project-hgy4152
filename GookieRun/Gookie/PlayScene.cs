using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class PlayScene : Scene
{
    public event GameAction PlayAgainRequested;
    

    private Map map;
    private Gookie gookie;

    private bool isCrash = false;
    private bool isSkill = false;
    private float immuneTime = 0f;
    private float playTime = 0f;

    private int score = 0;
    private int health = 60;


    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);

        // 스킬 사용
        isSkill = gookie.isSkill;
        map.mapReaction(isSkill, gookie.skill);

        // 장애물 충돌 후 무적
        if (isCrash)
        {
            immuneTime += deltaTime;

            if(immuneTime > 2)
            {
                isCrash = false;
                immuneTime = 0f;
            }
           
        }

        // 먹은거나 부딫힌거 검사 및 제거
        map.mapObj.RemoveAll(pos =>
        {

            if (gookie.isBound(pos.X, pos.Y, pos.Name))
            {

                pos.destroy = isSkill;

                return Judge(pos.Type, pos.Name);
            }


            return false;
        });

        playTime += deltaTime;
        // 체력 자동 감소
        if (playTime > 1)
        {
            health -= 1;
            playTime = 0;
        }

    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        buffer.WriteText(1, 0, $"HP:", ConsoleColor.Green);
        buffer.DrawHLine(5, 0, health, '█', ConsoleColor.Green);

        buffer.WriteText(7, 16, $"Score: {score}", ConsoleColor.Cyan);

    }

    public override void Load()
    {
        map = new Map(this);
        AddGameObject(map);


        // 쿠키, 스킬 셋팅 등록
        gookie = new Gookie(this);
        AddGameObject(gookie);
        AddGameObject(gookie.skill);


        Console.OutputEncoding = System.Text.Encoding.UTF8;

    }

    public override void Unload()
    {
        ClearGameObjects();

    }


    bool Judge(string type , string name)
    {
        if(type == "Jelly")
        {
            if(name == "Normal")
            {
                score += 10;
            }
            else
            {
                score += 50;
            }

            return true;
            
        }

        else if(type == "Obstarcle" && !isCrash)
        {

            isCrash = true;

            // 파괴 점수
            if(isSkill)
            {
                score += 100;
            }
            else
            {
                health -= 5;
            }
            
        }

        return false;
    }


}