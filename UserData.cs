using Units;
using Newtonsoft.Json;

namespace PMC_craft
{
    static class UserData
    {
        public static CustomizedUnit[] friendlyUnitDatas = new CustomizedUnit[10];
        public static List<CustomizedUnit> HostileUnitDatas = new List<CustomizedUnit>();
        public static Dictionary<string, Equipment> Equipments = new Dictionary<string, Equipment>();
        public static int credit { get; private set; }//보유자금
        public static void UseCredit(int value)
        {
            credit -= value;
        }
        public static int level { get; private set; }//플레이어 레벨(내부적)
        public static void LevelUp()
        {
            level++;
        }
        public static void LevelDown()
        {
            level--;
            if (level <= 0) level = 1;
        }

        public static void DeletefriendlyUnitData(int index)
        {
            if (index < friendlyUnitDatas.Length)
            {
                friendlyUnitDatas[index] = null;
            }
        }
        public static Dictionary<string, Equipment> AvailableEquips()
        {
            Dictionary<string, Equipment> availableEquips = new Dictionary<string, Equipment>();
            foreach (var item in Equipments)
            {
                if(item.Value.AvailableCount() > 0)
                {
                    availableEquips.Add(item.Key, item.Value);
                }
            }

            if (availableEquips.Count > 0) { return availableEquips; }
            else return null;
        }
        static string exePath = AppDomain.CurrentDomain.BaseDirectory;

        static string dataFolderPath = exePath + @"Data";
        static string saveFolderPath = exePath + @"Data\SaveData";
        static string saveFileName_Units = exePath + @"Data\SaveData\FriendlyUnitData.txt";
        static string saveFileName_Equips = exePath + @"Data\SaveData\EquipsData.txt";
        static string saveFileName_HostileUnit = exePath + @"Data\HostileUnitData.txt";
        static string saveFileName_UserData = exePath + @"Data\SaveData\UserData.txt";

        public static void SaveData()//데이터를 json으로 저장하는 함수
        {
            if (!Directory.Exists(dataFolderPath))
            {
                Directory.CreateDirectory(dataFolderPath);
            }
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            var data = JsonConvert.SerializeObject(friendlyUnitDatas, Formatting.Indented);
            File.WriteAllText(saveFileName_Units, data);

            data = JsonConvert.SerializeObject(HostileUnitDatas, Formatting.Indented);
            File.WriteAllText(saveFileName_HostileUnit, data);

            data = JsonConvert.SerializeObject(Equipments, Formatting.Indented);
            File.WriteAllText(saveFileName_Equips, data);

            int[] dataArray = [credit, level];
            data = JsonConvert.SerializeObject(dataArray, Formatting.Indented);
            File.WriteAllText(saveFileName_UserData, data);
        }
        public static void LoadData()//json을 데이터로 변환하는 함수
        {
            if (File.Exists(saveFileName_Equips))
            {
                var fileData = File.ReadAllText(saveFileName_Equips);
                Equipments = JsonConvert.DeserializeObject<Dictionary<string, Equipment>>(fileData);
            }
            else
            {
                Equipments.Add("전간기 보병 화기", new Equipment(100, EquipType.INFANTRY_WEAPON, 10, 0, 0, 1, 30, 25, 150, 100));
                Equipments.Add("대전기 보병 화기", new Equipment(250, EquipType.INFANTRY_WEAPON, 15, 0, 0, 1, 0, 0, 160, 100));
                Equipments.Add("냉전기 보병 화기", new Equipment(500, EquipType.INFANTRY_WEAPON, 20, 0, 0, 1, 0, 0, 170, 100));
                Equipments.Add("현대 보병 화기", new Equipment(1200, EquipType.INFANTRY_WEAPON, 25, 0, 0, 2, 0, 0, 185, 100));
                Equipments.Add("미래 보병 화기", new Equipment(3600, EquipType.INFANTRY_WEAPON, 30, 0, 0, 2, 0, 0, 200, 100));

                Equipments.Add("소구경 대전차포", new Equipment(250, EquipType.ANTI_TANK_WEAPON, 1, 15, 1, 10, 0, 0, 50, 30));
                Equipments.Add("대구경 대전차포", new Equipment(500, EquipType.ANTI_TANK_WEAPON, 3, 25, 1, 20, 0, 0, 55, 30));
                Equipments.Add("대전차 로켓 발사기", new Equipment(1500, EquipType.ANTI_TANK_WEAPON, 5, 40, 0, 30, 0, 0, 63, 30));
                Equipments.Add("대전차 미사일", new Equipment(3500, EquipType.ANTI_TANK_WEAPON, 5, 55, 0, 60, 0, 0, 75, 30));
                Equipments.Add("미래 대전차 미사일", new Equipment(8000, EquipType.ANTI_TANK_WEAPON, 5, 75, 0, 120, 0, 0, 90, 30));

                Equipments.Add("두돈반 트럭", new Equipment(300, EquipType.APC, 10, 0, 1, 0, 0, 0, 200, 100));
                Equipments.Add("반궤도 장갑차", new Equipment(1000, EquipType.APC, 18, 0, 1, 1, 0, 0, 210, 100));
                Equipments.Add("냉전기 APC", new Equipment(2400, EquipType.APC, 25, 5, 2, 2, 0, 0, 225, 100));
                Equipments.Add("현대 APC", new Equipment(6000, EquipType.APC, 35, 8, 3, 6, 0, 0, 245, 100));
                Equipments.Add("미래 APC", new Equipment(20000, EquipType.APC, 55, 15, 5, 12, 0, 0, 270, 100));

                Equipments.Add("중무장 APC", new Equipment(3000, EquipType.IFV, 25, 10, 5, 5, 0, 0, 180, 100));
                Equipments.Add("냉전기 IFV", new Equipment(9000, EquipType.IFV, 35, 15, 7, 7, 0, 0, 190, 100));
                Equipments.Add("현대 IFV", new Equipment(20000, EquipType.IFV, 50, 25, 10, 18, 0, 0, 200, 100));
                Equipments.Add("미래 IFV", new Equipment(40000, EquipType.IFV, 65, 40, 15, 25, 0, 0, 210, 100));

                Equipments.Add("전간기 견인곡사포", new Equipment(250, EquipType.TOWED_HOWITZER, 25, 5, 1, 5, 0, 0, 50, 10));
                Equipments.Add("대전기 견인곡사포", new Equipment(500, EquipType.TOWED_HOWITZER, 40, 7, 1, 5, 0, 0, 55, 10));
                Equipments.Add("냉전기 견인곡사포", new Equipment(1000, EquipType.TOWED_HOWITZER, 55, 10, 1, 6, 0, 0, 63, 10));
                Equipments.Add("현대 견인곡사포", new Equipment(2000, EquipType.TOWED_HOWITZER, 75, 12, 1, 6, 0, 0, 75, 10));
                Equipments.Add("미래 견인곡사포", new Equipment(4000, EquipType.TOWED_HOWITZER, 110, 15, 1, 6, 0, 0, 90, 10));

                Equipments.Add("전간기 자주포", new Equipment(700, EquipType.SPH, 28, 5, 2, 5, 0, 0, 100, 20));
                Equipments.Add("대전기 자주포", new Equipment(2000, EquipType.SPH, 45, 7, 2, 5, 0, 0, 110, 20));
                Equipments.Add("냉전기 자주포", new Equipment(4500, EquipType.SPH, 65, 10, 4, 6, 0, 0, 130, 20));
                Equipments.Add("현대 자주포", new Equipment(25000, EquipType.SPH, 105, 25, 10, 15, 0, 0, 150, 20));
                Equipments.Add("미래 자주포", new Equipment(60000, EquipType.SPH, 155, 50, 15, 25, 0, 0, 200, 20));

                Equipments.Add("대전기 중형 전차", new Equipment(700, EquipType.TANK, 20, 20, 20, 20, 0, 0, 300, 20));
                Equipments.Add("1세대 주력 전차", new Equipment(2000, EquipType.TANK, 25, 25, 30, 30, 0, 0, 330, 22));
                Equipments.Add("2세대 주력 전차", new Equipment(4500, EquipType.TANK, 35, 35, 40, 40, 0, 0, 370, 24));
                Equipments.Add("3세대 주력 전차", new Equipment(25000, EquipType.TANK, 45, 45, 60, 60, 0, 0, 450, 30));
                Equipments.Add("4세대 주력 전차", new Equipment(60000, EquipType.TANK, 55, 55, 80, 80, 0, 0, 600, 50));
            }

            if (File.Exists(saveFileName_Units))
            {
                var fileData = File.ReadAllText(saveFileName_Units);
                friendlyUnitDatas = JsonConvert.DeserializeObject<CustomizedUnit[]>(fileData);
            }
            else
            {
                string[] initWeapon = { "전간기 보병 화기", "전간기 보병 화기", "전간기 보병 화기", "전간기 보병 화기", "전간기 보병 화기" };
                friendlyUnitDatas[0]=(new CustomizedUnit("초기 보병 유닛", initWeapon));
                friendlyUnitDatas[1]=(new CustomizedUnit("초기 보병 유닛", initWeapon));
                friendlyUnitDatas[2]=(new CustomizedUnit("초기 보병 유닛", initWeapon));
                friendlyUnitDatas[3]=(new CustomizedUnit("초기 보병 유닛", initWeapon));
                friendlyUnitDatas[4]=(new CustomizedUnit("초기 보병 유닛", initWeapon));
                friendlyUnitDatas[5] = null;
                friendlyUnitDatas[6] = null;
                friendlyUnitDatas[7] = null;
                friendlyUnitDatas[8] = null;
                friendlyUnitDatas[9] = null;
            }
            if (File.Exists(saveFileName_HostileUnit))
            {
                var fileData = File.ReadAllText(saveFileName_HostileUnit);
                HostileUnitDatas = JsonConvert.DeserializeObject<List<CustomizedUnit>>(fileData);
            }
            else
            {
                HostileUnitDatas.Add(new CustomizedUnit("저항자 부대", ["전간기 보병 화기", "전간기 보병 화기", "전간기 보병 화기"]));
                HostileUnitDatas.Add(new CustomizedUnit("화이트펜서 중대", ["냉전기 보병 화기", "냉전기 보병 화기", "냉전기 보병 화기", "중무장 APC"]));
                HostileUnitDatas.Add(new CustomizedUnit("아이언바리케이드 중대", ["현대 보병 화기", "현대 보병 화기", "현대 보병 화기", "대전차 로켓 발사기", "현대 견인곡사포", "1세대 주력 전차"]));
                HostileUnitDatas.Add(new CustomizedUnit("자유의 투사", ["미래 보병 화기", "미래 보병 화기", "미래 보병 화기", "4세대 주력 전차", "4세대 주력 전차", "4세대 주력 전차", "미래 자주포", "미래 자주포", "미래 자주포"]));
            }            

            if(File.Exists(saveFileName_UserData))
            {
                var fileData = File.ReadAllText(saveFileName_UserData);
                int[] dataArray = JsonConvert.DeserializeObject<int[]>(fileData);
                credit = dataArray[0];
                level = dataArray[1];
            }
            else
            {
                credit = 0;
                level = 1;
            }            
        }
    }
}
