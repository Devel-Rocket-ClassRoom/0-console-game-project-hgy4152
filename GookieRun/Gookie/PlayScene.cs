using Framework.Engine;
using GookieRun.Gookie;
using System;
using System.Collections.Generic;
using System.Text;

class PlayScene : Scene
{
    public event GameAction PlayAgainRequested;

    private Map map;
    private Gookie gookie;
    

    private bool isCrash = false;
    private float immuneTime = 0f;

    private int score = 0;
    private int scoreLog = 0;
   

    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);
        

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

                pos.destroy = skillActive();

                return Judge(pos.Type, pos.Name);
            }


            return false;
        });




    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        int playposB = gookie.CurrentPosition;
        int playposT = gookie.CurrentPosition - gookie.body;

        buffer.WriteText(1, 0, $"Score: {score}", ConsoleColor.Cyan);
        buffer.WriteText(1, 15, $"Obstarcle: {scoreLog} / {immuneTime}", ConsoleColor.Cyan);
        buffer.WriteText(1, 18, $"Log - direction: {gookie._direction}", ConsoleColor.Cyan);


    }

    public override void Load()
    {
        map = new Map(this);
        AddGameObject(map);


        gookie = new Gookie(this);
        AddGameObject(gookie);

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
            scoreLog++;
            isCrash = true;
            
        }

        return false;
    }

    bool skillActive()
    {
        return true;
    }
}