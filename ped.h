#ifndef OSM_CPP_PED_H
#define OSM_CPP_PED_H

#include "pos.h"

class Ped {
private:
    Pos curPos_;
    // space 指针 -> 指向 所处的环境
    void *space_;
public:
    Ped(double x, double y, void *space);
    void move();
    double getX();
    double getY();
};


#endif //OSM_CPP_PED_H
