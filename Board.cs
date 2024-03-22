using PMC_craft;
using Units;

class Board
{
    const int WIDHT = 16;
    const int HAIGHT = 16;

    Position cursor = new Position(0, 0);//현재 위치한 칸
    Position select;//선택된 칸
    bool isSelect = false;//현재 선택된 칸이 있는지?
    StateScreen stateConsoleScreen;//상태창
    static int turn = 1;

    static Unit[,] unitPositionArray = new Unit[WIDHT, HAIGHT];//유닛들의 위치
    public static Position? FindUnit(Unit unit)
    {
        for (int i = 0; i < HAIGHT; i++)
        {
            for (int j = 0; j < WIDHT; j++)
            {
                if (unitPositionArray[i,j] == unit)
                {
                    return new Position(i, j);
                }
            }
        }
        return null;
    }

    public Board(StateScreen stateScreen, List<Unit> friendlyUnits, List<Unit> hostileUnits)
    {
        this.stateConsoleScreen = stateScreen;
        select = cursor;
        DrawBase();
        CursorMove(0, 0);
        turn = 1;

        UnitInit(friendlyUnits, hostileUnits);
        StateScreen.SetEnemyUnitCountText();
        if(Unit.GameWinCheck())
        {
            GameEnd(true);
        }
        if(Unit.GameDefeatCheck())
        {
            GameEnd(false);
        }
    }
    void UnitInit(List<Unit> friendlyUnits, List<Unit> hostileUnits)//유닛 위치 초기화. 최초 1회만 실행.
    {
        foreach (Unit unit in friendlyUnits)
        {
            Position position = unit.GetPosition();
            unitPositionArray[position.x, position.y] = unit;
            DrawUnit(unit);
        }
        foreach (Unit unit in hostileUnits)
        {
            Position position = unit.GetPosition();
            unitPositionArray[position.x, position.y] = unit;
            DrawUnit(unit);
        }
    }
    public void NextTurn()
    {
        foreach (Unit unit in unitPositionArray)
        {
            if (unit != null)
            {
                unit.AiControl();                
                DrawUnit(unit);                
            }
        }
        foreach (Unit unit in unitPositionArray)
        {
            if (unit != null)
            {
                unit.NextTurn();                                
            }
        }
        foreach (Unit unit in unitPositionArray)
        {
            if (unit != null)
            {
                unit.EnemyVisualSet();
                DrawUnit(unit);
            }
        }
        turn++;
        Unit.AllEnemyVisualSet();
        ConsoleScreen.AddData("------------------------------------------------------------");
        ConsoleScreen.AddData($"다음 턴으로 진행. 현재 {turn}턴");        
    }

    public Unit GetUnitData()//커서가 위치한 칸의 유닛 데이터 반환
    {
        return unitPositionArray[cursor.x, cursor.y];
    }
    public static Unit GetUnitData(Position position)//입력한 포지션에 위치한 칸의 유닛 데이터 반환
    {
        try
        {
            return unitPositionArray[position.x, position.y];
        }
        catch (Exception e)
        {
            return null;
        }
    }

    void DrawBase()//기본 판 그리기. 최초 1회만 실행.
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        for (int i = 0; i < WIDHT + 1; i++)
        {
            for (int j = 0; j < HAIGHT + 1; j++)
            {
                Console.SetCursorPosition(i * 4 + 3, j * 2 + 1);
                Console.Write('+');
            }
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        for (int i = 0; i < WIDHT; i++)
        {
            Console.SetCursorPosition(i * 4 + 5, 0);
            Console.Write(i + 1);
        }
        for (int i = 0; i < HAIGHT; i++)
        {
            Console.SetCursorPosition(0, i * 2 + 2);
            Console.Write((char)(i + 'A'));
        }
        Console.ResetColor();

        for (int i = 0; i < 35; i++)
        {
            Console.SetCursorPosition(70, i);
            Console.Write('|');
        }
        Console.SetCursorPosition(0, 35);
        Console.Write("----------------------------------------------------------------------+");//- 70개
    }

    public void CursorMove(int moveX, int moveY)//흰색 커서 이동하는 기능
    {
        DeletePoint(cursor);
        cursor = Position.Clamped(cursor + new Position(moveX, moveY));
        if (isSelect)
        {
            DrawPoint(select, ConsoleColor.Green);
        }
        DrawPoint(cursor, ConsoleColor.Yellow);

        stateConsoleScreen.SetSelectedPosition(cursor);
        stateConsoleScreen.SetPositionUnitText(unitPositionArray[cursor.x, cursor.y]);        
    }
    public void CursorMove_OrderMode(int moveX, int moveY)//명령 모드일 때, 흰색 커서 이동하는 기능. 영역을 표시하는 기능이 추가되어있음.
    {        
        bool isInArea = false;
        if (orderPositionArray != null)
        {
            foreach (Position item in orderPositionArray)
            {
                if (item == cursor)
                {
                    isInArea = true;
                    break;
                }
            }
        }
        if (!isInArea)
        {
            DeletePoint(cursor);
        }

        DrawArea();

        cursor = Position.Clamped(cursor + new Position(moveX, moveY));
        if (isSelect)
        {
            DrawPoint(select, ConsoleColor.Green);
        }
        DrawPoint(cursor, ConsoleColor.Yellow);

        stateConsoleScreen.SetSelectedPosition(cursor);
        stateConsoleScreen.SetPositionUnitText(unitPositionArray[cursor.x, cursor.y]);

        Unit selectUnit = GetUnitData(select);
        List<Position> rount = selectUnit.ReserveMoveSimulation(cursor);
        DeleteCircle();
        if (rount.Count > 0)
        {            
            for (int i = 0; i < rount.Count; i++)
            {
                if (i == rount.Count - 1)
                {
                    DrawCircle(rount[i], ConsoleColor.Yellow);
                }
                else
                {
                    DrawCircle(rount[i], ConsoleColor.Gray);
                }
            }
        }
    }
    public bool SetSelectPosition()//칸 선택하는 기능. 유닛이 있으면 그 유닛을 선택함.
    {
        if (GetUnitData(cursor) != null && GetUnitData(cursor).visualLevel != 0)
        {
            DeletePoint(select);
            select = cursor;
            DrawPoint(select, ConsoleColor.Green);
            isSelect = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DeleteSelectPosition()//칸의 선택을 해제하는 기능
    {
        isSelect = false;
        DeletePoint(select);
    }

    public Position GetCursor() { return cursor; }//현재 커서 위치를 반환
    public Position GetSelectedPosition() { return select; }//현재 선택 위치를 반환

    public void DrawPoint(Position position, ConsoleColor consoleColor)//원하는 위치와 원하는 색상의 네모 박스를 그리는 기능.
    {
        Console.ForegroundColor = consoleColor;
        Console.SetCursorPosition((position.x + 1) * 4, (position.y + 1) * 2 - 1);
        Console.Write("---");
        Console.SetCursorPosition((position.x + 1) * 4, (position.y + 1) * 2 + 1);
        Console.Write("---");
        Console.SetCursorPosition((position.x + 1) * 4 - 1, (position.y + 1) * 2);
        Console.Write('|');
        Console.SetCursorPosition((position.x + 1) * 4 + 3, (position.y + 1) * 2);
        Console.Write('|');
        Console.ResetColor();
    }
    Stack<Position> circlePosStack = new Stack<Position>();
    public void DrawCircle(Position position, ConsoleColor consoleColor)//원하는 위치에 원을 그리는 기능
    {
        Console.ForegroundColor = consoleColor;
        Console.SetCursorPosition((position.x + 1) * 4, (position.y + 1) * 2);
        Console.Write(" O");
        circlePosStack.Push(position);
        Console.ResetColor();
    }
    public void DeleteCircle()
    {
        while (circlePosStack.Count > 0)
        {
            Position position = circlePosStack.Pop();
            Console.SetCursorPosition((position.x + 1) * 4 + 1, (position.y + 1) * 2);
            Console.Write(" ");
        }
    }
    public void DeletePoint(Position position)//위치한 네모 박스를 지우는 기능.
    {
        Console.SetCursorPosition((position.x + 1) * 4, (position.y + 1) * 2 - 1);
        Console.Write("   ");
        Console.SetCursorPosition((position.x + 1) * 4, (position.y + 1) * 2 + 1);
        Console.Write("   ");
        Console.SetCursorPosition((position.x + 1) * 4 - 1, (position.y + 1) * 2);
        Console.Write(' ');
        Console.SetCursorPosition((position.x + 1) * 4 + 3, (position.y + 1) * 2);
        Console.Write(' ');
    }
    public static void DrawUnit(Unit unit)//유닛 코드명을 표기하는 기능.
    {
        DeleteUnit(unit.position);
        ConsoleColor unitColor;
        if(unit.isFriendly)
        {
            if(unit.CanAction() == false)
            {
                unitColor = ConsoleColor.DarkBlue;
            }
            else
            {
                unitColor = ConsoleColor.Cyan;
            }
        }
        else
        {
            if (unit.CanAction() == false)
            {
                unitColor = ConsoleColor.DarkRed;
            }
            else
            {
                unitColor = ConsoleColor.Red;
            }
        }
        Console.ForegroundColor = unitColor;
        Console.SetCursorPosition((unit.GetPosition().x + 1) * 4, (unit.GetPosition().y + 1) * 2);
        Console.Write(unit.codeName);
        Console.ResetColor();
    }
    public static void DeleteUnit(Position position)//유닛 코드명을 지우는 기능.
    {
        Console.SetCursorPosition((position.x + 1) * 4, (position.y + 1) * 2);
        Console.Write("   ");
    }
    public void UpdateUnitPosition()//현재 선택한 유닛을 그 유닛의 실제 위치로 이동시키는 기능.
    {
        Unit temp = unitPositionArray[select.x, select.y];
        DeleteUnit(select);
        unitPositionArray[select.x, select.y] = null;
        unitPositionArray[temp.GetPosition().x, temp.GetPosition().y] = temp;
        DrawUnit(temp);
    }
    public void UpdateUnitPosition(Position position)//position에 위치한 유닛을 실제 위치로 이동시키는 기능.
    {
        Unit temp = unitPositionArray[position.x, position.y];
        DeleteUnit(position);
        unitPositionArray[position.x, position.y] = null;
        if (temp != null && !temp.isDestroy)
        {
            unitPositionArray[temp.GetPosition().x, temp.GetPosition().y] = temp;
            DrawUnit(temp);
        }
    }
    public static void UpdateUnit(Unit unit)//매개변수의 유닛 상태를 업데이트하는 기능
    {
        Position? position = FindUnit(unit);//유닛의 보드 위치를 받아옴
        if(position != null) 
        {            
            DeleteUnit(position.Value);
            unitPositionArray[position.Value.x, position.Value.y] = null;//보드 위치의 유닛을 지움
            if (!unit.isDestroy)
            {
                unitPositionArray[unit.GetPosition().x, unit.GetPosition().y] = unit;
                DrawUnit(unit);
            }
            else
            {
                StateScreen.SetEnemyUnitCountText();
            }
        }               
        else
        {
            ConsoleScreen.AddData($"{unit.realName} 유닛을 이동할 수 없습니다.");
        }
    }

    Position[] orderPositionArray;
    public void SetOrderArea(Position[] positions)
    {
        if (positions != null)
        {
            orderPositionArray = new Position[positions.Length];//값 복사해서 할당
            for (int i = 0; i < positions.Length; i++)
            {
                orderPositionArray[i] = positions[i];
            }
        }
    }
    public void DrawArea()//유닛의 이동 가능 범위, 사거리 등을 시각적으로 표시하기 위한 기능
    {
        if (orderPositionArray != null)
        {
            for (int i = 0; i < orderPositionArray.Length; i++)
            {
                if (orderPositionArray[i].x > 15 || orderPositionArray[i].x < 0 || orderPositionArray[i].y > 15 || orderPositionArray[i].y < 0)
                    continue;
                DrawPoint(orderPositionArray[i], ConsoleColor.Gray);
            }
        }
    }
    public void DeleteArea()
    {
        if (orderPositionArray != null)
        {
            for (int i = 0; i < orderPositionArray.Length; i++)
            {
                if (orderPositionArray[i].x > 15 || orderPositionArray[i].x < 0 || orderPositionArray[i].y > 15 || orderPositionArray[i].y < 0)
                    continue;
                DeletePoint(orderPositionArray[i]);
            }
            orderPositionArray = null;
        }
        circlePosStack.Clear();
    }

    public static void GameEnd(bool isWin)
    {
        for (int i = 0; i < 10; i++)
        {
            Console.SetCursorPosition(16, 10 + i);
            Console.Write("                                        ");           
        }
        Console.SetCursorPosition(16, 10);
        Console.Write("----------------------------------------");
        Console.SetCursorPosition(16, 11);
        if(isWin)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"                -승리-                ");
            UserData.LevelUp();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"                -패배-                ");
            UserData.LevelDown();
        }
        Console.ResetColor();

        int killCount = Unit.KillCount();
        int killBonus = 20 * killCount;
        int turnBonus = (200 - turn * 5 > 0 && isWin) ? 200 - turn * 5 : 0;
        int difficultyBonus = isWin ? (killBonus + turnBonus) * (UserData.level-1) : 0;
        int totalBonus = killBonus + turnBonus + difficultyBonus;

        Console.SetCursorPosition(16, 12);
        Console.Write($"처치한 유닛 :       {killCount}");
        Console.SetCursorPosition(16, 13);
        Console.Write($"소모된 턴 :         {turn}");
        Console.SetCursorPosition(16, 14);
        Console.Write($"유닛 처치 보너스 :  {killBonus}￠");
        Console.SetCursorPosition(16, 15);
        Console.Write($"소요 시간 보너스 :  {turnBonus}￠");
        Console.SetCursorPosition(16, 16);
        Console.Write($"난이도 보너스 :     {difficultyBonus}￠");
        Console.SetCursorPosition(16, 17);
        Console.Write($"총 자금 획득량 :    {totalBonus}￠");
        Console.SetCursorPosition(16, 20);
        Console.Write("----------------------------------------");
        Console.SetCursorPosition(16, 21);
        Console.Write("           >> PRESS A KEY <<            ");

        UserData.UseCredit(-totalBonus);

        for (int i = 0; i < HAIGHT; i++)//게임이 끝나면 보드 클리어
        {
            for (int j = 0; j < WIDHT; j++)
            {
                if (unitPositionArray[i, j] != null)
                {
                    unitPositionArray[i, j] = null;
                }
            }
        }

        Console.ReadKey();
        Program.GameSceneChange();
        UserData.SaveData();
    }
}
