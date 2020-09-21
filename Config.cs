using System;

namespace OSM
{
    // 初始参数的声明
    public class Config
    {
        public static int maxFloor = 30;

        public static double WIDTH = 0, HEIGHT = 0;

        public static bool isRunning = false;

        public static double SCALE = 14;

        public static int pedId = 0;

        public static int Q = 20;

        public static double R = 0.2;

        public static double stepLen = 0.3;

        public static double miuP = 10000;
        public static double vP = 0.4;
        public static double aP = 1;
        public static double bP = 0.2;
        public static double gP = 0.4;
        public static double hP = 1;
        

        public static double miuO = 10000;
        public static double vO = 0.2;
        public static double aO = 3;
        public static double bO = 2;
        public static double hO = 6;
        
        public static double FI = Math.PI / 2.0;
        
        public static double delt = 0.2;

        // 楼梯长度
        public static double stairLen = 0.3;

        // number of step
        public static int maxStep = 12;

        // split step
        public static int Sp = 3;

        // update program 's SCALE
        public static double LargeScale = 30.0;

        public static int getTarget = 0;

        // count the number of ped who get target
        public static int delCnt = 0;

        // ped every thing
        public static int floorPedSum = 0;
    }
}