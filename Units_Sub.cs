namespace Units
{
    class Infantry : Unit
    {
        public Infantry(Position position, bool isFriendly, string codeName = "IFT", string realName = "보병", float hp = 2000, float organization = 400,
            float softAttack = 50, float hardAttack = 20, float hardness = 0, float armor = 0, float piercing = 20) : base(position, isFriendly, codeName, realName, hp, organization,
                 softAttack, hardAttack, hardness, armor, piercing)
        {
            canMoveArray = new Position[8];
            canAttackArray = new Position[8];            
        }
        public Infantry(Unit data) : base(data)
        {
            canMoveArray = new Position[8];
            canAttackArray = new Position[8];
            codeName = "IFT";
        }

        public override void SetCanMoveArray()
        {
            canMoveArray[0].SetPosition(position.x + 1, position.y + 1);
            canMoveArray[1].SetPosition(position.x, position.y + 1);
            canMoveArray[2].SetPosition(position.x - 1, position.y + 1);
            canMoveArray[3].SetPosition(position.x + 1, position.y);
            canMoveArray[4].SetPosition(position.x - 1, position.y);
            canMoveArray[5].SetPosition(position.x + 1, position.y - 1);
            canMoveArray[6].SetPosition(position.x, position.y - 1);
            canMoveArray[7].SetPosition(position.x - 1, position.y - 1);
        }
        public override void SetCanAttackArray()
        {
            canAttackArray[0].SetPosition(position.x + 1, position.y + 1);
            canAttackArray[1].SetPosition(position.x, position.y + 1);
            canAttackArray[2].SetPosition(position.x - 1, position.y + 1);
            canAttackArray[3].SetPosition(position.x + 1, position.y);
            canAttackArray[4].SetPosition(position.x - 1, position.y);
            canAttackArray[5].SetPosition(position.x + 1, position.y - 1);
            canAttackArray[6].SetPosition(position.x, position.y - 1);
            canAttackArray[7].SetPosition(position.x - 1, position.y - 1);
        }
    }

    class MechanizedInfantry : Unit
    {
        public MechanizedInfantry(Position position, bool isFriendly, string codeName = "MIF", string realName = "기계화 보병", float hp = 2500, float organization = 400,
            float softAttack = 70, float hardAttack = 40, float hardness = 0.5f, float armor = 10, float piercing = 40) : base(position, isFriendly, codeName, realName, hp, organization,
                 softAttack, hardAttack, hardness, armor, piercing)
        {
            canMoveArray = new Position[12];
            canAttackArray = new Position[8];
        }
        public MechanizedInfantry(Unit data) : base(data)
        {
            canMoveArray = new Position[12];
            canAttackArray = new Position[8];
            codeName = "MIF";
        }

        public override void SetCanMoveArray()
        {
            canMoveArray[0].SetPosition(position.x + 1, position.y + 1);
            canMoveArray[1].SetPosition(position.x, position.y + 1);
            canMoveArray[2].SetPosition(position.x - 1, position.y + 1);
            canMoveArray[3].SetPosition(position.x + 1, position.y);
            canMoveArray[4].SetPosition(position.x - 1, position.y);
            canMoveArray[5].SetPosition(position.x + 1, position.y - 1);
            canMoveArray[6].SetPosition(position.x, position.y - 1);
            canMoveArray[7].SetPosition(position.x - 1, position.y - 1);

            canMoveArray[8].SetPosition(position.x + 2, position.y);
            canMoveArray[9].SetPosition(position.x - 2, position.y);
            canMoveArray[10].SetPosition(position.x, position.y + 2);
            canMoveArray[11].SetPosition(position.x, position.y - 2);
        }
        public override void SetCanAttackArray()
        {
            canAttackArray[0].SetPosition(position.x + 1, position.y + 1);
            canAttackArray[1].SetPosition(position.x, position.y + 1);
            canAttackArray[2].SetPosition(position.x - 1, position.y + 1);
            canAttackArray[3].SetPosition(position.x + 1, position.y);
            canAttackArray[4].SetPosition(position.x - 1, position.y);
            canAttackArray[5].SetPosition(position.x + 1, position.y - 1);
            canAttackArray[6].SetPosition(position.x, position.y - 1);
            canAttackArray[7].SetPosition(position.x - 1, position.y - 1);
        }
    }

    class Panzer : Unit
    {
        public Panzer(Position position, bool isFriendly, string codeName = "TNK", string realName = "전차", float hp = 3000, float organization = 250,
            float softAttack = 150, float hardAttack = 150, float hardness = 1, float armor = 90, float piercing = 100) : base(position, isFriendly, codeName, realName, hp, organization,
                 softAttack, hardAttack, hardness, armor, piercing)
        {
            canMoveArray = new Position[8];
            canAttackArray = new Position[8];
        }
        public Panzer(Unit data) : base(data)
        {
            canMoveArray = new Position[8];
            canAttackArray = new Position[8];
            codeName = "TNK";
        }

        public override void SetCanMoveArray()
        {
            canMoveArray[0].SetPosition(position.x, position.y + 1);
            canMoveArray[1].SetPosition(position.x + 1, position.y);
            canMoveArray[2].SetPosition(position.x - 1, position.y);
            canMoveArray[3].SetPosition(position.x, position.y - 1);

            canMoveArray[4].SetPosition(position.x + 2, position.y);
            canMoveArray[5].SetPosition(position.x - 2, position.y);
            canMoveArray[6].SetPosition(position.x, position.y + 2);
            canMoveArray[7].SetPosition(position.x, position.y - 2);
        }
        public override void SetCanAttackArray()
        {
            canAttackArray[0].SetPosition(position.x, position.y + 1);
            canAttackArray[1].SetPosition(position.x + 1, position.y);
            canAttackArray[2].SetPosition(position.x - 1, position.y);
            canAttackArray[3].SetPosition(position.x, position.y - 1);

            canAttackArray[4].SetPosition(position.x + 2, position.y);
            canAttackArray[5].SetPosition(position.x - 2, position.y);
            canAttackArray[6].SetPosition(position.x, position.y + 2);
            canAttackArray[7].SetPosition(position.x, position.y - 2);
        }
    }

    class Artillery : Unit
    {
        public Artillery(Position position, bool isFriendly, string codeName = "ATL", string realName = "야포단", float hp = 500, float organization = 150,
            float softAttack = 50, float hardAttack = 50, float hardness = 0, float armor = 0, float piercing = 30) : base(position, isFriendly, codeName, realName, hp, organization,
                 softAttack, hardAttack, hardness, armor, piercing)
        {
            canMoveArray = new Position[4];
            canAttackArray = new Position[32];
        }
        public Artillery(Unit data) : base(data)
        {
            canMoveArray = new Position[4];
            canAttackArray = new Position[32];
            codeName = "ATL";
        }

        public override void SetCanMoveArray()
        {
            canMoveArray[0].SetPosition(position.x, position.y + 1);
            canMoveArray[1].SetPosition(position.x + 1, position.y);
            canMoveArray[2].SetPosition(position.x - 1, position.y);
            canMoveArray[3].SetPosition(position.x, position.y - 1);
        }
        public override void SetCanAttackArray()
        {
            canAttackArray[0].SetPosition(position.x, position.y + 2);
            canAttackArray[1].SetPosition(position.x + 2, position.y);
            canAttackArray[2].SetPosition(position.x - 2, position.y);
            canAttackArray[3].SetPosition(position.x, position.y - 2);

            canAttackArray[4].SetPosition(position.x, position.y + 3);
            canAttackArray[5].SetPosition(position.x + 3, position.y);
            canAttackArray[6].SetPosition(position.x - 3, position.y);
            canAttackArray[7].SetPosition(position.x, position.y - 3);

            canAttackArray[8].SetPosition(position.x, position.y + 4);
            canAttackArray[9].SetPosition(position.x + 4, position.y);
            canAttackArray[10].SetPosition(position.x - 4, position.y);
            canAttackArray[11].SetPosition(position.x, position.y - 4);

            canAttackArray[24].SetPosition(position.x, position.y + 5);
            canAttackArray[25].SetPosition(position.x + 5, position.y);
            canAttackArray[26].SetPosition(position.x - 5, position.y);
            canAttackArray[27].SetPosition(position.x, position.y - 5);

            canAttackArray[12].SetPosition(position.x + 2, position.y + 2);
            canAttackArray[13].SetPosition(position.x + 2, position.y - 2);
            canAttackArray[14].SetPosition(position.x - 2, position.y + 2);
            canAttackArray[15].SetPosition(position.x - 2, position.y - 2);

            canAttackArray[16].SetPosition(position.x + 3, position.y + 3);
            canAttackArray[17].SetPosition(position.x + 3, position.y - 3);
            canAttackArray[18].SetPosition(position.x - 3, position.y + 3);
            canAttackArray[19].SetPosition(position.x - 3, position.y - 3);

            canAttackArray[20].SetPosition(position.x + 4, position.y + 4);
            canAttackArray[21].SetPosition(position.x + 4, position.y - 4);
            canAttackArray[22].SetPosition(position.x - 4, position.y + 4);
            canAttackArray[23].SetPosition(position.x - 4, position.y - 4);

            canAttackArray[28].SetPosition(position.x + 5, position.y + 5);
            canAttackArray[29].SetPosition(position.x + 5, position.y - 5);
            canAttackArray[30].SetPosition(position.x - 5, position.y + 5);
            canAttackArray[31].SetPosition(position.x - 5, position.y - 5);
        }
        protected override void Attack(Unit target)
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
                    organization = 0;
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
    }

    class SelfPropelledArtillery : Unit
    {
        public SelfPropelledArtillery(Position position, bool isFriendly, string codeName = "SPA", string realName = "자주포", float hp = 500, float organization = 150,
            float softAttack = 50, float hardAttack = 50, float hardness = 0, float armor = 0, float piercing = 30) : base(position, isFriendly, codeName, realName, hp, organization,
                 softAttack, hardAttack, hardness, armor, piercing)
        {
            canMoveArray = new Position[8];
            canAttackArray = new Position[32];
        }
        public SelfPropelledArtillery(Unit data) : base(data)
        {
            canMoveArray = new Position[8];
            canAttackArray = new Position[32];
            codeName = "SPA";
        }

        public override void SetCanMoveArray()
        {
            canMoveArray[0].SetPosition(position.x, position.y + 1);
            canMoveArray[1].SetPosition(position.x + 1, position.y);
            canMoveArray[2].SetPosition(position.x - 1, position.y);
            canMoveArray[3].SetPosition(position.x, position.y - 1);

            canMoveArray[4].SetPosition(position.x + 2, position.y);
            canMoveArray[5].SetPosition(position.x - 2, position.y);
            canMoveArray[6].SetPosition(position.x, position.y + 2);
            canMoveArray[7].SetPosition(position.x, position.y - 2);
        }
        public override void SetCanAttackArray()
        {
            canAttackArray[0].SetPosition(position.x, position.y + 2);
            canAttackArray[1].SetPosition(position.x + 2, position.y);
            canAttackArray[2].SetPosition(position.x - 2, position.y);
            canAttackArray[3].SetPosition(position.x, position.y - 2);

            canAttackArray[4].SetPosition(position.x, position.y + 3);
            canAttackArray[5].SetPosition(position.x + 3, position.y);
            canAttackArray[6].SetPosition(position.x - 3, position.y);
            canAttackArray[7].SetPosition(position.x, position.y - 3);

            canAttackArray[8].SetPosition(position.x, position.y + 4);
            canAttackArray[9].SetPosition(position.x + 4, position.y);
            canAttackArray[10].SetPosition(position.x - 4, position.y);
            canAttackArray[11].SetPosition(position.x, position.y - 4);

            canAttackArray[24].SetPosition(position.x, position.y + 5);
            canAttackArray[25].SetPosition(position.x + 5, position.y);
            canAttackArray[26].SetPosition(position.x - 5, position.y);
            canAttackArray[27].SetPosition(position.x, position.y - 5);

            canAttackArray[12].SetPosition(position.x + 2, position.y + 2);
            canAttackArray[13].SetPosition(position.x + 2, position.y - 2);
            canAttackArray[14].SetPosition(position.x - 2, position.y + 2);
            canAttackArray[15].SetPosition(position.x - 2, position.y - 2);

            canAttackArray[16].SetPosition(position.x + 3, position.y + 3);
            canAttackArray[17].SetPosition(position.x + 3, position.y - 3);
            canAttackArray[18].SetPosition(position.x - 3, position.y + 3);
            canAttackArray[19].SetPosition(position.x - 3, position.y - 3);

            canAttackArray[20].SetPosition(position.x + 4, position.y + 4);
            canAttackArray[21].SetPosition(position.x + 4, position.y - 4);
            canAttackArray[22].SetPosition(position.x - 4, position.y + 4);
            canAttackArray[23].SetPosition(position.x - 4, position.y - 4);

            canAttackArray[28].SetPosition(position.x + 5, position.y + 5);
            canAttackArray[29].SetPosition(position.x + 5, position.y - 5);
            canAttackArray[30].SetPosition(position.x - 5, position.y + 5);
            canAttackArray[31].SetPosition(position.x - 5, position.y - 5);
        }
        protected override void Attack(Unit target)
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
                    organization = 0;
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
    }
}
