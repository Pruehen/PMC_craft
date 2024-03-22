using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Units;

namespace PMC_craft
{
    interface IControlType
    {
        void EnterState(CombatController player);
        void UpdateState(CombatController player, ConsoleKeyInfo keyInfo);        
    }
    class BoardState : IControlType//보드 선택 상태
    {
        public void EnterState(CombatController player)
        {
            player.instructionScreen.SetInstructionScreen_Board();            
        }

        public void UpdateState(CombatController player, ConsoleKeyInfo keyInfo)
        {
            // 방향키에 따라 CursorMove 호출
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    player.board.CursorMove(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    player.board.CursorMove(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    player.board.CursorMove(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    player.board.CursorMove(1, 0);
                    break;
                case ConsoleKey.Enter:
                    if (player.board.SetSelectPosition())
                    {
                        player.stateScreen.SetSelectedUnit(player.board.GetUnitData());
                        player.ChangeState(new OrderSelectState());
                    }
                    break;
                case ConsoleKey.Spacebar:
                    player.board.NextTurn();
                    player.board.CursorMove(0, 0);
                    break;
                case ConsoleKey.Escape:
                    player.ChangeState(new ExitState());
                    break;
                default:
                    break;
            }
        }
    }    
    class OrderSelectState : IControlType//유닛 명령 선택 상태
    {
        public void EnterState(CombatController player)
        {
            player.instructionScreen.SetInstructionScreen_Select();
        }

        public void UpdateState(CombatController player, ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    player.stateScreen.SelectOrderNum(-1);
                    break;
                case ConsoleKey.DownArrow:
                    player.stateScreen.SelectOrderNum(1);
                    break;
                case ConsoleKey.Enter:
                    if (player.stateScreen.isOrderList)
                    {
                        player.board.SetOrderArea(player.stateScreen.SelectOrder_ArrayReturn());
                        player.board.DrawArea();
                        player.ChangeState(new OrderPositionState());
                    }
                    break;
                case ConsoleKey.Escape:
                    player.board.DeleteSelectPosition();
                    player.stateScreen.SetSelectedUnit(null);
                    player.ChangeState(new BoardState());
                    player.board.CursorMove(0, 0);
                    break;
                default:
                    break;
            }
        }
    }    
    class OrderPositionState : IControlType//유닛 명령 지정 상태
    {
        public void EnterState(CombatController player)
        {
            player.instructionScreen.SetInstructionScreen_Order();
        }

        public void UpdateState(CombatController player, ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    player.board.CursorMove_OrderMode(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    player.board.CursorMove_OrderMode(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    player.board.CursorMove_OrderMode(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    player.board.CursorMove_OrderMode(1, 0);
                    break;
                case ConsoleKey.Enter:
                    player.stateScreen.OrderUnit(player.board.GetCursor(), player.board.GetUnitData());
                    //player.board.UpdateUnitPosition();//아군 유닛이 이동했을 경우, 그 유닛을 갱신
                    //player.board.UpdateUnitPosition(player.board.GetCursor());//적군 유닛이 패주했을 경우, 그 유닛을 갱신

                    player.board.DeleteSelectPosition();
                    player.board.DeleteArea();
                    player.board.CursorMove(0, 0);
                    player.stateScreen.SetSelectedUnit(null);
                    player.ChangeState(new BoardState());
                    break;
                case ConsoleKey.Escape:
                    player.board.DeleteSelectPosition();
                    player.board.DeleteArea();
                    player.board.CursorMove(0, 0);
                    player.stateScreen.SetSelectedUnit(null);
                    player.ChangeState(new BoardState());                    
                    break;
                default:
                    break;
            }
        }
    }
    class ExitState : IControlType//게임 종료 확인 상태
    {
        public void EnterState(CombatController player)
        {
            player.instructionScreen.SetInstructionScreen_Exit();
        }

        public void UpdateState(CombatController player, ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    Board.GameEnd(false);                    
                    break;
                case ConsoleKey.Escape:                    
                    player.ChangeState(new BoardState());
                    player.board.CursorMove_OrderMode(0, 0);
                    break;
                default:
                    break;
            }
        }
    }

    class CombatController
    {
        IControlType currentState;
        public Board board;
        public StateScreen stateScreen;
        public InstructionScreen instructionScreen = new InstructionScreen();//조작법 설명. 만들고 표시까지만 함.

        public CombatController(Board board, StateScreen stateScreen)
        {
            ChangeState(new BoardState()); // 초기 상태를 이동 상태로 설정
            this.board = board;
            this.stateScreen = stateScreen;
        }
        public void Update(ConsoleKeyInfo keyInfo)
        {
            currentState.UpdateState(this, keyInfo);
        }

        public void ChangeState(IControlType newState)
        {
            currentState = newState;
            currentState.EnterState(this);
        }
    }

    public enum GameScene
    {
        MANETENANCE,
        COMBAT
    }    

    internal class Program
    {
        static GameScene gameScene = GameScene.MANETENANCE;
        public static void GameSceneChange()
        {
            if(gameScene == GameScene.MANETENANCE)
            {
                gameScene = GameScene.COMBAT;
            }
            else
            {
                gameScene = GameScene.MANETENANCE;
            }
        }
        

        static void Main(string[] args)
        {
            UserData.LoadData();

            Console.CursorVisible = false;
            Console.SetWindowSize(150, 60);
            
            PrintIntroWdw();
                             
            while (true)
            {
                if (gameScene == GameScene.MANETENANCE)
                {
                    Console.Clear();
                    PrintMaintenanceWdw();

                    UiController uiController = new UiController();

                    while (gameScene == GameScene.MANETENANCE)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true); // 사용자 입력 받기
                        uiController.Update(keyInfo);
                        Thread.Sleep(16);
                    }
                }
                else if(gameScene == GameScene.COMBAT)
                {
                    List<Unit> hostileUnitDatas = new List<Unit>();
                    List<Unit> friendlyUnitDatas = new List<Unit>();

                    int dynamicDifficulty = UserData.level * UserData.level;//동적 난이도 = 레벨의 제곱
                    if(UserData.level % 5 == 0)//레벨이 5의 배수일 경우 
                    {
                        dynamicDifficulty *= 3;//동적 난이도를 3배수로 함
                    }
                    if (dynamicDifficulty < 0)
                        dynamicDifficulty = int.MaxValue;

                    Stack<Position> hostilePositions = RandomPositionListReturn();//랜덤한 포지션 스택

                    double n = Math.Log(dynamicDifficulty, 6);//동적 난이도의 log6 값을 구함.
                    int createIndex = (int)n;//소수점을 버림.
                    while(dynamicDifficulty > 0 && hostilePositions.Count > 0)
                    {
                        if (createIndex >= UserData.HostileUnitDatas.Count)//인덱스 범위를 초과할 경우 값을 강제로 낮춤
                        {
                            createIndex--;
                            continue;
                        }

                        if (dynamicDifficulty >= Math.Pow(6, createIndex)) //동적 난이도가 6의 index제곱보다 높을 경우, index의 유닛 추가
                        {
                            hostileUnitDatas.Add(UserData.HostileUnitDatas[createIndex].CreateUnit(hostilePositions.Pop(), false));
                            dynamicDifficulty -= (int)Math.Pow(6, createIndex);
                        }
                        else
                        {
                            createIndex--;
                        }
                    }


                    int positionX = 3;
                    foreach (CustomizedUnit unit in UserData.friendlyUnitDatas) 
                    {
                        if (unit != null)
                        {
                            friendlyUnitDatas.Add(unit.CreateUnit(new Position(positionX, 13), true));
                        }
                        positionX++;
                    }

                    Unit.UnitInit(friendlyUnitDatas, hostileUnitDatas);

                    Console.Clear();

                    StateScreen stateScreen = new StateScreen();//상태창 클래스. 현재 선택된 칸이나 유닛 등을 표시해줌.                                                                  
                    Board board = new Board(stateScreen, friendlyUnitDatas, hostileUnitDatas);//실제 게임 보드 클래스            
                    CombatController playerController = new CombatController(board, stateScreen);

                    ConsoleScreen.Init();

                    while (gameScene == GameScene.COMBAT)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true); // 사용자 입력 받기
                        playerController.Update(keyInfo);
                        Thread.Sleep(16);
                    }
                }
            }
        }
        static Stack<Position> RandomPositionListReturn()
        {
            List<Position> positionList = new List<Position>();
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    positionList.Add(new Position(x, y));
                }
            }
            Random rng = new Random();
            int n = positionList.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Position value = positionList[k];
                positionList[k] = positionList[n];
                positionList[n] = value;
            }

            Stack<Position> returnPositionStack = new Stack<Position>();
            for (int i = 0; i < positionList.Count; i++)
            {
                returnPositionStack.Push(positionList[i]);
            }
            return returnPositionStack;
        }

        static void PrintIntroWdw()
        {
            Console.SetCursorPosition(0, 5);
            Console.WriteLine("          ==================================================================================================");
            Console.WriteLine("            '########::'##::::'##::'######::::::::::::'######::'########:::::'###::::'########:'########:");
            Console.WriteLine("            ##.... ##: ###::'###:'##... ##::::::::::'##... ##: ##.... ##:::'## ##::: ##.....::... ##..:::");
            Console.WriteLine("            ##:::: ##: ####'####: ##:::..::::::::::: ##:::..:: ##:::: ##::'##:. ##:: ##:::::::::: ##:::::");
            Console.WriteLine("            ########:: ## ### ##: ##:::::::::::::::: ##::::::: ########::'##:::. ##: ######:::::: ##:::::");
            Console.WriteLine("            ##.....::: ##. #: ##: ##:::::::::::::::: ##::::::: ##.. ##::: #########: ##...::::::: ##:::::");
            Console.WriteLine("            ##:::::::: ##:.:: ##: ##::: ##:::::::::: ##::: ##: ##::. ##:: ##.... ##: ##:::::::::: ##:::::");
            Console.WriteLine("            ##:::::::: ##:::: ##:. ######::'#######:. ######:: ##:::. ##: ##:::: ##: ##:::::::::: ##:::::");
            Console.WriteLine("            ..:::::::::..:::::..:::......:::.......:::......:::..:::::..::..:::::..::..:::::::::::..:::::");
            Console.WriteLine("          ==================================================================================================");
            Console.WriteLine("\n\n\n\n\n                                               >> PRESS A KEY <<");
            Console.ReadKey();
            Console.Clear();
        }

        static void PrintMaintenanceWdw()
        {
            for (int i = 0; i < 50; i++)
            {
                Console.SetCursorPosition(5, i);
                Console.Write('=');
            }
        }        
    }
}