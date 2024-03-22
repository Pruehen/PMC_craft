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
}
