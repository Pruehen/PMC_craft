using Units;

class StateScreen
{
    public void SetSelectedPosition(Position position)//현재 선택된 좌표를 표시하는 기능
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(76, 2);
        Console.Write("                              ");//스페이스 30번
        Console.SetCursorPosition(76, 2);
        Console.Write($"ㅁ선택 좌표 : {Position.PrintPosition(position)}");
        Console.ResetColor();
    }
    public static void SetEnemyUnitCountText()//현재 남아있는 적 유닛의 수를 표시하는 기능
    {
        Console.SetCursorPosition(76, 5);
        Console.Write("                                 ");
        Console.SetCursorPosition(76, 5);
        Console.Write($"남은 적 유닛 : {Unit.HostileUnitActiveCount()}");
    }
    public void SetPositionUnitText(Unit unit)//유닛 종류 및 유닛의 상세 스탯을 표시하는 기능
    {
        Console.SetCursorPosition(76, 3);
        Console.Write("                              ");//스페이스 30번
        for (int i = 0; i < 10; i++)
        {
            Console.SetCursorPosition(76, 11 + i);
            Console.Write("                                 ");//스페이스 33번
        }
        Console.SetCursorPosition(76, 3);

        if (unit != null)
        {
            if (unit.isFriendly)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (!unit.isFriendly)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if(unit.visualLevel == 0)
            {
                Console.ResetColor();
            }
            Console.Write($"ㅁ위치 유닛 : {unit.realName}");

            string[] datas = unit.GetUnitDatas_InGame();
            for (int i = 0; i < datas.Length; i++)
            {
                Console.SetCursorPosition(76, 11 + i);
                Console.Write(datas[i]);
            }
        }
        else
        {
            Console.Write($"ㅁ위치 유닛 : 없음");
        }
        Console.ResetColor();
    }

    Unit? selectedUnit;
    public bool SetSelectedUnit(Unit? unit)//현재 위치한 곳을 선택했을 경우, 선택된 유닛을 표시하는 기능
    {
        Console.SetCursorPosition(76, 4);
        Console.Write("                                 ");//스페이스 33번
        Console.SetCursorPosition(76, 4);
        if (unit != null && unit.visualLevel > 0)
        {
            selectedUnit = unit;
            if (unit.isFriendly)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write($"ㅁ유닛 선택 : {selectedUnit.realName}");
        }
        else
        {
            selectedUnit = null;
        }
        Console.ResetColor();

        SetUnitOrderList();

        if (selectedUnit != null)
            return true;
        else
            return false;
    }
    string[]? orderList;
    public bool isOrderList { get; private set; }
    int selectIndex = 0;
    void SetUnitOrderList()//선택된 유닛의 명령 리스트를 가져오는 기능.
    {
        if (selectedUnit != null)
        {
            orderList = selectedUnit.GetOrderList();
            if (orderList != null)
            {
                isOrderList = true;
            }
            else
            {
                isOrderList = false;
            }
            selectIndex = 0;
            SelectOrderNum(0);
        }
        else
        {
            orderList = null;
            isOrderList = false;
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(76, 7 + i);
                Console.Write("                                 ");//스페이스 33번
            }
        }
    }

    public void SelectOrderNum(int value)//가져온 리스트의 명령 중 하나를 선택하는 기능.
    {
        selectIndex += value;
        if (selectIndex < 0) selectIndex = 1;
        else if (selectIndex > 1) selectIndex = 0;

        if (orderList != null)
        {
            for (int i = 0; i < orderList.Length; i++)
            {
                Console.SetCursorPosition(76, 7 + i);
                if (i == selectIndex)
                {
                    Console.Write($"▶명령 : {orderList[i]}");
                    selectedUnit.SetOrder(orderList[i]);
                }
                else
                {
                    Console.Write($"  명령 : {orderList[i]}");
                }
            }
        }
        else
        {
            Console.SetCursorPosition(76, 8);
            Console.Write("명령 불가능");
        }
    }
    public bool OrderUnit(Position position, Unit unit)
    {
        return selectedUnit.OrderUnit(position, unit);
    }

    public Position[] SelectOrder_ArrayReturn()//현재 선택한 명령에 따라 유닛의 명령 가능 범위를 리턴하는 함수.
    {
        if (orderList != null && selectedUnit != null)
        {
            if (selectIndex == 0)//이동 명령
            {
                return selectedUnit.GetCanMoveArray();
            }
            else if (selectIndex == 1)//공격 명령
            {
                return selectedUnit.GetCanAttackArray();
            }
            else//방어 명령
            {
                return [selectedUnit.position];//수정 필요
            }
        }
        else
        {
            return null;
        }
    }

    public StateScreen()
    {
        DrawBase();
        SetSelectedPosition(Position.zero());
    }

    void DrawBase()//기본 판 그리는 기능
    {
        for (int i = 1; i < 35; i++)
        {
            Console.SetCursorPosition(75, i);
            Console.Write('|');
            Console.SetCursorPosition(109, i);
            Console.Write('|');
        }
        Console.SetCursorPosition(75, 1);
        Console.Write("+---------------------------------+");//- 33개
        Console.SetCursorPosition(76, 6);
        Console.Write("---------------------------------");//- 33개
        Console.SetCursorPosition(76, 10);
        Console.Write("---------------------------------");//- 33개
        Console.SetCursorPosition(75, 35);
        Console.Write("+---------------------------------+");
    }
}