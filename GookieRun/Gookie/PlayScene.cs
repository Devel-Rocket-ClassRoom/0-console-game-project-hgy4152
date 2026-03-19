using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class PlayScene : Scene
{
    public event GameAction PlayAgainRequested;

    private Obstarcle obstarcle;
    private Gookie gookie;
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
    }

    public override void Unload()
    {
        ClearGameObjects();

    }


}