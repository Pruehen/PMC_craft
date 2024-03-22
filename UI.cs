namespace PMC_craft
{

    class UI
    {
        public static CustomizedUnit selectedUnit;
        public static int selectedUnitIndex;
        public static int selectedSlot;
        public static void PrintCredit()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(10, 3);
            Console.WriteLine("                                                    ");
            Console.SetCursorPosition(10, 3);
            Console.WriteLine($"Credit : {UserData.credit} ￠");
            Console.ResetColor();            
        }

        int index = 0;
        public int Index() { return index; }
        string name;
        public string GetName() { return name; }        
        public void SetName(string value) { name = value; }
        public UI(string name, UI? parent)
        {
            this.name = name;
            parentMenu = parent;
        }

        List<UI> subMenu = new List<UI>();
        UI? parentMenu;
        public void AddSubMenu(UI uiType) { subMenu.Add(uiType); }
        public void RemoveAllSubMenu() { subMenu.Clear(); }
        public UI? GetSubMenu() { return (subMenu.Count == 0)? null : subMenu[index]; }
        public UI? GetParent() { return parentMenu; }

        public virtual void EnterState() { UpdateState(0); PrintCredit(); }
        public void UpdateState(int deltaIndex)
        {
            index += deltaIndex;
            if (index >= subMenu.Count) index = subMenu.Count - 1;
            else if (index < 0) index = 0;

            for (int i = 0; i < 15; i++)
            {
                Console.SetCursorPosition(7, 5 + i);
                Console.Write("                              ");
            }

            Console.SetCursorPosition(7, 5);
            Console.Write($"-{name}-");

            for (int i = 0; i <= subMenu.Count; i++)
            {                
                if (i < subMenu.Count)
                {
                    Console.SetCursorPosition(7, 7 + i);
                    if (i == index)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"▶{subMenu[i].name}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($"  {subMenu[i].name}");
                    }
                }
                else
                {
                    Console.SetCursorPosition(7, 8 + i);
                    if (parentMenu != null)
                    {
                        Console.Write($"  뒤로가기 : ESC");
                    }
                    else
                    {
                        Console.Write($"  종료 : ESC");
                    }
                }
            }
        }
        public virtual void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    break;
                case ConsoleKey.Enter:
                    uiController.ChangeState(GetSubMenu());
                    break;
                case ConsoleKey.Escape:
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }


        public void PrintEquipData(string keyData, string name, int startX, int startY)
        {            
            Equipment? data = null;
            if (keyData != null && UserData.Equipments.ContainsKey(keyData))
            {
                data = UserData.Equipments[keyData];
            }
            int y = startY;
            Console.SetCursorPosition(startX, y++);
            Console.Write(name);
            Console.SetCursorPosition(startX, y++);
            Console.Write("-----------------------------");
            string[] strings;
            if (data != null)
            {
                strings = data.GetEquipData();
            }
            else
            {
                strings = ["                              " ,
                    "                              " , 
                    "                              " , 
                    "        --데이터 없음--        " , 
                    "                              " , 
                    "                              " , 
                    "                              "];
            }
            for (int i = 0; i < strings.Length; i++)
            {
                Console.SetCursorPosition(startX, y++);
                Console.Write(strings[i]);
            }
            Console.SetCursorPosition(startX, y++);
            Console.Write("-----------------------------");
        }
        public void PrintUnitData(int index, int startX, int startY)
        {
            CustomizedUnit unitData = null;
            if (UserData.friendlyUnitDatas[index] != null)
            {
                unitData = UserData.friendlyUnitDatas[index];
            }

            DeleteData(startX, startY+2, startX + 30, startY + 10);
            int y = startY;
            Console.SetCursorPosition(startX, y++);
            Console.Write("유닛 상태표");
            Console.SetCursorPosition(startX, y++);
            Console.Write("-----------------------------");
            string[] strings;
            if (unitData != null)
            {
                strings = unitData.GetUnitDatas_DataOnly();
            }
            else
            {
                strings = ["                              " ,
                    "                              " ,
                    "                              " ,
                    "        --데이터 없음--        " ,
                    "                              " ,
                    "                              " ,
                    "                              " ,
                    "                              "];
            }
            for (int i = 0; i < strings.Length; i++)
            {
                Console.SetCursorPosition(startX, y++);
                Console.Write(strings[i]);
            }
            Console.SetCursorPosition(startX, y++);
            Console.Write("-----------------------------");
        }
        public void PrintUnitData(CustomizedUnit unitData, int startX, int startY)
        {
            DeleteData(startX, startY + 2, startX + 30, startY + 10);
            int y = startY;
            Console.SetCursorPosition(startX, y++);
            Console.Write("유닛 상태표");
            Console.SetCursorPosition(startX, y++);
            Console.Write("-----------------------------");
            string[] strings;
            if (unitData != null)
            {
                strings = unitData.GetUnitDatas_DataOnly();
            }
            else
            {
                strings = ["                              " ,
                    "                              " ,
                    "                              " ,
                    "        --데이터 없음--        " ,
                    "                              " ,
                    "                              " ,
                    "                              " ,
                    "                              "];
            }
            for (int i = 0; i < strings.Length; i++)
            {
                Console.SetCursorPosition(startX, y++);
                Console.Write(strings[i]);
            }
            Console.SetCursorPosition(startX, y++);
            Console.Write("-----------------------------");
        }
        public void DeleteData(int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(' ');
                }
            }
        }
    }

    class UiController
    {
        UI currentUI;
        UI main;

        public enum UI_Type
        {
            MAIN,
            WEAPON,
            FORMATION,
        }

        public UiController()
        {
            main = new UI_Main("메인");

            ChangeState(main); // 초기 상태 설정
        }
        public void Update(ConsoleKeyInfo keyInfo)
        {
            currentUI.KeyInput(keyInfo, this);
        }

        public void ChangeState(UI? newState)
        {
            if (newState != null)
            {
                currentUI = newState;
                currentUI.EnterState();
            }
        }
        public void GameEnd()
        {
            Environment.Exit(0);
        }
    }
    class UI_Main : UI//메인 화면
    {
        public UI_Main(string name) : base(name, null)
        {
            AddSubMenu(new UI_Combet("전투", this));
            AddSubMenu(new UI_Weapon("장비", this));
            AddSubMenu(new UI_Formation("편제", this));
            AddSubMenu(new UI_Tutorial("튜토리얼", this));
        }
        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    break;
                case ConsoleKey.Enter:
                    uiController.ChangeState(GetSubMenu());
                    break;
                case ConsoleKey.Escape:
                    UserData.SaveData();
                    Console.Clear();
                    uiController.GameEnd();
                    break;
                default:
                    break;
            }
        }
    }
    class UI_Combet : UI//전투 준비 화면
    {
        public UI_Combet(string name, UI parent) : base(name, parent)
        {
            AddSubMenu(new UI($"스테이지 {UserData.level} 진입", this));
        }
        public override void EnterState()
        {
            RemoveAllSubMenu();
            AddSubMenu(new UI($"스테이지 {UserData.level} 진입", this));

            base.EnterState();
        }
        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    Program.GameSceneChange();
                    break;
                case ConsoleKey.Escape:
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }
    }
    class UI_Weapon : UI//장비 관리 화면
    {
        public UI_Weapon(string name, UI parent) : base(name, parent) 
        {
            AddSubMenu(new UI_WeaponShop("장비 상점", this));
            AddSubMenu(new UI_WeaponView("보유 장비 일람", this));
        }
    }
    class UI_WeaponShop : UI//장비 구매 화면
    {
        List<string> indexer = new List<string>();//표시할 무기에 인덱스로 접근하기 위한 인덱서        
        public UI_WeaponShop(string name, UI parent) : base(name, parent)
        {
            //장비 열거 기능 추가            
            indexer.Clear();            

            int credit = UserData.credit;
            foreach (var item in UserData.Equipments)
            {
                if (item.Value.cost <= credit)
                {
                    AddSubMenu(new UI($"{item.Key}", this));
                    indexer.Add(item.Key);
                }
            }            
        }
        public override void EnterState()
        {
            RemoveAllSubMenu();
            indexer.Clear();            

            int credit = UserData.credit;
            foreach (var item in UserData.Equipments)
            {
                if (item.Value.cost <= credit)
                {
                    AddSubMenu(new UI($"{item.Key}", this));
                    indexer.Add(item.Key);
                }
            }

            base.EnterState();
            PrintEquipData((indexer.Count != 0) ? indexer[0] : null, "장비 상태표", 40, 6);
        }

        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);                    
                    PrintEquipData((indexer.Count != 0) ? indexer[Index()] : null, "장비 상태표", 40, 6);
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);                    
                    PrintEquipData((indexer.Count != 0) ? indexer[Index()] : null, "장비 상태표", 40, 6);
                    break;
                case ConsoleKey.Enter:
                    if (indexer.Count != 0)//구매 가능 장비가 있을 경우
                    {
                        UserData.Equipments[indexer[Index()]].TryBuyEquipment();//구매 시도
                        PrintCredit();
                    }
                    PrintEquipData((indexer.Count != 0) ? indexer[Index()] : null, "장비 상태표", 40, 6);
                    break;
                case ConsoleKey.Escape:
                    DeleteData(7, 5, 100, 50);
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }
    }
    class UI_WeaponView : UI//장비 일람 화면
    {        
        List<string> indexer = new List<string>();//표시할 무기에 인덱스로 접근하기 위한 인덱서
        int index = 0;
        public UI_WeaponView(string name, UI parent) : base(name, parent)
        {
            //장비 열거 기능 추가
            RemoveAllSubMenu();
            indexer.Clear();
            index = 0;

            if (UserData.AvailableEquips() != null)
            {
                foreach (var item in UserData.AvailableEquips())
                {
                    AddSubMenu(new UI($"{item.Key}", this));
                    indexer.Add(item.Key);
                }
            }
        }
        public override void EnterState()
        {
            RemoveAllSubMenu();
            indexer.Clear();
            index = 0;

            if (UserData.AvailableEquips() != null)
            {
                foreach (var item in UserData.AvailableEquips())
                {
                    AddSubMenu(new UI($"{item.Key}", this));
                    indexer.Add(item.Key);
                }
            }

            base.EnterState();
            PrintEquipData((indexer.Count != 0) ? indexer[0] : null,"장비 상태표", 40, 6);
        }

        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    if (index > 0) index--;
                    PrintEquipData((indexer.Count != 0) ? indexer[index] : null, "장비 상태표", 40, 6);
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    if (index < indexer.Count-1) index++;
                    PrintEquipData((indexer.Count != 0) ? indexer[index] : null, "장비 상태표", 40, 6);
                    break;
                case ConsoleKey.Escape:
                    DeleteData(40, 6, 100, 30);
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }
    }
    class UI_Formation : UI//편제 확인 화면. 유닛을 선택함.
    {
        public UI_Formation(string name, UI parent) : base(name, parent) 
        {
            RemoveAllSubMenu();
            for (int i = 0; i < 10; i++)
            {                
                string unitName;
                if (UserData.friendlyUnitDatas[i] != null)
                {
                    unitName = UserData.friendlyUnitDatas[i].realName;                    
                }
                else
                {                    
                    unitName = "EMPTY";
                }

                AddSubMenu(new UI_UnitSelect($"{i}번 유닛 : {unitName}", this, i));
            }            
        }
        public override void EnterState()
        {
            RemoveAllSubMenu();
            for (int i = 0; i < 10; i++)
            {
                string unitName;
                if (UserData.friendlyUnitDatas[i] != null)
                {
                    unitName = UserData.friendlyUnitDatas[i].realName;
                }
                else
                {
                    unitName = "EMPTY";
                }

                AddSubMenu(new UI_UnitSelect($"{i}번 유닛 : {unitName}", this, i));
            }

            base.EnterState();
            PrintUnitData(Index(), 40, 6);
        }

        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    PrintUnitData(Index(), 40, 6);
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    PrintUnitData(Index(), 40, 6);
                    break;
                case ConsoleKey.Enter:
                    selectedUnit = UserData.friendlyUnitDatas[Index()];
                    selectedUnitIndex = Index();
                    uiController.ChangeState(GetSubMenu());
                    break;
                case ConsoleKey.Escape:
                    DeleteData(40, 6, 70, 20);
                    uiController.ChangeState(GetParent());                    
                    break;
                default:
                    break;
            }
        }
    }

    class UI_UnitSelect : UI//편제 중 유닛 선택 화면. 유닛에 장착된 장비를 선택함.
    {
        int unitIndex;
        public UI_UnitSelect(string name, UI parent, int unitIndex) : base(name, parent)
        {
            this.unitIndex = unitIndex;
        }
        public override void EnterState()
        {            
            RemoveAllSubMenu();
            string unitName;
            if (UserData.friendlyUnitDatas[unitIndex] != null)
            {
                unitName = UserData.friendlyUnitDatas[unitIndex].realName;
            }
            else
            {
                unitName = "EMPTY";
            }
            SetName($"{unitIndex}번 유닛 : {unitName}");

            if (selectedUnit == null)
            {
                AddSubMenu(new UI_EquipSelect("유닛 추가", this));
            }
            else
            {
                string[] slot = selectedUnit.GetSlotItem();
                for (int i = 0; i < 9; i++)
                {
                    if (slot[i] == null)
                    {
                        AddSubMenu(new UI_EquipSelect("[빈 장비]", this));
                    }
                    else
                    {
                        AddSubMenu(new UI_EquipSelect(slot[i], this));
                    }
                }
            }

            PrintUnitData(selectedUnit, 40, 6);
            base.EnterState();
            PrintEquipData(GetSubMenu().GetName(), "장착된 장비 상태표", 75, 6);            
        }

        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    PrintEquipData(GetSubMenu().GetName(), "장착된 장비 상태표", 75, 6);
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    PrintEquipData(GetSubMenu().GetName(), "장착된 장비 상태표", 75, 6);
                    break;
                case ConsoleKey.Enter:
                    selectedSlot = Index();
                    uiController.ChangeState(GetSubMenu());
                    break;
                case ConsoleKey.Escape:
                    DeleteData(75, 6, 120, 20);
                    DeleteData(40, 6, 70, 20);
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }
    }

    class UI_EquipSelect : UI//선택한 장비를 장비할지 해제할지 선택
    {
        List<string> indexer = new List<string>();//표시할 무기에 인덱스로 접근하기 위한 인덱서        
        public UI_EquipSelect(string name, UI parent) : base(name, parent)
        {
            //장비 열거 기능 추가            
            indexer.Clear();
            if (UserData.AvailableEquips() != null)
            {
                foreach (var item in UserData.AvailableEquips())//현재 장착할 수 있는 아이템을 ui에 추가
                {
                    string key = item.Key;
                    if (GetName() != key)
                    {
                        AddSubMenu(new UI($"{key} 장착", this));
                        indexer.Add(key);
                    }
                }
            }
            if (GetName() != "유닛 추가")
            {
                AddSubMenu(new UI("장비 제거", this));
            }
        }
        public override void EnterState()
        {
            base.EnterState();
            if (indexer.Count != 0)
            {
                PrintEquipData(indexer[0], "선택 장비 상태표", 40, 6);
            }
            else
            {
                PrintEquipData(null, "선택 장비 상태표", 40, 6);
            }
        }

        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    if (indexer.Count != 0)
                    {
                        PrintEquipData(indexer[Index()], "선택 장비 상태표", 40, 6);
                    }
                    else
                    {
                        PrintEquipData(null, "선택 장비 상태표", 40, 6);
                    }
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    if (indexer.Count != 0 && Index() < indexer.Count)
                    {
                        PrintEquipData(indexer[Index()], "선택 장비 상태표", 40, 6);
                    }
                    else
                    {
                        PrintEquipData(null, "선택 장비 상태표", 40, 6);
                    }
                    break;
                case ConsoleKey.Enter:
                    if((Index() >= indexer.Count))//장비 탈착 선택
                    {
                        selectedUnit.RemoveItem(selectedSlot);
                        if(selectedUnit.IsSlotEmpty())
                        {
                            UserData.DeletefriendlyUnitData(selectedUnitIndex);
                            selectedUnit = null;
                        }
                    }
                    else//장비 부착 선택. 유닛이 null일 경우 유닛 생성
                    {                        
                        if(selectedUnit == null)
                        {
                            Console.SetCursorPosition(10, 20);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("생성할 유닛 이름을 입력하십시오 : ");
                            string name = Console.ReadLine();
                            selectedUnit = new CustomizedUnit(name, new string[9]);
                            UserData.friendlyUnitDatas[selectedUnitIndex] = selectedUnit;
                            Console.SetCursorPosition(10, 20);
                            Console.Write("                                                                             ");
                            Console.ResetColor();
                        }
                        selectedUnit.AddItem(indexer[Index()], selectedSlot);
                    }
                    DeleteData(40, 6, 100, 30);
                    uiController.ChangeState(GetParent());
                    break;
                case ConsoleKey.Escape:
                    DeleteData(40, 6, 100, 30);
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }
    }
}

