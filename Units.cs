using PMC_craft;
using Newtonsoft.Json;

namespace Units
{
    class AI
    {
        Unit controlUnit;

        public AI(Unit unit)
        {
            this.controlUnit = unit;
        }

        public void Order(Queue<Unit> targets)
        {
            for (int i = 0; i < 5; i++)//최대 5번의 행동 기회
            {
                if(targets.Count > 0 && targets.Peek().isDestroy)
                {
                    controlUnit.DequeueTarget();
                }
                //ConsoleScreen.AddData("공격 명령 실행");
                if (targets.Count == 0 || controlUnit.AttackUnit(targets.Peek().position, targets.Peek()) == false)//target에 대한 공격 시도. target이 null일 경우 자동으로 실패
                {
                    //target의 공격에 실패했을 경우, 자신의 공격 범위 안에 있는 적 중 가장 조직력이 낮은 적을 공격.
                    List<Unit> inRangeUnit = new List<Unit>();
                    Unit temp = null;

                    foreach (Position position in controlUnit.GetCanAttackArray())
                    {
                        temp = Board.GetUnitData(position);//자신의 공격 범위 안에 있는 유닛을 검색
                        if (temp != null && temp.isFriendly == true)//유닛이 있고, 그 유닛이 아군(적의 적)일 경우
                        {
                            inRangeUnit.Add(temp);//그 유닛을 공격 대상에 포함.
                        }
                    }

                    if(inRangeUnit.Count != 0)//공격 가능 대상이 존재할 경우
                    {
                        float minOrg = 10000;//탐색한 최소 조직력
                        foreach (Unit unit in inRangeUnit)//선형 탐색
                        {
                            if(unit.organization < minOrg)//탐색 대상이 검색한 대상의 조직력보다 낮은 조직력을 가지고 있을 경우
                            {
                                temp = unit;//그 대상을 저장 (최소 조직력 초기값이 10000이라 반드시 1번은 들어가게 되어있음)
                            }
                        }
                        controlUnit.AttackUnit(temp.position, temp);//탐색한 대상을 공격.
                    }
                }

                if (controlUnit.CanAction() && targets.Count > 0)//이동 가능시 이동 명령.
                {
                    //구버전 이동
                    //Position moveVector = Position.Normalized(target.position - controlUnit.position);
                    //controlUnit.MoveUnit(moveVector + controlUnit.position, null);

                    //if (Position.IsOutPosition(moveVector + controlUnit.position) || Board.GetUnitData(moveVector + controlUnit.position) != null)
                    //{
                    //    if (moveVector.x > 0)
                    //    {
                    //        if (!controlUnit.MoveUnit(new Position(1, 1) + controlUnit.position, null))
                    //        {
                    //            if (!controlUnit.MoveUnit(new Position(1, -1) + controlUnit.position, null))
                    //            {
                    //                controlUnit.MoveUnit(new Position(1, 0) + controlUnit.position, null);
                    //            }
                    //        }
                    //    }
                    //    else if (moveVector.x < 0)
                    //    {
                    //        if (!controlUnit.MoveUnit(new Position(-1, 1) + controlUnit.position, null))
                    //        {
                    //            if (!controlUnit.MoveUnit(new Position(-1, -1) + controlUnit.position, null))
                    //            {
                    //                controlUnit.MoveUnit(new Position(-1, 0) + controlUnit.position, null);
                    //            }
                    //        }
                    //    }
                    //    if (moveVector.y > 0)
                    //    {
                    //        if (!controlUnit.MoveUnit(new Position(1, 1) + controlUnit.position, null))
                    //        {
                    //            if (!controlUnit.MoveUnit(new Position(-1, 1) + controlUnit.position, null))
                    //            {
                    //                controlUnit.MoveUnit(new Position(0, 1) + controlUnit.position, null);
                    //            }
                    //        }
                    //    }
                    //    else if (moveVector.y < 0)
                    //    {
                    //        if (!controlUnit.MoveUnit(new Position(1, -1) + controlUnit.position, null))
                    //        {
                    //            if (!controlUnit.MoveUnit(new Position(-1, -1) + controlUnit.position, null))
                    //            {
                    //                controlUnit.MoveUnit(new Position(0, -1) + controlUnit.position, null);
                    //            }
                    //        }
                    //    }
                    //}

                    float minDistance = 10000;//유닛과의 거리가 최소가 되는 지점으로 이동할 것 (최대한 붙을 것)
                    Position movePos = Position.zero();
                    foreach (Position position in controlUnit.GetCanMoveArray())//유닛의 이동 가능 범위를 탐색
                    {
                        float distance = (float)Position.DistanceDouble(position, targets.Peek().position);
                        if (Board.GetUnitData(position) == null && distance < minDistance)
                            //위치에 다른 유닛이 없을 경우 + 유닛과의 거리가 기존 탐색 위치보다 더 가까울 경우
                        {
                            minDistance = distance;
                            movePos = position;                           
                        }
                    }
                    if(minDistance != 10000)//최소거리에 변화가 있을 경우
                    {
                        controlUnit.MoveUnit(movePos, null);
                    }
                    else
                    {
                        return;
                    }                        
                }
                else//행동 불가능하거나 target이 없는 경우
                {
                    return;//행동을 종료
                }

                //else
                //{
                //    //ConsoleScreen.AddData("수비 명령 실행");
                //    //bool isActionPossible = false;
                //    //foreach (Position position in controlUnit.GetCanAttackArray())
                //    //{
                //    //    Unit? inRangeUnit = Board.GetUnitData(position);
                //    //    if (inRangeUnit != null && controlUnit.isFriendly != inRangeUnit.isFriendly)
                //    //    {
                //    //        controlUnit.AttackUnit(inRangeUnit.position, inRangeUnit);
                //    //        isActionPossible = true;
                //    //        if (!controlUnit.CanAction())
                //    //            return;
                //    //    }
                //    //}
                //    //if (!isActionPossible)
                //    //    break;
                //}
            }
        }
    }

    class Unit
    {
        string _codeName;
        public string codeName
        {
            get
            {
                if (visualLevel == 0)
                    return "";
                if (visualLevel == 1)
                    return "???";
                else
                    return _codeName;
            }
            protected set
            {
                _codeName = value;
            }
        }//이름 (3글자)
        string _realName;
        public string realName 
        { 
            get
            {
                if (visualLevel == 0)
                    return "없음";
                if (visualLevel == 1)
                    return "???";
                else
                    return _realName;
            }
            protected set
            {
                _realName = value;
            }
        }//이름 (실제 이름)
        public Position position { get; protected set; }//현재 위치
        public bool isFriendly { get; protected set; }//아군인지 적인지
        public float hp { get; protected set; }//체력 - 실제 내구력. 0이 되면 유닛이 파괴된다.
        float maxHp;
        public float organization { get; protected set; }//조직력 - 얼마나 잘 조직되어있는지에 대한 척도. 0이 되면 패주하여 주변 칸으로 이동한다.
        protected float maxOrganization;
        public float softAttack { get; protected set; }//대인 공격력 - 장갑 비율이 낮은 적에게 입히는 피해량
        public float hardAttack { get; protected set; }//대물 공격력 - 장갑 비율이 높은 적에게 입히는 피해량
        public float hardness { get; protected set; }//장갑 비율 - 현재 유닛이 얼마나 장갑화되어있는지에 대한 척도
        public float armor { get; protected set; }//장갑 수치. 높으면 관통이 낮은 적에게 적은 피해를 받는다.
        public float piercing { get; protected set; }//관통 수치. 관통 수치보다 장갑 수치가 높을 경우 정상적으로 피해를 가할 수 있다.
        [JsonIgnore]
        public bool isDestroy { get; protected set; }//유닛이 파괴되었는지 여부
        public int visualLevel { get; protected set; }//해당 유닛이 얼마나 잘 보이는지 여부 0 안보임 1 조금보임 2 잘보임
        [JsonIgnore]
        public Func<Position, Unit, bool> OrderUnit;

        [JsonConstructor]
        public Unit(Position position, bool isFriendly, string codeName, string realName, float hp, float organization,
                     float softAttack, float hardAttack, float hardness, float armor, float piercing)
        {            
            this.position = position;
            this.isFriendly = isFriendly;

            this._codeName = codeName;
            this._realName = realName;
            this.hp = hp;
            maxHp = hp;

            this.organization = organization;
            maxOrganization = organization;

            this.softAttack = softAttack;
            this.hardAttack = hardAttack;
            this.hardness = hardness;
            this.armor = armor;
            this.piercing = piercing;
            isDestroy = false;

            if (!isFriendly)
            {
                unitAI = new AI(this);
                visualLevel = 2;
            }
            else
            {
                visualLevel = 2;
            }

            targetQueue = new Queue<Unit>();
            //minVisualArray = new Position[24];
            //maxVisualArray = new Position[25];
        }
        public Unit(Unit data)
        {            
            this.position = data.position;
            this.isFriendly = data.isFriendly;
            this._codeName = data._codeName;
            this._realName = data._realName;
            this.hp = data.hp;
            maxHp = hp;
            this.organization = data.organization;
            maxOrganization = organization;
            this.softAttack = data.softAttack;
            this.hardAttack = data.hardAttack;
            this.hardness = data.hardness;
            this.armor = data.armor;
            this.piercing= data.piercing;
            isDestroy = false;

            if (!isFriendly)
            {
                unitAI = new AI(this);
                visualLevel = 2;
            }
            else
            {
                visualLevel = 2;
            }

            targetQueue = new Queue<Unit>();
            //minVisualArray = new Position[24];
            //maxVisualArray = new Position[25];
        }
        public void Init()
        {
            hp = maxHp;
            organization = maxOrganization;
            if (!isFriendly) visualLevel = 0;
        }

        static List<Unit> hostileUnitDatas = new List<Unit>();
        static List<Unit> friendlyUnitDatas = new List<Unit>();
        static int friendlyUnitInitCount = 0;
        static int hostileUnitInitCount = 0;
        public static void UnitInit(List<Unit> friendlyUnits, List<Unit> hostileUnits)
        {            
            friendlyUnitDatas.Clear();
            foreach (Unit item in friendlyUnits)
            {
                friendlyUnitDatas.Add(item);
            }
            friendlyUnitInitCount = friendlyUnitDatas.Count;

            hostileUnitDatas.Clear();
            foreach (Unit item in hostileUnits)
            {
                hostileUnitDatas.Add(item);
            }
            hostileUnitInitCount = hostileUnitDatas.Count;
        }
        public static int HostileUnitActiveCount()
        {
            int count = 0;
            foreach (var item in hostileUnitDatas)
            {
                if(!item.isDestroy)
                {
                    count++;
                }
            }

            return count;
        }
        public static int HostileUnitAllCount()
        {
            return hostileUnitDatas.Count;
        }
        public static bool GameWinCheck()
        {
            if (hostileUnitDatas.Count == 0)
                return true;
            else
                return false;
        }
        public static bool GameDefeatCheck()
        {
            if (friendlyUnitDatas.Count == 0)
                return true;
            else
                return false;
        }
        public static int KillCount()
        {
            return hostileUnitInitCount - hostileUnitDatas.Count;
        }

        AI unitAI;
        public void AiControl()
        {
            if (unitAI == null)
                return;

            unitAI.Order(targetQueue);
            EnemyVisualSet();
        }
        public bool CanAction()
        {
            return organization >= maxOrganization * 0.2f;
        }

        public string[] GetUnitDatas_InGame()
        {
            string[] datas = new string[9];
            const int barSize = 10;

            if (visualLevel == 2)
            {
                datas[0] = $"내구력 :      {hp:F1}";
                char[] hpBar = new char[barSize];
                int hpRatio = (int)((hp / maxHp) * barSize);
                for (int i = 0; i < barSize; i++)
                {
                    if (hpRatio > i)
                    {
                        hpBar[i] = '■';
                    }
                    else
                    {
                        hpBar[i] = '□';
                    }
                }
                datas[1] = new string(hpBar);
                datas[2] = $"조직력 :      {organization:F1}";
                char[] orgBar = new char[barSize];
                int orgRatio = (int)((organization / maxOrganization) * barSize);
                for (int i = 0; i < barSize; i++)
                {
                    if (orgRatio > i)
                    {
                        orgBar[i] = '■';
                    }
                    else
                    {
                        orgBar[i] = '□';
                    }
                }
                datas[3] = new string(orgBar);
                datas[4] = $"대인 공격력 : {softAttack:F1}";
                datas[5] = $"대물 공격력 : {hardAttack:F1}";
                datas[6] = $"장갑 비율 :   {(hardness * 100):F1}%";
                datas[7] = $"장갑 수치 :   {armor:F1}";
                datas[8] = $"관통력 :      {piercing:F1}";
            }

            else if (visualLevel == 1)
            {
                datas[0] = $"내구력 :      ???";
                datas[1] = "NULL";
                datas[2] = $"조직력 :      {organization:F1}";                
                datas[3] = "NULL";
                datas[4] = $"대인 공격력 : ???";
                datas[5] = $"대물 공격력 : ???";
                datas[6] = $"장갑 비율 :   {(hardness * 100):F1}%";
                datas[7] = $"장갑 수치 :   {armor:F1}";
                datas[8] = $"관통력 :      ???";
            }

            return datas;
        }

        public string[] GetUnitDatas_DataOnly()
        {
            string[] datas = new string[7];            

            datas[0] = $"내구력 :      {hp:F1}";            
            datas[1] = $"조직력 :      {organization:F1}";            
            datas[2] = $"대인 공격력 : {softAttack:F1}";
            datas[3] = $"대물 공격력 : {hardAttack:F1}";
            datas[4] = $"장갑 비율 :   {(hardness * 100):F1}%";
            datas[5] = $"장갑 수치 :   {armor:F1}";
            datas[6] = $"관통력 :      {piercing:F1}";

            return datas;
        }

        protected string[] orderList = { "이동", "공격" };
        public string[]? GetOrderList()
        {
            if (isFriendly && CanAction())
            {
                return orderList;
            }
            else
                return null;
        }

        public enum OrderState
        {
            MOVE, ATTACK
        }
        public OrderState state = OrderState.MOVE;//외부에서 참조하는 용도. 내부에서 사용하지 않음.
        public void SetOrder(string order)
        {
            if (order == "이동")
            {
                OrderUnit = ReserveMove;
                state = OrderState.MOVE;
            }
            else if (order == "공격")
            {
                OrderUnit = AttackUnit;
                state = OrderState.ATTACK;
            }
        }
        public void NextTurn()
        {
            if (organization < maxOrganization)
            {
                organization += (maxOrganization - organization) * 0.3f + 10;
                if (organization > maxOrganization)
                {
                    organization = maxOrganization;
                }
            }
        }

        public void EnemyVisualSet()//적 주변에 플레이어 유닛이 있을 경우, 거리에 따라 visualLevel을 set함.
        {
            if (!isFriendly)
            {
                //SetVisualArray(); 렉이 너무 심해서 제거                
                bool softVisual = false;
                bool hardVisual = false;                

                foreach (Unit unit in friendlyUnitDatas) 
                {
                    int distance = Position.Distance(this.position, unit.position);
                    if (distance <= 2)//2칸 이내라면
                    {
                        hardVisual = true;
                        EnqueueTarget(unit);
                    }
                    else if(distance <= 4)//4칸 이내라면
                    {
                        softVisual = true;
                        EnqueueTarget(unit);
                    }
                }
                if(hardVisual)//2칸 이내에 적이 있음
                {
                    visualLevel = 2;
                }
                else if(softVisual)//4칸 이내에 적이 있음
                {
                    visualLevel = 1;
                }
                else//4칸 이내에 적이 없음
                {
                    visualLevel = 0;
                }              
            }
            Board.UpdateUnit(this);
        }
        public static void AllEnemyVisualSet()
        {
            foreach (var item in hostileUnitDatas)
            {
                item.EnemyVisualSet();                                
            }
        }

        protected Position[] canMoveArray;//유닛의 이동 가능 범위
        protected Position[] canAttackArray;//유닛의 공격 가능 범위
        //protected Position[] minVisualArray;//최소 시야 범위 - 렉이 너무 심해서 제거
        //protected Position[] maxVisualArray;//최대 시야 범위 - 렉이 너무 심해서 제거
        public bool ReserveMove(Position targetPosition, Unit unit)//이동 예약 기능. 플레이어의 이동 처리에 사용
        {
            while (CanAction())
            {
                float minDistance = 10000;//유닛과의 거리가 최소가 되는 지점으로 이동할 것 (최대한 붙을 것)
                Position movePos = Position.zero();
                float currentDictance = (float)Position.DistanceDouble(this.position, targetPosition);
                foreach (Position position in GetCanMoveArray())//유닛의 이동 가능 범위를 탐색
                {
                    float distance = (float)Position.DistanceDouble(position, targetPosition);
                    if (Board.GetUnitData(position) == null && distance < minDistance && distance < currentDictance)
                    //위치에 다른 유닛이 없을 경우 + 유닛과의 거리가 기존 탐색 위치보다 더 가까울 경우 + 현재 위치하고 있는 곳보다 타겟과의 거리가 더 가까울 경우
                    {
                        minDistance = distance;
                        movePos = position;
                    }
                }
                if (minDistance != 10000)//최소거리에 변화가 있을 경우 (이동 가능한 칸이 하나라도 있을 경우)
                {
                    MoveUnit(movePos, null);
                    if (targetPosition == position)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        public List<Position> ReserveMoveSimulation(Position targetPosition)//이동 예약 시뮬레이션 기능. 예상 이동 경로를 반환.
        {
            float virtualOrg = organization;
            Position temp = position;//유닛의 원래 위치
            List<Position> routePosition = new List<Position>();
            while (virtualOrg >= maxOrganization * 0.2f)
            {
                float minDistance = 10000;//유닛과의 거리가 최소가 되는 지점으로 이동할 것 (최대한 붙을 것)
                Position movePos = Position.zero();
                float currentDictance = (float)Position.DistanceDouble(this.position, targetPosition);
                foreach (Position position in GetCanMoveArray())//유닛의 이동 가능 범위를 탐색. 이동 가능 범위의 칸 수만큼 탐색함.
                {
                    float distance = (float)Position.DistanceDouble(position, targetPosition);
                    if (Board.GetUnitData(position) == null && distance < minDistance && distance < currentDictance)
                    //위치에 다른 유닛이 없을 경우 + 유닛과의 거리가 기존 탐색 위치보다 더 가까울 경우 + 현재 위치하고 있는 곳보다 타겟과의 거리가 더 가까울 경우
                    {
                        minDistance = distance;
                        movePos = position;
                    }
                }
                if (minDistance != 10000)//최소거리에 변화가 있을 경우 (이동 가능한 칸이 하나라도 있을 경우)
                {
                    position = movePos;
                    routePosition.Add(position);
                    virtualOrg -= maxOrganization * 0.2f;
                    if (targetPosition == position)
                    {
                        position = temp;
                        return routePosition;
                    }
                }
                else
                {
                    position = temp;
                    return routePosition;
                }
            }
            position = temp;
            return routePosition;
        }
        public bool MoveUnit(Position newPosition, Unit unit)//유닛 이동 명령. 이동 가능할 경우 이동하고 true, 이동 불가능할 경우 false 반환.
        {
            bool isCanMove = false;
            newPosition = Position.Clamped(newPosition);

            if (IsCanMove(newPosition) && unit == null && CanAction())
            {
                organization -= maxOrganization * 0.2f;
                isCanMove = true;
                ConsoleScreen.AddData($"유닛 이동 {Position.PrintPosition(position)} -> {Position.PrintPosition(newPosition)}", ConsoleColor.Yellow);
                position = newPosition;
                Board.UpdateUnit(this);                

                if(isFriendly)
                {
                    AllEnemyVisualSet();
                }
            }
            else
            {
                if (unitAI == null)
                {
                    string error = "ERROR";
                    if (IsCanMove(newPosition) == false)
                    {
                        error = "유효하지 않은 위치";
                    }
                    else if (unit != null)
                    {
                        error = "해당 위치에 다른 유닛이 있음";
                    }
                    else if (organization < maxOrganization * 0.2f)
                    {
                        error = "조직력 부족";
                    }

                    ConsoleScreen.AddData($"ERROR = {error}", ConsoleColor.Red);
                }
            }
            return isCanMove;
        }
        public bool AttackUnit(Position newPosition, Unit unit)//유닛 공격 명령. 공격 가능할 경우 이동하고 true, 이동 불가능할 경우 false 반환.
        {
            bool isCanAttack = false;
            newPosition = Position.Clamped(newPosition);

            if (IsCanAttack(newPosition) && unit != null && unit.isFriendly != this.isFriendly && CanAction() && unit.isDestroy == false)//유닛이 존재하고, 그 유닛이 공격 가능할 경우
            {
                isCanAttack = true;
                ConsoleScreen.AddData($"유닛 공격 {Position.PrintPosition(position)} -> {Position.PrintPosition(newPosition)}", ConsoleColor.Yellow);
                Attack(unit);//공격을 시행하고 true 반환                                
            }
            else
            {
                if (unitAI == null)
                {
                    string error = "ERROR";
                    if (IsCanAttack(newPosition) == false || unit == null)
                    {
                        error = "유효하지 않은 위치";
                    }
                    else if (unit.isFriendly == this.isFriendly)
                    {
                        error = "아군을 공격할 수 없음";
                    }
                    else if (organization < maxOrganization * 0.2f)
                    {
                        error = "조직력 부족";
                    }

                    ConsoleScreen.AddData($"ERROR = {error}", ConsoleColor.Red);
                }
            }
            Board.UpdateUnit(this);
            return isCanAttack;
        }

        protected virtual void Attack(Unit target)
        {
            float hpTemp = target.hp;
            while (true)
            {
                float dmg = (softAttack * (1 - target.hardness)) + (hardAttack * target.hardness);//공격자의 대인/대물 공격력 비율과 타겟의 장갑 비율에 따른 기초 공격력 산출                accumulateDmg
                if (piercing < target.armor)
                {
                    dmg *= piercing / (target.armor * 2);//타겟의 장갑 수치가 공격자의 관통을 넘길 경우, 피해량을 절반 이상 경감.
                }
                organization -= maxOrganization * 0.2f;
                if (target.Hit(dmg, this) == 0 || !CanAction())
                {
                    ConsoleScreen.AddData($"{(hpTemp - target.hp):F0}의 피해");                    

                    if (target.isDestroy)
                    {
                        if (target.isFriendly)
                        {
                            ConsoleScreen.AddData($"아군 유닛 파괴 {Position.PrintPosition(position)}", ConsoleColor.Red);
                        }
                        else
                        {
                            ConsoleScreen.AddData($"적 유닛 파괴 {Position.PrintPosition(position)}", ConsoleColor.Cyan);
                        }
                    }
                    return;
                }
            }
        }
        Queue<Unit> targetQueue;
        public void EnqueueTarget(Unit target)
        {
            if(targetQueue.Count == 0 || targetQueue.Contains(target) == false)
            {
                targetQueue.Enqueue(target); 
            }
        }
        public void DequeueTarget()
        {
            if (targetQueue.Count > 0)
            {
                targetQueue.Dequeue();
            }
        }

        public float Hit(float dmg, Unit attacker)
        {
            organization -= dmg;//조직력 감소
            hp -= dmg;//체력 감소            

            if (organization <= 0)//감소된 조직력이 0 이하일 경우
            {
                hp += organization * 2;//음수값의 조직력의 2배만큼 체력 추가로 감소
                organization = 0;
                if (hp <= 0)//체력이 0 이하가 될 경우 파괴
                {
                    hp = 0;
                    Destroy();
                    return 0;
                }
                if (TryRout((Position.Normalized(position - attacker.position)) + position) == false)
                {
                    Destroy();//패주에 실패할 경우 파괴
                }
            }
            if (hp <= 0)//체력이 0 이하가 될 경우 파괴
            {
                hp = 0;
                Destroy();
                return 0;
            }
            return organization;
        }
        void Destroy()
        {
            isDestroy = true;
            Board.UpdateUnit(this);
            if (isFriendly)
            {
                friendlyUnitDatas.Remove(this);
            }
            else
            {
                hostileUnitDatas.Remove(this);
            }
            if (GameWinCheck())
            {
                Board.GameEnd(true);
            }
            if(GameDefeatCheck())
            {
                Board.GameEnd(false);
            }
        }
        bool TryRout(Position routPosition)
        {
            if (!PositionCheck(routPosition))
            {
                Position[] newPos = new Position[4];
                newPos[0] = position + new Position(1, 0);
                newPos[1] = position + new Position(-1, 0);
                newPos[2] = position + new Position(0, 1);
                newPos[3] = position + new Position(0, -1);
                for (int i = 0; i < newPos.Length; i++)
                {
                    if (PositionCheck(newPos[i]))
                    {
                        routPosition = newPos[i];
                        break;
                    }
                }
            }
            if (PositionCheck(routPosition))
            {
                position = routPosition;
                Board.UpdateUnit(this);
                if (isFriendly)
                {
                    ConsoleScreen.AddData($"아군 유닛 후퇴 -> {Position.PrintPosition(routPosition)}", ConsoleColor.Yellow);
                    AllEnemyVisualSet();
                }
                else
                {
                    ConsoleScreen.AddData($"적 유닛 후퇴 -> {Position.PrintPosition(routPosition)}", ConsoleColor.Yellow);
                    EnemyVisualSet();
                }                                
                return true;
            }
            else
            {
                return false;
            }
        }
        bool PositionCheck(Position position)
        {
            if (position.y > 15 || position.y < 0 || position.x > 15 || position.x < 0)
            {
                return false;
            }
            else if (Board.GetUnitData(position) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public virtual void SetCanMoveArray() { }//유닛의 이동 가능 범위를 설정하는 가상 함수
        public virtual void SetCanAttackArray() { }//유닛의 공격 가능 범위를 설정하는 가상 함수
        bool IsCanMove(Position position)//입력받은 위치가 이동 가능 범위 내인지 확인하는 함수
        {
            bool isCanMove = false;
            SetCanMoveArray();

            if (Board.GetUnitData(position) != null) return false;

            foreach (Position item in canMoveArray)
            {
                if (position == item)
                {
                    isCanMove = true;
                    break;
                }
            }
            return isCanMove;
        }
        bool IsCanAttack(Position position)//입력받은 위치가 공격 가능 범위 내인지 확인하는 함수
        {
            bool isCanAttack = false;
            SetCanAttackArray();

            if (Board.GetUnitData(position) == null) return false;

            foreach (Position item in canAttackArray)
            {
                if (position == item)
                {
                    isCanAttack = true;
                    break;
                }
            }
            return isCanAttack;
        }
        public Position[] GetCanMoveArray() { SetCanMoveArray(); return canMoveArray; }//화면에 표시하기 위해 이동 가능 범위 전체를 리턴하는 함수
        public Position[] GetCanAttackArray() { SetCanAttackArray(); return canAttackArray; }//화면에 표시하기 위해 공격 가능 범위 전체를 리턴하는 함수

        public Position GetPosition()//유닛의 현재 위치를 리턴하는 함수
        {
            return position;
        }
    }
}

