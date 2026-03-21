using System;
using System.Threading;

namespace Framework.Engine
{
    public abstract class GameApp
    {
        private const int k_TargetFrameTime = 8;
        private bool _isRunning;

        protected ScreenBuffer Buffer { get; private set; } // 그리기

        public event GameAction GameStarted; 
        public event GameAction GameStopped;

        protected GameApp(int width, int height) // 콘솔 사이즈
        {
            Buffer = new ScreenBuffer(width, height); // 지정한 부분만 갱신됨
        }

        public void Run()
        {
            Console.CursorVisible = false;
            Console.Clear();

            // 초기화
            _isRunning = true;
            Initialize();
            GameStarted?.Invoke(); // 실행 시 불러와야하는 이벤트들

            int previousTime = Environment.TickCount;

            // 게임 루프
            while (_isRunning)
            {
                // 프레임 간격 보정
                int currentTime = Environment.TickCount; 
                float deltaTime = (currentTime - previousTime) / 1000f;
                previousTime = currentTime;


                Input.Poll();
                Update(deltaTime);
                Buffer.Clear();
                Draw();
                Buffer.Present();

                // 프레임 제한
                int elapsed = Environment.TickCount - currentTime;
                int sleepTime = k_TargetFrameTime - elapsed;
                if (sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }
            }

            // 게임 종료
            GameStopped?.Invoke();
            Console.CursorVisible = true;
            Console.ResetColor();
            Console.Clear();
        }

        protected void Quit()
        {
            _isRunning = false;
        }

        protected abstract void Initialize();
        protected abstract void Update(float deltaTime);
        protected abstract void Draw();
    }
}
