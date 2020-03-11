#include "ped.h"
#include "space.h"
#include "util.h"

Ped::Ped(double x, double y, void *space) {
    curPos_ = Pos(x, y);
    space_ = space;
}

void Ped::move() {
    auto space = (Space *)space_;

}

double Ped::getX() {
    return curPos_.getX();
}

double Ped::getY() {
    return curPos_.getY();
}


