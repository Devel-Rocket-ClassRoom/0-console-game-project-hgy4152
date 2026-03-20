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

    private int score = 0;
   

    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);

        // 쿠키 y좌표 비교
        isCrash = map.CrashReport(gookie.CurrentPosition - gookie.body); // 머리에서 키만큼 빼기



    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        int playpos = gookie.CurrentPosition - gookie.body;

        buffer.WriteText(1, 0, $"Score: {score}", ConsoleColor.Cyan);
        buffer.WriteText(1, 16, $"Log - playerPos: {playpos}", ConsoleColor.Cyan);
        buffer.WriteText(1, 17, $"Log - ObsPos: {map.height - playpos}", ConsoleColor.Cyan);


    }

    public override void Load()
    {
        map = new Map(this);
        AddGameObject(map);
        map.OnJellyEat += Crash;

        gookie = new Gookie(this);
        AddGameObject(gookie);

        item = new Item(this);
        AddGameObject(item);
    }

    public override void Unload()
    {
        ClearGameObjects();

    }


    public void Crash()
    {
        if (!isCrash) // 장애물 충돌 중이 아닐 때만 점수 추가
        {
            if (gookie.CurrentPosition == gookie.StartPosition)
                score += 10; // 일반 젤리
            else if(map.height != 0)
                score += 50; // 왕 젤리 장애물 위를 지날 때만 점수 더 주게 끔 설정해줘야함
        }
    }

}