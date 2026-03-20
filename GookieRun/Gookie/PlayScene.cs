using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class PlayScene : Scene
{
    public event GameAction PlayAgainRequested;

    private Map map;
    private Gookie gookie;
    private Item item;

    private bool isCrash = false;
    private bool isJelly = false;

    private int score = 0;
   

    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);

        // 쿠키 y좌표 비교
        isCrash = map.CrashReport(gookie.CurrentPosition - 3); // 머리에서 키만큼 빼기
        isJelly = map.isJelly;


        if (!isCrash && isJelly)
        {
            //일반젤리
            score += 10;


        }
        else if(!isCrash && isJelly)
        {
            // 왕곰젤리
            score += 50;
        }


    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        buffer.WriteText(1, 0, $"Score: {score}", ConsoleColor.Cyan);


    }

    public override void Load()
    {
        map = new Map(this);
        AddGameObject(map);

        gookie = new Gookie(this);
        AddGameObject(gookie);

        item = new Item(this);
        AddGameObject(item);
    }

    public override void Unload()
    {
        ClearGameObjects();

    }


}