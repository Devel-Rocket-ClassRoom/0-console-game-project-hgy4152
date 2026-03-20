using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class PlayScene : Scene
{
    public event GameAction PlayAgainRequested;

    private Obstarcle obstarcle;
    private Gookie gookie;
    private Item item;

   

    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);


    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

    }

    public override void Load()
    {
        obstarcle = new Obstarcle(this);
        AddGameObject(obstarcle);

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