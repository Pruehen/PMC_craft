

static class ConsoleScreen
{
    const int maxLines = 9; // 최대 9줄까지 출력
    static Queue<string> dataQueue = new Queue<string>(maxLines);

    public static void Init()//기본 판 그리는 기능 + 초기화
    {
        for (int i = 36; i < 45; i++)
        {
            Console.SetCursorPosition(70, i);
            Console.Write('|');
        }
        Console.SetCursorPosition(0, 45);
        Console.Write("----------------------------------------------------------------------+");//- 70개

        AddData("//Processing complete");
        AddData("//Initiate Battlefield");
    }

    public static void AddData(string str, ConsoleColor color = ConsoleColor.White)
    {
        // 데이터 추가                               
        dataQueue.Enqueue(str);

        // 데이터 출력
        PrintData(color);
    }
    static void PrintData(ConsoleColor color)
    {
        // 최대 라인 수 이상의 데이터가 있으면 가장 오래된 데이터 삭제
        if (dataQueue.Count > maxLines)
        {
            dataQueue.Dequeue();
        }

        // 데이터 출력
        int index = 1;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        foreach (string data in dataQueue)
        {
            Console.SetCursorPosition(0, 35 + index);
            Console.Write("                                                                      ");
            Console.SetCursorPosition(0, 35 + index);
            if (dataQueue.Count == index)
            {
                Console.ForegroundColor = color;
            }
            Console.Write(data);
            index++;
        }
        Console.ResetColor();
    }
}