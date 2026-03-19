using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;


public class GameManager : GameApp
{

    private SceneManager<Scene> _scenes = new SceneManager<Scene>();

    public GameManager() : base(100, 20)
    {

    }
    public GameManager(int width, int height) : base(width, height)
    {

    }



    protected override void Initialize()
    {
        ChangeToTitle();
    }

    protected override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.Escape))
        {
            Quit();
            return;
        }

        _scenes.CurrentScene?.Update(deltaTime);

    }
    protected override void Draw()
    {
        _scenes.CurrentScene?.Draw(Buffer);
    }
    private void ChangeToTitle()
    {
        var title = new TitleScene();
        title.StartRequested += ChangeToPlay; // 플레이 씬 실행 이벤트 추가
        _scenes.ChangeScene(title);
    }

    private void ChangeToPlay()
    {
        var play = new PlayScene();
        play.PlayAgainRequested += ChangeToTitle;
        _scenes.ChangeScene(play);
    }
}