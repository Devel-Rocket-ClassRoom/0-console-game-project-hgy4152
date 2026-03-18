namespace Framework.Engine
{
    public delegate void GameAction();
    public delegate void GameAction<T>(T value);
}


// 게임 이벤트 실행
// 필요하면 매개변수나 반환형 있는것도 만들어 써도 됨