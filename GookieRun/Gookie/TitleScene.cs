using System;
using System.Collections.Generic;
using System.Text;

class TitleScene
{
    // 각 알파벳의 5x5 비트맵 데이터를 정의합니다.
    static Dictionary<char, int[,]> Alphabet = new Dictionary<char, int[,]>
    {
        ['G'] = new int[,] { { 0, 1, 1, 1, 0 }, { 1, 0, 0, 0, 0 }, { 1, 0, 1, 1, 1 }, { 1, 0, 0, 0, 1 }, { 0, 1, 1, 1, 0 } },
        ['o'] = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 }, { 1, 0, 0, 0, 1 }, { 1, 0, 0, 0, 1 }, { 0, 1, 1, 1, 0 } },
        ['k'] = new int[,] { { 1, 0, 0, 0, 1 }, { 1, 0, 0, 1, 0 }, { 1, 1, 1, 0, 0 }, { 1, 0, 0, 1, 0 }, { 1, 0, 0, 0, 1 } },
        ['i'] = new int[,] { { 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 1, 0, 0 } },
        ['e'] = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 }, { 1, 1, 1, 1, 1 }, { 1, 0, 0, 0, 0 }, { 0, 1, 1, 1, 0 } },
        ['R'] = new int[,] { { 1, 1, 1, 1, 0 }, { 1, 0, 0, 0, 1 }, { 1, 1, 1, 1, 0 }, { 1, 0, 0, 1, 0 }, { 1, 0, 0, 0, 1 } },
        ['u'] = new int[,] { { 0, 0, 0, 0, 0 }, { 1, 0, 0, 0, 1 }, { 1, 0, 0, 0, 1 }, { 1, 0, 0, 0, 1 }, { 0, 1, 1, 1, 1 } },
        ['n'] = new int[,] { { 0, 0, 0, 0, 0 }, { 1, 1, 1, 1, 0 }, { 1, 0, 0, 0, 1 }, { 1, 0, 0, 0, 1 }, { 1, 0, 0, 0, 1 } },
        [' '] = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } }
    };

    static void Main()
    {
        string input = "Gookie Run"; // 출력할 대상
        int height = 5; // 폰트 높이 (5줄)

        Console.Title = "Gookie Run Generator";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== [ 2D Array Font System ] ===\n");

        // 핵심 로직: 모든 글자의 '첫 번째 줄'을 먼저 다 찍고, 그 다음 '두 번째 줄'을 찍는 방식
        for (int row = 0; row < height; row++)
        {
            foreach (char c in input)
            {
                if (Alphabet.ContainsKey(c))
                {
                    int[,] glyph = Alphabet[c];
                    for (int col = 0; col < 5; col++)
                    {
                        // 1이면 '*' 출력, 0이면 공백(' ') 출력
                        Console.Write(glyph[row, col] == 1 ? "*" : " ");
                    }
                    Console.Write("  "); // 글자 사이의 자간
                }
            }
            Console.WriteLine(); // 한 줄(Row) 출력이 끝나면 개행
        }

        Console.ResetColor();
        Console.WriteLine("\n================================");
        Console.WriteLine("출력이 완료되었습니다. (Press any key)");
        Console.ReadKey();
    }
}