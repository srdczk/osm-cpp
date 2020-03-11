#ifndef OSM_CPP_WALL_H
#define OSM_CPP_WALL_H

#include "pos.h"

// 墙面
class Wall {
private:
    bool isZero(double d);
    Pos begin_, end_;
public:
    Wall(const Pos &begin, const Pos end);
    Wall(double x1, double y1, double x2, double y2);
    Pos crossPoint(const Pos &pos);
    bool isIn(const Pos &pos, double r);
    double distanceTo(const Pos &pos);
    Pos getBegin();
    Pos getEnd();
};


#endif //OSM_CPP_WALL_H
