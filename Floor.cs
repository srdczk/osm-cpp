using System.Collections.Generic;

namespace OSM
{
    // 楼层实体类
    public class Floor
    {
        private int floor;
        // 结束楼层的布局不一样
        private bool isEnd;
        private double startX;
        private double startY;
        
        private HashSet<Ped> peds = new HashSet<Ped>();
        
        // 需要 remove 的
        public HashSet<Ped> wantRemove = new HashSet<Ped>();

        // Second Interval ped
        public HashSet<Ped> fromUp = new HashSet<Ped>();

        // remove ped
        public HashSet<Ped> toDel = new HashSet<Ped>();

        private List<Wall> walls = new List<Wall>();
        // 虚拟墙面, 用来绘制 阶梯 <---> 计算场域值的大小
        private List<Wall> vWalls = new List<Wall>();

        public Floor(int f, bool e)
        { 
            floor = f;
            isEnd = e;
            // startX, 和 startY 可以通过 floor 计算出来
            // 增加墙面
            // 先添加必须添加的
            // 每一行最多 10 个
            int x = floor / 10, y = floor % 10;
            startY = 5 + x * (10);
            startX = 5 + (9 - y) * (5);
            walls.Add(new Wall(startX, startY - 1.5, startX, startY + 6.6));
            walls.Add(new Wall(startX, startY + 6.6, startX + 3.5, startY + 6.6));
            walls.Add(new Wall(startX + 1.5, startY + 5.1, startX + 2.0, startY + 5.1));
            walls.Add(new Wall(startX + 2.0, startY + 5.1, startX + 2.0, startY + 1.5));
            walls.Add(new Wall(startX + 2.0, startY + 1.5, startX + 1.5, startY + 1.5));
            walls.Add(new Wall(startX + 1.5, startY + 1.5, startX + 1.5, startY + 5.1));
            walls.Add(new Wall(startX + 1.5, startY - 1.5, startX + 1.5, startY));
            if (isEnd) 
            {
                walls.Add(new Wall(startX + 3.5, startY + 6.6, startX + 3.5, startY + 1.5));
                walls.Add(new Wall(startX + 3.5, startY + 1.5, startX + 5, startY + 1.5));
                walls.Add(new Wall(startX + 1.5, startY, startX + 5, startY));
            } 
            else 
            {
                walls.Add(new Wall(startX + 3.5, startY + 6.6, startX + 3.5, startY));
                walls.Add(new Wall(startX + 1.5, startY, startX + 3.5, startY));
            }
            vWalls.Add(new Wall(startX + 2.0, startY, startX + 2.0, startY + 1.5));
            for (int i = 0; i < 13; i++) 
            {
                vWalls.Add(new Wall(startX, startY + 1.5 + 0.3 * i, startX + 1.5, startY + 1.5 + 0.3 * i));
                vWalls.Add(new Wall(startX + 2.0, startY + 1.5 + 0.3 * i, startX + 3.5, startY + 1.5 + 0.3 * i));
            }
        }

        public int GetFloor() 
        {
            return floor;
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public List<Wall> GetVirtual() 
        {
            return vWalls;
        }

        public List<Wall> GetWalls() 
        {
            return walls;
        }

        public double GetStartX() 
        {
            return startX;
        }

        public double GetStartY() 
        {
            return startY;
        }

        public void RemovePed(Ped ped)
        {
            peds.Remove(ped);
        }

        public void AddPed(Ped ped)
        {
            peds.Add(ped);
        }

        public HashSet<Ped> GetPeds()
        {
            return peds;
        }

        public void FromUpAdd(Ped ped)
        {
            fromUp.Add(ped);
        }

        public void AddDel(Ped ped)
        {
            toDel.Add(ped);
        }
        
        public void DoUpdate()
        {
            foreach (var delPed in toDel)
            {
                peds.Remove(delPed);
            }
        }

        public void FromUpRemove(Ped ped)
        {
            fromUp.Remove(ped);
        }

        public HashSet<Ped> GetFromUp()
        {
            return fromUp;
        }

        public bool IsFromUp(Ped ped)
        {
            return fromUp.Contains(ped);
        }

    }
}