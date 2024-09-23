namespace TextRPG_project
{
    internal class Program
    {
        static void MainMenu(Character player)
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전에 들어가기 전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
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
                    //상점 보기
                    break;

                default:
                    Console.WriteLine("\n잘못된 입력입니다.");
                    MainMenu(player);
                    break;

            }

        }

            
        class Character
        {
            private int level;
            private string name;
            private int class_type;
            private int attack;
            private int defense;
            private int health;
            private int gold;
            private int itemAttack;     //아이템으로 올라간 총 공격력
            private int itemDefense;    //아이템으로 올라간 총 방어력

            private List<Item> items;   // 인벤토리의 아이템
            public Character(string _name, int class_num)
            {
                level = 1;
                name = _name;
                class_type = class_num;
                attack = 10;
                defense = 5;
                health = 100;
                gold = 1500;
                items = new List<Item>();
                itemAttack = 0;
                itemDefense = 0;
            }

            public void ShowStats()
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv : {level}");
                Console.Write($"직  업 : ");
                if (class_type == 1) Console.WriteLine("전사");
                else Console.WriteLine("도적");
                Console.Write($"공격력 : {attack} ");
                if (itemAttack > 0) Console.Write($"(+{itemAttack})");
                Console.Write($"\n방어력 : {defense}");
                if (itemDefense > 0) Console.Write($"(+{itemDefense})");
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

            public void Buy(Item item)
            {
                item.sold = true;
                items.Add(item);
            }

            public void ShowInventory()
            {
                Console.Clear();
                Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]");
                for(int i=0; i<items.Count; i++)
                {
                    if (items[i].equip) Console.Write("[E] ");
                    items[i].ShowItem();
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
                    Console.Write($"- {i} ");
                    if (items[i].equip) Console.Write("[E] ");
                    items[i].ShowItem();
                }
                Console.WriteLine("0. 나가기");

                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();
                int input_int = int.Parse(input);


               if (input_int == 0)
                {
                    ShowInventory();
                }
               else if(input_int > 0 && input_int <= items.Count)
                {
                    if (items[input_int].equip) unequip(items[input_int]);
                    else equip(items[input_int]);
                    ItemManagement();
                }
                else 
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    ItemManagement();
                }
            }

            private void equip(Item item)
            {
                if(item.itemType == 1)
                {
                    defense += item.stat;
                    itemDefense += item.stat;
                }
                else
                {
                    attack += item.stat;
                    itemAttack += item.stat;
                }
            }
            private void unequip(Item item)
            {
                if (item.itemType == 1)
                {
                    defense -= item.stat;
                    itemDefense -= item.stat;
                }
                else
                {
                    attack -= item.stat;
                    itemAttack -= item.stat;
                }
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
                    Console.Write($"{name}  | 방어력 +{stat}");
                else Console.Write($"{name} | 공격력 +{stat}");
            }
        }

        //class Store
        //{
        //    private Item[] item;

        //    public Store()
        //    {
        //        item = new Item[6];
        //        item[0] = new Item("수련자 갑옷", 1, 5, 1000);
        //        item[1] = new Item("무쇠 갑옷", 1, 9, 2000);
        //        item[2] = new Item("스파르타의 갑옷", 1, 15, 3500);
        //        item[3] = new Item("낡은 검", 2, 2, 600);
        //        item[4] = new Item("청동 도끼", 2, 5, 1500);
        //        item[5] = new Item("스파르타의 창", 2, 7, 2300);
        //    }

        //    public void ShowStore()
        //    {
        //        for(int i=0; i<item.Length; i++)
        //        {
        //            item[i].ShowItem();
        //            if (item[i].sold)
        //                Console.WriteLine("구매 완료");
        //            else Console.WriteLine($"{item[i].price} G");
        //        }
        //    }
        //}


        static void Main(string[] args)
        {
            string name;
            string input;
            string class_type;
            do
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("원하시는 이름을 선택해주세요.\n");

                name = Console.ReadLine();

                Console.WriteLine($"\n설정하신 이름은 {name}입니다.\n");
                Console.WriteLine("1. 저장\n2. 취소\n");
                Console.WriteLine("원하시는 행동을 선택해주세요.");

                input = Console.ReadLine();

                if (input != "1" && input != "2")
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                }
            } while (input != "1"); // 이름 선택 화면

            do
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                    Console.WriteLine("원하시는 직업을 선택해주세요.\n");
                    Console.WriteLine("1. 전사\n2. 도적");
                    Console.WriteLine("\n원하시는 행동을 선택해주세요.");
                    class_type = Console.ReadLine();

                    if (class_type == "1") Console.WriteLine("\n고른 직업은 전사입니다.\n");
                    else if (class_type == "2") Console.WriteLine("\n고른 직업은 도적 입니다.\n");
                    else
                    {
                        Console.WriteLine("\n잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                } while (class_type != "1" && class_type != "2");
                Console.WriteLine("1. 저장\n2. 취소\n");
                Console.WriteLine("원하시는 행동을 선택해주세요.");

                input = Console.ReadLine();

                if (input != "1" && input != "2")
                {
                    Console.WriteLine("\n잘못된 입력입니다.");
                    Thread.Sleep(1000);
                }

                
            } while (input != "1"); // 직업 선택 화면

            Character player = new Character(name, int.Parse(class_type)); // player class 초기화

            MainMenu(player); // 게임 시작
        }
    }
}
