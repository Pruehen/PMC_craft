class InstructionScreen
{
    public InstructionScreen()
    {
        for (int i = 36; i < 45; i++)
        {
            Console.SetCursorPosition(75, i);
            Console.Write('|');
            Console.SetCursorPosition(109, i);
            Console.Write('|');
        }
        Console.SetCursorPosition(75, 45);
        Console.Write("+---------------------------------+");

        SetInstructionScreen_Board();
    }

    public void SetInstructionScreen_Board()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(76, 36);
        Console.Write("     -조작법 설명-           ");
        Console.SetCursorPosition(76, 37);
        Console.Write("칸 이동   -> 방향키          ");
        Console.SetCursorPosition(76, 38);
        Console.Write("유닛 위치로-> Tab          ");
        Console.SetCursorPosition(76, 39);
        Console.Write("칸 선택   -> Enter          ");
        Console.SetCursorPosition(76, 40);
        Console.Write("턴 종료   -> SpaceBar       ");
        Console.SetCursorPosition(76, 41);
        Console.Write("메인      -> ESC            ");
        Console.ResetColor();
    }
    public void SetInstructionScreen_Select()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(76, 36);
        Console.Write("     -조작법 설명-          ");
        Console.SetCursorPosition(76, 37);
        Console.Write("명령 선택 -> 상하 방향키     ");
        Console.SetCursorPosition(76, 38);
        Console.Write("명령 지시 -> Enter          ");
        Console.SetCursorPosition(76, 39);
        Console.Write("선택 취소 -> ESC            ");
        Console.SetCursorPosition(76, 40);
        Console.Write("                           ");
        Console.SetCursorPosition(76, 41);
        Console.Write("                           ");

        Console.ResetColor();
    }
    public void SetInstructionScreen_Order()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(76, 36);
        Console.Write("     -조작법 설명-          ");
        Console.SetCursorPosition(76, 37);
        Console.Write("칸 이동   -> 방향키         ");
        Console.SetCursorPosition(76, 38);
        Console.Write("명령 수행 -> Enter          ");
        Console.SetCursorPosition(76, 39);
        Console.Write("선택 취소 -> ESC            ");
        Console.SetCursorPosition(76, 40);
        Console.Write("                            ");
        Console.SetCursorPosition(76, 41);
        Console.Write("                           ");
        Console.ResetColor();
    }
    public void SetInstructionScreen_Exit()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(76, 36);
        Console.Write("         -전투 이탈-         ");
        Console.SetCursorPosition(76, 37);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("진행 사항이 저장되지 않습니다 ");
        Console.SetCursorPosition(76, 38);
        Console.Write("정말로 이탈하시겠습니까?      ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(76, 39);
        Console.Write("예     -> Enter             ");
        Console.SetCursorPosition(76, 40);
        Console.Write("아니오 -> ESC               ");
        Console.SetCursorPosition(76, 41);
        Console.Write("                           ");
        Console.ResetColor();
    }
}