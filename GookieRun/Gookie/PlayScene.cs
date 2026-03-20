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



    private int score = 0;
   

    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);


    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        int playpos = gookie.CurrentPosition - gookie.body;

        buffer.WriteText(1, 0, $"Score: {score}", ConsoleColor.Cyan);
        buffer.WriteText(1, 16, $"Log - playerPos: {playpos}", ConsoleColor.Cyan);


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




}