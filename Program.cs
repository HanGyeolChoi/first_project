using System.Numerics;
using System.Xml.Linq;

namespace TextRPG_project
{
    internal class Program
    {
        static List<Item> itemList = new List<Item>(); // 아이템 리스트 초기화;
        public enum DungeonDiff { 쉬운 = 1, 일반, 어려운}
        static string Start()
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 선택해주세요.\n");

            string name = Console.ReadLine();

            Console.WriteLine($"\n설정하신 이름은 {name}입니다.\n");
            Console.WriteLine("1. 저장\n2. 취소\n");
            Console.WriteLine("원하시는 행동을 선택해주세요.");

            string input = Console.ReadLine();
            if (input == "1")
            {
                return name;
            }
            else if (input == "2")
            {
                return Start();
            }
            else 
            {
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(1000);
                return Start();
            }
        }

        static int SelectClass()
        {
            
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 직업을 선택해주세요.\n");
            Console.WriteLine("1. 전사\n2. 도적");
            Console.WriteLine("\n원하시는 행동을 선택해주세요.");
            string class_type = Console.ReadLine();

            if (class_type == "1") Console.WriteLine("\n고른 직업은 전사입니다.\n");
            else if (class_type == "2") Console.WriteLine("\n고른 직업은 도적 입니다.\n");
            else
            {
                Console.WriteLine("\n잘못된 입력입니다.");
                Thread.Sleep(1000);
                return SelectClass();
            }
            Console.WriteLine("1. 저장\n2. 취소\n");
            Console.WriteLine("원하시는 행동을 선택해주세요.");

            string input = Console.ReadLine();

            if(input == "1")
            {
                return int.Parse(class_type);
            }
            else if(input == "2")
            {
                return SelectClass();
            }
            else
            {
                Console.WriteLine("\n잘못된 입력입니다.");
                Thread.Sleep(1000);
                return SelectClass();
            }
        }
        static void MainMenu(Character player)
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전에 들어가기 전 활동을 할 수 있습니다.\n");

            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 휴식하기");
            Console.WriteLine("5. 던전 입장");
            Console.WriteLine("0. 종료");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    player.ShowStats();//상태창 보기
                    break;
                case "2":
                    player.ShowInventory();//인벤토리 보기
                    break;
                case "3":
                    Store(itemList, player);//상점 보기
                    break;
                case "4":
                    player.Rest();
                    break;
                case "5":
                    DungeonMenu(player);
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("\n잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    MainMenu(player);
                    break;

            }

        }

            
        class Character
        {
            private int level;
            private string name;
            private int class_type;     //전사일 경우 1, 도적일 경우 2
            public int attack;
            public int defence;
            public int health;
            public int gold;
            private int itemAttack;     //아이템으로 올라간 총 공격력
            private int itemDefence;    //아이템으로 올라간 총 방어력

            public List<Item> items;   // 인벤토리의 아이템
            private Item? equippedArmor;
            private Item? equippedWeapon;
            public Character(string _name, int class_num)
            {
                level = 1;
                name = _name;
                class_type = class_num;
                attack = 10;
                defence = 5;
                health = 100;
                gold = 1500;
                items = new List<Item>();
                itemAttack = 0;
                itemDefence = 0;
                equippedArmor = null;
                equippedWeapon = null;
            }

           
            public void Buy(Item item)
            {
                item.sold = true;
                items.Add(item);
                gold -= item.price;
            }

            public void Sell(Item item)
            {
                if (item.equip == true) Unequip(item);
                item.sold = false;
                items.Remove(item);
                gold += item.price * 85 / 100;
            }


            public void ShowStats()
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                Console.WriteLine($"Lv : {level}");
                Console.WriteLine($"이  름 : {name}");
                Console.Write($"직  업 : ");
                if (class_type == 1) Console.WriteLine("전사");
                else Console.WriteLine("도적");
                Console.Write($"공격력 : {attack} ");
                if (itemAttack > 0) Console.Write($"(+{itemAttack})");
                Console.Write($"\n방어력 : {defence}");
                if (itemDefence > 0) Console.Write($"(+{itemDefence})");
                Console.WriteLine($"\n체  력 : {health}");
                Console.WriteLine($"Gold : {gold}");

                Console.WriteLine("\n0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine();
                if(input == "0")
                {
                    MainMenu(this);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    ShowStats();
                }
            }


            public void ShowInventory()
            {
                Console.Clear();
                Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");

                Console.WriteLine("[아이템 목록]");
                for(int i=0; i<items.Count; i++)
                {
                    Console.Write("- ");
                    if (items[i].equip) Console.Write("[E] ");
                    items[i].ShowItem();
                    Console.WriteLine();
                }

                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기");

                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    ItemManagement();
                }
                else if(input == "0")
                {
                    MainMenu(this);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    ShowInventory();
                }
            }


            public void ItemManagement()
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리\n보유 중인 아이템을 관리할 수 있습니다.\n");

                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < items.Count; i++)
                {
                    Console.Write($"- {i+1} ");
                    if (items[i].equip) Console.Write("[E] ");
                    items[i].ShowItem();
                    Console.WriteLine();
                }
                Console.WriteLine("0. 나가기");

                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine();
                int input_int;
                if (int.TryParse(input, out input_int))
                {
                    if (input_int == 0)
                    {
                        ShowInventory();
                    }
                    else if (input_int > 0 && input_int <= items.Count)
                    {
                        if (items[input_int - 1].equip) Unequip(items[input_int - 1]);
                        else Equip(items[input_int - 1]);
                        ItemManagement();
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                        ItemManagement();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    ItemManagement();
                }
            }


            public void Rest()
            {
                Console.Clear();
                Console.WriteLine("휴식하기");
                Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. 보유 골드: {gold} G\n");

                Console.WriteLine("1. 휴식 하기");
                Console.WriteLine("0. 나가기");

                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    if(gold >= 500)
                    {
                        gold -= 500;
                        health += 40;
                        if (health >= 100) health = 100;
                        Console.WriteLine("휴식을 완료했습니다.");
                        Thread.Sleep(1000);
                        MainMenu(this);
                    }
                    else
                    {
                        Console.WriteLine("Gold 가 부족합니다.");
                        Thread.Sleep(1000);
                        MainMenu(this);
                    }
                }
                else if (input == "0")
                {
                    MainMenu(this);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    Rest();
                }
            }


            private void Equip(Item item)
            {

                if(item.itemType == 1)
                {
                    if (equippedArmor != null) Unequip(equippedArmor);
                    defence += item.stat;
                    itemDefence += item.stat;
                    equippedArmor = item;
                }
                else
                {
                    if(equippedWeapon != null) Unequip(equippedWeapon);
                    attack += item.stat;
                    itemAttack += item.stat;
                    equippedWeapon = item;
                }
                item.equip = true;
            }

            private void Unequip(Item item)
            {
                if (item.itemType == 1)
                {
                    defence -= item.stat;
                    itemDefence -= item.stat;
                    equippedArmor = null;
                }
                else
                {
                    attack -= item.stat;
                    itemAttack -= item.stat;
                    equippedWeapon = null;
                }
                item.equip = false;
            }


        }

        class Item
        {
            private string name;
            public int itemType;    // 타입 1 = 방어구, 타입 2 = 무기
            public int stat;       // 아이템으로 올라가는 수치
            public int price;
            public bool sold;
            public bool equip;
            public Item(string _name, int _type, int _stat, int _price)
            {
                name = _name;
                itemType = _type;
                stat = _stat;
                price = _price;
                sold = false;
                equip = false;
            }

            public void ShowItem()
            {
                if (itemType == 1)
                    Console.Write($"{name}| 방어력 +{stat}\t");
                else Console.Write($"{name}| 공격력 +{stat}\t");
            }
        }


        static void Store(List<Item> items, Character player)
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.gold} G \n");

            Console.WriteLine("[아이템 목록]");
            foreach(Item item in items)
            {
                Console.Write("- ");
                item.ShowItem();
                if (!item.sold) Console.WriteLine($" | {item.price} G");
                else Console.WriteLine(" | 판매 완료");
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기\n");

            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            string input = Console.ReadLine();

            if (input == "1")
            {
                BuyMenu(items, player);
            }
            else if(input == "2")
            {
                SellMenu(items, player);
            }
            else if (input == "0")
            {
                MainMenu(player);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(1000);
                Store(items, player);
            }
        }


        static void BuyMenu(List<Item> items, Character player)
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.gold} G \n");

            Console.WriteLine("[아이템 목록]");
            for(int i=0; i<items.Count; i++)
            {
                Console.Write($"- {i+1} ");
                items[i].ShowItem();
                if (!items[i].sold) Console.WriteLine($" | {items[i].price} G");
                else Console.WriteLine(" | 판매 완료");
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("0. 나가기\n");

            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            string input = Console.ReadLine();
            int input_int;
            if (int.TryParse(input, out input_int))
            {
                if (input_int == 0)
                {
                    Store(items, player);
                }
                else if (input_int > 0 && input_int <= items.Count)
                {
                    if (items[input_int - 1].sold)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        if (player.gold > items[input_int - 1].price)
                        {
                            player.Buy(items[input_int - 1]);
                            Console.WriteLine("구매를 완료했습니다.");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.WriteLine("Gold가 부족합니다.");
                            Thread.Sleep(1000);
                        }
                    }
                    BuyMenu(items, player);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    BuyMenu(items, player);
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(1000);
                BuyMenu(items, player);
            }
        }


        static void SellMenu(List<Item> items, Character player)
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요하지 않은 아이템을 판매할 수 있습니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.gold} G \n");

            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < player.items.Count; i++)
            {
                Console.Write($"- {i + 1} ");
                player.items[i].ShowItem();
                Console.WriteLine($" | 판매 가격: {player.items[i].price * 85 / 100} G");
            }

            Console.WriteLine("0. 나가기\n");

            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            string input = Console.ReadLine();
            int input_int;
            if (int.TryParse(input, out input_int))
            {
                if (input_int == 0)
                {
                    Store(items, player);
                }
                else if (input_int > 0 && input_int <= player.items.Count)
                {
                    if (player.items[input_int - 1].sold)
                    {
                        player.Sell(player.items[input_int - 1]);
                        Console.WriteLine("판매가 완료되었습니다.");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine("구매하지 않은 아이템입니다.");
                        Thread.Sleep(1000);
                    }
                    SellMenu(items, player);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    SellMenu(items, player);
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(1000);
                SellMenu(items, player);
            }
        }


        static void DungeonMenu(Character player)
        {
            Console.Clear();
            int diff1 = 5;
            int diff2 = 11;
            int diff3 = 17;
            Console.WriteLine("던전 입장");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            Console.WriteLine($"1. 쉬운 던전\t\t| 방어력 {diff1} 이상 권장");
            Console.WriteLine($"2. 일반 던전\t\t| 방어력 {diff2} 이상 권장");
            Console.WriteLine($"3. 어려운 던전\t\t| 방어력 {diff3} 이상 권장");
            Console.WriteLine("0. 나가기\n");

            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            string input = Console.ReadLine();
            int multi;
            if (input == "1")
            {
                multi = EnterDungeon(player, diff1);
                if(player.health <= 0)
                {
                    GameOver();
                }
            }
            else if (input == "2")
            {
                multi = EnterDungeon(player, diff2);
            }
            else if (input == "3")
            {
                multi = EnterDungeon(player, diff3);
            }
            else if (input == "0")
            {
                MainMenu(player);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(1000);
                DungeonMenu(player);
            }
        }

        static int EnterDungeon(Character player, int diff)
        {
            Random rand = new Random();
            int decreasedHealth;
            int result;
            if(player.defence >= diff)
            {
                decreasedHealth = rand.Next(20 - player.defence + diff, 35 - player.defence + diff + 1);    // 깎이는 체력 수치
                result = rand.Next(100 + player.attack, 100 + player.attack * 2 + 1);   // 보상 배율 랜덤 설정
            }
            else
            {
                int win = rand.Next(0, 10);
                if (win < 4)
                {
                    result = 0;
                    decreasedHealth = 50;
                }
                else
                {
                    decreasedHealth = rand.Next(20 - player.defence + diff, 35 - player.defence + diff + 1);    // 깎이는 체력 수치
                    result = rand.Next(100 + player.attack, 100 + player.attack * 2 + 1);   // 보상 배율 랜덤 설정
                }
            }
            player.health -= decreasedHealth;
            return result;
        }

        static void GameOver()
        {
            Console.Clear();
            Console.WriteLine("게임 오버");
            Console.WriteLine("체력이 0 이하로 떨어졌습니다.");

            //Console.WriteLine("\n1. 처음부터 다시 시작");
            //Console.WriteLine("0. 나가기");

            //string input = Console.ReadLine();
            //if(input == "1")
            //{
            //    Start();
            //}
            //else if(input == "0")
            //{
                    
            //}

            //else
            //{
            //    Console.WriteLine("잘못된 입력입니다.");
            //    Thread.Sleep(1000);
            //    GameOver();
            //}
        }
        
        static void DungeonResult(int diff)
        {
            Console.Clear();
            Console.WriteLine("던전 클리어");
            Console.WriteLine("축하합니다");
            Console.WriteLine($"{(DungeonDiff)diff} 던전을 클리어 하였습니다.");

            Console.WriteLine("\n[탐험 결과]");
            Console.WriteLine($"체력 {player.health} -> {player.health - decreasedHealth}");
            Console.WriteLine($"Gold {player.gold} -> {player.gold + earnGold}");

            Console.WriteLine("\n0. 나가기");

            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            string input = Console.ReadLine();
            if(input == "0")
            {
                DungeonMenu(player);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(1000);
                DungeonResult(diff);
            }
        }



        static void Main(string[] args)
        {
            string name;
            int class_type;

            name = Start();
            class_type = SelectClass();

            Character player = new Character(name, class_type); // player class 초기화
            itemList.Add(new Item("수련자 갑옷\t\t", 1, 5, 1000));
            itemList.Add(new Item("무쇠 갑옷\t\t", 1, 9, 2000));
            itemList.Add(new Item("스파르타의 갑옷  \t", 1, 15, 3500));
            itemList.Add(new Item("낡은 검\t\t", 2, 2, 600));
            itemList.Add(new Item("청동 도끼\t\t", 2, 5, 1500));
            itemList.Add(new Item("스파르타의 창 \t", 2, 7, 2300));
            MainMenu(player); // 게임 시작
        }
    }
}
