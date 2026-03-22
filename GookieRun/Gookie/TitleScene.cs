using System;
using System.Collections.Generic;
using System.Text;
using Framework.Engine;
public class TitleScene : Scene
{
    public event GameAction StartRequested;
    // 각 알파벳의 5x5 비트맵 데이터를 정의합니다.
    static Dictionary<char, int[,]> Alphabet = new Dictionary<char, int[,]>
    {
        ['G'] = new int[,] { { 0, 1, 1, 1, 0 }, 
                             { 1, 0, 0, 0, 0 }, 
                             { 1, 0, 1, 1, 1 }, 
                             { 1, 0, 0, 0, 1 }, 
                             { 0, 1, 1, 1, 0 } },

        ['o'] = new int[,] { { 0, 0, 0, 0, 0 }, 
                             { 0, 1, 1, 1, 0 }, 
                             { 1, 0, 0, 0, 1 }, 
                             { 1, 0, 0, 0, 1 }, 
                             { 0, 1, 1, 1, 0 } },

        ['k'] = new int[,] { { 1, 0, 0, 0, 1 }, 
                             { 1, 0, 0, 1, 0 }, 
                             { 1, 1, 1, 0, 0 }, 
                             { 1, 0, 0, 1, 0 }, 
                             { 1, 0, 0, 0, 1 } },

        ['i'] = new int[,] { { 0, 0, 1, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 1, 0, 0 },
                             { 0, 0, 1, 0, 0 },
                             { 0, 0, 1, 0, 0 } },

        ['e'] = new int[,] { { 0, 0, 0, 0, 0 },
                             { 0, 1, 1, 1, 0 },
                             { 1, 1, 1, 1, 1 },
                             { 1, 0, 0, 0, 0 },
                             { 0, 1, 1, 1, 0 } },

        ['R'] = new int[,] { { 1, 1, 1, 1, 0 },
                             { 1, 0, 0, 0, 1 },
                             { 1, 1, 1, 1, 0 },
                             { 1, 0, 0, 1, 0 },
                             { 1, 0, 0, 0, 1 } },

        ['u'] = new int[,] { { 0, 0, 0, 0, 0 },
                             { 1, 0, 0, 0, 1 },
                             { 1, 0, 0, 0, 1 },
                             { 1, 0, 0, 0, 1 },
                             { 0, 1, 1, 1, 1 } },

        ['n'] = new int[,] { { 0, 0, 0, 0, 0 },
                             { 1, 1, 1, 1, 0 },
                             { 1, 0, 0, 0, 1 },
                             { 1, 0, 0, 0, 1 },
                             { 1, 0, 0, 0, 1 } },

        [' '] = new int[,] { { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 } }

    };


    public override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.Enter))
        {
            StartRequested?.Invoke(); // 다음 씬으로 넘어가기 구현
        }
    }

    public override void Draw(ScreenBuffer buffer)
    {
        string input = "Gookie Run"; // 출력할 대상
        int height = 5; // 폰트 높이 (5줄)

        Console.Title = "Gookie Run";
        Console.ForegroundColor = ConsoleColor.Yellow;
        buffer.DrawBox(14, 1, 75, 10, ConsoleColor.White);

        int startX = 17; // 시작 X 좌표
        int startY = 3; // 시작 Y 좌표

        foreach (char c in input)
        {
            if (Alphabet.ContainsKey(c))
            {
                int[,] glyph = Alphabet[c];

                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        if (glyph[row, col] == 1)
                        {
                     
                            buffer.SetCell(startX + col, startY + row, '*', ConsoleColor.Yellow);
                        }
                    }
                }
                startX += 7; // 자간 포함 다음 글자 위치 (5칸 + 자간 2칸)
            }
        }

        Console.ResetColor();

        buffer.WriteTextCentered(12, "Press Any Key to Start");

    }





    public override void Load()
    {

    }

    public override void Unload()
    {

    }

}