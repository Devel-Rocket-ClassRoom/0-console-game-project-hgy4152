using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class Item : GameObject
{
 
    public Item(Scene scene) : base(scene)
    {
        Name = "Item";
    }

    public override void Draw(ScreenBuffer buffer)
    {

    }

    public override void Update(float deltaTime)
    {
    }
}