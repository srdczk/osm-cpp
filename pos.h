//
// Created by srdczk on 20-3-10.
//

#ifndef OSM_CPP_POS_H
#define OSM_CPP_POS_H


class Pos {
private:
    double x_, y_;
public:
    Pos(double x, double y);
    double length();
    double distanceTo(Pos pos);
    Pos reverse();
    Pos add(const Pos &pos);
    Pos add(double x, double y);
    Pos subtract(double x, double y);
};


#endif //OSM_CPP_POS_H
