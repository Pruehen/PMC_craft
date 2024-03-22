using Newtonsoft.Json;
using Units;

namespace PMC_craft
{
    enum EquipType
    {
        INFANTRY_WEAPON,
        ANTI_TANK_WEAPON,
        APC,
        IFV,
        TOWED_HOWITZER,
        SPH,
        TANK
    }
    class Equipment
    {
        public int cost { get; }
        public float softAttack { get; }
        public float hardAttack { get; }
        public float armor { get; }
        public float piercing { get; }
        public EquipType type { get; }

        public int count { get; private set; }
        public int useCount { get; private set; }
        public int AvailableCount() { return count - useCount; }
        public void PlusUseCount(int value)
        {
            useCount += value;
        }
        public void TryBuyEquipment()
        {
            if(UserData.credit >= cost)
            {
                count++;
                UserData.UseCredit(cost);
            }
        }
        public float hp { get; }
        public float organization { get; }

        public Equipment(int cost, EquipType type, float softAttack, float hardAttack, float armor, float piercing, int count, int useCount, float hp, float organization)
        {
            this.cost = cost;
            this.type = type;
            this.softAttack = softAttack;
            this.hardAttack = hardAttack;
            this.armor = armor;
            this.piercing = piercing;
            this.count = count;
            this.useCount = useCount;
            this.hp = hp;
            this.organization = organization;
        }

        public string[] GetEquipData()
        {
            string[] datas = new string[7];
            string typeText = "알 수 없는 무기";
            switch(type)
            {
                case EquipType.INFANTRY_WEAPON:
                    typeText = "보병 화기";
                    break;
                case EquipType.ANTI_TANK_WEAPON:
                    typeText = "대전차 화기";
                    break;
                case EquipType.APC:
                    typeText = "병력수송장갑차";
                    break;
                case EquipType.IFV:
                    typeText = "보병전투차";
                    break;
                case EquipType.TOWED_HOWITZER:
                    typeText = "견인곡사포";
                    break;
                case EquipType.SPH:
                    typeText = "자주포";
                    break;
                case EquipType.TANK:
                    typeText = "전차";
                    break;
            }
            datas[0] = $"분류 :        {typeText}        ";
            datas[1] = $"가격 :        {cost:F0}$        ";
            datas[2] = $"대인 공격력 : {softAttack:F1}        ";
            datas[3] = $"대물 공격력 : {hardAttack:F1}        ";
            datas[4] = $"장갑 수치 :   {armor:F1}        ";
            datas[5] = $"관통력 :      {piercing:F1}        ";            
            datas[6] = $"현재 가용량 : {AvailableCount()}        ";

            return datas;
        }
    }

    class CustomizedUnit
    {
        [JsonIgnore]
        float maxHp = 0;//체력 - 실제 내구력. 0이 되면 유닛이 파괴된다.        
        [JsonIgnore]
        float maxOrganization = 0;//조직력 - 얼마나 잘 조직되어있는지에 대한 척도. 0이 되면 패주하여 주변 칸으로 이동한다.        
        [JsonIgnore]
        float softAttack = 0;//대인 공격력 - 장갑 비율이 낮은 적에게 입히는 피해량
        [JsonIgnore]
        float hardAttack = 0;//대물 공격력 - 장갑 비율이 높은 적에게 입히는 피해량
        [JsonIgnore]
        float hardness = 0;//장갑 비율 - 현재 유닛이 얼마나 장갑화되어있는지에 대한 척도
        [JsonIgnore]
        float armor = 0;//장갑 수치. 높으면 관통이 낮은 적에게 적은 피해를 받는다.
        [JsonIgnore]
        float piercing = 0;//관통 수치. 관통 수치보다 장갑 수치가 높을 경우 정상적으로 피해를 가할 수 있다.
        [JsonIgnore]
        string codeName = "";
        
        public string realName { get; }

        [JsonProperty]
        string[] slot = new string[9];//장비 장착 슬롯
        public string[] GetSlotItem() { return slot; }
        public bool IsSlotEmpty()
        {
            for (int i = 0; i < 9; i++)
            {
                if(slot[i] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public CustomizedUnit(string realName, string[] slot)
        {
            this.realName = realName;
            this.slot = new string[9];
            for (int i = 0; i < 9; i++)
            {
                if(i < slot.Length)
                {
                    this.slot[i] = slot[i];
                }
                else
                {
                    this.slot[i] = null;
                }
            }

            for (int i = 0; i < this.slot.Length; i++)
            {                
                if (this.slot[i] != null && !UserData.Equipments.ContainsKey(this.slot[i]))//string이 null이 아니면서 유효한 키가 아닐 경우
                {
                    this.slot[i] = "전간기 보병 화기";//기본 키로 string 변경
                }
            }
        }

        public void AddItem(string name, int index)
        {
            if(slot[index] != null)
            {
                RemoveItem(index);
            }
            slot[index] = name;
            UserData.Equipments[slot[index]].PlusUseCount(1);
        }
        public void RemoveItem(int index)
        {
            if (slot[index] != null)
            {
                UserData.Equipments[slot[index]].PlusUseCount(-1);
                slot[index] = null;
            }
        }
        public string[] GetUnitDatas_DataOnly()
        {
            SpecCalculation();
            string[] datas = new string[8];
            string type;
            switch (codeName)
            {
                case "IFT":
                    type = "보병";
                    break;
                case "MIF":
                    type = "기계화 보병";
                    break;
                case "ATL":
                    type = "견인포";
                    break;
                case "SPA":
                    type = "자주포";
                    break;
                case "TNK":
                    type = "기갑";
                    break;
                default:
                    type = "보병";
                    break;
            }
            datas[0] = $"분류 :        {type}";
            datas[1] = $"내구력 :      {maxHp:F1}";
            datas[2] = $"조직력 :      {maxOrganization:F1}";
            datas[3] = $"대인 공격력 : {softAttack:F1}";
            datas[4] = $"대물 공격력 : {hardAttack:F1}";
            datas[5] = $"장갑 비율 :   {(hardness * 100):F1}%";
            datas[6] = $"장갑 수치 :   {armor:F1}";
            datas[7] = $"관통력 :      {piercing:F1}";

            return datas;
        }

        void SpecCalculation()
        {
            maxHp = 0;
            maxOrganization = 0;
            softAttack = 0;
            hardAttack = 0;
            hardness = 0;
            armor = 0;
            piercing = 0;
            codeName = "";

            if (slot != null)
            {
                Equipment equipment;
                float equipCount = 0;
                
                int mifValue = 0;
                int atlValue = 0;
                int spaValue = 0;
                int tnkValue = 0;

                for (int i = 0; i < slot.Length; i++)
                {
                    if (slot[i] != null)
                    {
                        equipment = UserData.Equipments[slot[i]];

                        maxHp += equipment.hp;
                        maxOrganization += equipment.organization;
                        softAttack += equipment.softAttack;
                        hardAttack += equipment.hardAttack;
                        switch (equipment.type)
                        {
                            case EquipType.INFANTRY_WEAPON:                                
                                break;
                            case EquipType.ANTI_TANK_WEAPON:                                
                                break;
                            case EquipType.APC:                                
                                mifValue++;
                                hardness += 0.5f;
                                break;
                            case EquipType.IFV:                                
                                mifValue++;
                                hardness += 0.9f;
                                break;
                            case EquipType.TOWED_HOWITZER:                                
                                atlValue++;
                                break;
                            case EquipType.SPH:                                
                                mifValue++;
                                atlValue++;
                                spaValue++;
                                hardness += 1;
                                break;
                            case EquipType.TANK:                                
                                mifValue++;
                                tnkValue++;
                                hardness += 1;
                                break;
                        }
                        armor += equipment.armor;
                        piercing += equipment.piercing;

                        equipCount++;
                    }
                }
                if (equipCount > 0)
                {
                    float countGain = 1 / (equipCount * 0.1f + 0.9f);                    
                    maxOrganization *= countGain;
                    softAttack *= countGain;
                    hardAttack *= countGain;
                    hardness *= 1 / equipCount;
                    armor *= 1 / equipCount;
                    piercing *= 1 / equipCount;

                    if (mifValue == equipCount && tnkValue >= mifValue / 2)
                        codeName = "TNK";
                    else if (spaValue == equipCount)
                        codeName = "SPA";
                    else if (atlValue == equipCount)
                        codeName = "ATL";
                    else if (mifValue == equipCount)
                        codeName = "MIF";
                    else
                        codeName = "IFT";
                }
            }
        }

        public Unit CreateUnit(Position position, bool isFriendly, float specGain = 1)
        {
            SpecCalculation();
            switch (codeName)
            {
                case "IFT":
                    return new Infantry(position, isFriendly, codeName, realName, maxHp * specGain, maxOrganization * specGain, softAttack * specGain, hardAttack * specGain,
                        hardness, armor, piercing);                    
                case "MIF":
                    return new MechanizedInfantry(position, isFriendly, codeName, realName, maxHp * specGain, maxOrganization * specGain, softAttack * specGain, hardAttack * specGain,
                        hardness, armor, piercing);                    
                case "ATL":
                    return new Artillery(position, isFriendly, codeName, realName, maxHp * specGain, maxOrganization * specGain, softAttack * specGain, hardAttack * specGain,
                        hardness, armor, piercing);                    
                case "SPA":
                    return new SelfPropelledArtillery(position, isFriendly, codeName, realName, maxHp * specGain, maxOrganization * specGain, softAttack * specGain, hardAttack * specGain,
                        hardness, armor, piercing);                    
                case "TNK":
                    return new Panzer(position, isFriendly, codeName, realName, maxHp * specGain, maxOrganization * specGain, softAttack * specGain, hardAttack * specGain,
                        hardness, armor, piercing);                    
                default:
                    return new Infantry(position, isFriendly, "IFT", realName, maxHp * specGain, maxOrganization * specGain, softAttack * specGain, hardAttack * specGain,
                        hardness, armor, piercing);
            }
        }
    }
}
