using System.Collections;
using System.Collections.Generic;

namespace OSM
{
    public class Space
    {
        // Space 的结构, 构建场景
        public static double[][] sff = CustomizeUtil.GenerateField(1.5);

        private List<Ped> peds;

        private List<Wall> walls;
        // int -> Floor -> 楼层索引获取指定楼层
        private Hashtable map = new Hashtable();

        public Space()
        {
            peds = new List<Ped>();
            walls = new List<Wall>();
        }

        public Space Init()
        {
            map.Clear();
            Config.floorPedSum = Config.maxFloor * 100;
            if (Config.maxFloor >= 10) Config.SCALE = 14;
            else Config.SCALE = 14 + (10 - Config.maxFloor) * 2;
            // 通过 maxFloor <---> 控制
            if (Config.maxFloor > 40) {
                Config.WIDTH = 0;
                Config.HEIGHT = 0;
            } else if (Config.maxFloor > 30) {
                Config.WIDTH = 0;
                Config.HEIGHT = 7;
            } else if (Config.maxFloor > 20) {
                Config.WIDTH = 0;
                Config.HEIGHT = 9;
            } else if (Config.maxFloor > 10) {
                Config.WIDTH = 0;
                Config.HEIGHT = 15;
            } else {
                Config.WIDTH = (Config.maxFloor - 10) * 5;
                Config.HEIGHT = 20 + (Config.maxFloor - 10) * 2;
            }
            for (int i = 0; i < Config.maxFloor; i++)
            {
                if (i == 0) map.Add(i, new Floor(0, true));
                else map.Add(i, new Floor(i, false));
            }
            for (int i = 0; i < Config.maxFloor; i++)
            {
                // 添加随机的行人
                AddRandomPed(i);
            }
            // 每层随机加入 15 个人 --->
            return this;
        }

        // add Random Ped from this
        private void CustomizeAdd()
        {

        }

        // 每一次加入一个行人
        public void RandomPed()
        {
            for (int i = 0; i < Config.maxFloor; i++)
            {
                var sPeds = ((Floor)map[i]).GetPeds();
                // 随机添加行人
                double x = 0.2 + CustomizeUtil.GetRandom() * 1.1, y = -1.3 + CustomizeUtil.GetRandom() * 1.1;
                bool pd = true;
                while (pd) 
                {
                    pd = false;
                    foreach (var ped in sPeds)
                    {
                        // 如果有重叠
                        if (CustomizeUtil.GetDistance(ped.GetCurPos().GetX(), ped.GetCurPos().GetY(), x, y) < 0.4)
                        {
                            x = 0.2 + CustomizeUtil.GetRandom() * 1.1;
                            y = -1.3 + CustomizeUtil.GetRandom() * 1.1;
                            pd = true;
                            break;
                        }
                    }
                }
                Vector sub = CustomizeUtil.CalXY(i);
                // 加入新的行人
                sPeds.Add(new Ped(Config.pedId++, x + sub.GetX(), y + sub.GetY(), Config.stepLen, Config.R, 1, 0, this, i));
            }
        }
        
        // 随机添加行人
        private void AddRandomPed(int floor)
        {
            var sPeds = ((Floor)map[floor]).GetPeds();
            // 初始化 15 个行人
            while (sPeds.Count < 15)
            {
                double x = 0.2 + CustomizeUtil.GetRandom() * 1.1, y = -1.3 + CustomizeUtil.GetRandom() * 1.1;
                bool pd = true;
                while (pd) {
                    pd = false;
                    foreach (var ped in sPeds)
                    {
                        // 如果有重叠
                        if (CustomizeUtil.GetDistance(ped.GetCurPos().GetX(), ped.GetCurPos().GetY(), x, y) < 0.4)
                        {
                            x = 0.2 + CustomizeUtil.GetRandom() * 1.1;
                            y = -1.3 + CustomizeUtil.GetRandom() * 1.1;
                            pd = true;
                            break;
                        }
                    }
                }

                Vector sub = CustomizeUtil.CalXY(floor);
                
                sPeds.Add(new Ped(Config.pedId++, x + sub.GetX(), y + sub.GetY(), Config.stepLen, Config.R, 1, 0, this, floor));
            }
        }

        public Hashtable GetMap()
        {
            return map;
        }

        public void AddPed(Ped ped)
        {
            peds.Add(ped);
        }

        public void AddWall(Wall wall)
        {
            walls.Add(wall);
        }

        public List<Ped> GetPeds()
        {
            return peds;
        }

        public List<Wall> GetWalls()
        {
            return walls;
        }
        
    }
}