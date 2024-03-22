using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PMC_craft
{
    static class TutoDataManager
    {
        static string dataPath = @"C:\Users\KGA\Desktop\PMC_craft\Data\TutorialText.txt";
        static Dictionary<string, string> tutoData = null;
        public static void PrintTuto(string key)
        {
            string data;
            if (tutoData == null)
            {
                if (File.Exists(dataPath))
                {
                    var fileData = File.ReadAllText(dataPath);
                    tutoData = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileData);
                }
                else
                {
                    tutoData = new Dictionary<string, string>();

                }                
            }
            if (tutoData.ContainsKey(key))
            {
                data = tutoData[key];
            }
            else
            {
                data = $"{key} 은(는) 유효하지 않은 key입니다.";
                tutoData.Add(key, data);
                var _data = JsonConvert.SerializeObject(tutoData, Formatting.Indented);
                File.WriteAllText(dataPath, _data);
            }

            for (int i = 0; i < 30; i++)
            {
                Console.SetCursorPosition(40, 6 + i);
                Console.Write("                                                                                                              ");
            }
            Console.SetCursorPosition(40, 6);
            string[] lines = data.Split('\n');
            foreach (string line in lines)
            {
                Console.WriteLine(line);
                Console.SetCursorPosition(40, Console.CursorTop); // 커서를 특정 위치(예: 40번째 열)로 이동
            }
        }
    }
    class UI_Tutorial : UI
    {
        public UI_Tutorial(string name, UI parent) : base(name, parent)
        {
            AddSubMenu(new UI_Tutorial_UnitType("유닛 종류", this));
            AddSubMenu(new UI_Tutorial_Formation("편제", this));
            AddSubMenu(new UI_Tutorial_State("스탯", this));
            AddSubMenu(new UI_Tutorial_Battle("전투", this));
        }
    }

    class UI_Tutorial_UnitType : UI
    {
        string data;
        public UI_Tutorial_UnitType(string name, UI parent) : base(name, parent)
        {
            AddSubMenu(new UI("보병", this));
            AddSubMenu(new UI("기계화 보병", this));
            AddSubMenu(new UI("견인포", this));
            AddSubMenu(new UI("자주포", this));
            AddSubMenu(new UI("기갑", this));
        }
        public override void EnterState()
        {
            base.EnterState();
            TutoDataManager.PrintTuto(GetSubMenu().GetName());
        }
        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    TutoDataManager.PrintTuto(GetSubMenu().GetName());
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    TutoDataManager.PrintTuto(GetSubMenu().GetName());
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Escape:
                    DeleteData(40, 6, 150, 40);
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }
    }

    class UI_Tutorial_Formation : UI
    {
        string data;
        public UI_Tutorial_Formation(string name, UI parent) : base(name, parent)
        {
            AddSubMenu(new UI("편제 기본", this));
            AddSubMenu(new UI("유닛 판정", this));
        }
        public override void EnterState()
        {
            base.EnterState();
            TutoDataManager.PrintTuto(GetSubMenu().GetName());
        }
        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    TutoDataManager.PrintTuto(GetSubMenu().GetName());
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    TutoDataManager.PrintTuto(GetSubMenu().GetName());
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Escape:
                    DeleteData(40, 6, 150, 40);
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }
    }

    class UI_Tutorial_State : UI
    {
        string data;
        public UI_Tutorial_State(string name, UI parent) : base(name, parent)
        {
            AddSubMenu(new UI("내구력", this));
            AddSubMenu(new UI("조직력", this));
            AddSubMenu(new UI("대인 공격력", this));
            AddSubMenu(new UI("대물 공격력", this));
            AddSubMenu(new UI("장갑 비율", this));
            AddSubMenu(new UI("장갑 수치", this));
            AddSubMenu(new UI("관통 수치", this));
        }
        public override void EnterState()
        {
            base.EnterState();
            TutoDataManager.PrintTuto(GetSubMenu().GetName());
        }
        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    TutoDataManager.PrintTuto(GetSubMenu().GetName());
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    TutoDataManager.PrintTuto(GetSubMenu().GetName());
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Escape:
                    DeleteData(40, 6, 150, 40);
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }
    }

    class UI_Tutorial_Battle : UI
    {
        string data;
        public UI_Tutorial_Battle(string name, UI parent) : base(name, parent)
        {
            AddSubMenu(new UI("이동", this));
            AddSubMenu(new UI("공격", this));
            AddSubMenu(new UI("후퇴", this));
            AddSubMenu(new UI("전멸", this));
            AddSubMenu(new UI("포위 섬멸", this));
            AddSubMenu(new UI("시야", this));            
        }
        public override void EnterState()
        {
            base.EnterState();
            TutoDataManager.PrintTuto(GetSubMenu().GetName());
        }
        public override void KeyInput(ConsoleKeyInfo keyInfo, UiController uiController)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateState(-1);
                    TutoDataManager.PrintTuto(GetSubMenu().GetName());
                    break;
                case ConsoleKey.DownArrow:
                    UpdateState(1);
                    TutoDataManager.PrintTuto(GetSubMenu().GetName());
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Escape:
                    DeleteData(40, 6, 150, 40);
                    uiController.ChangeState(GetParent());
                    break;
                default:
                    break;
            }
        }
    }
}
