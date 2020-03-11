
#ifndef OSM_CPP_POS_H
#define OSM_CPP_POS_H


class Pos {
private:
    bool isZero(double d);
    double x_, y_;
public:
    Pos();
    Pos(double x, double y);
    double length();
    double distanceTo(const Pos &pos);
    Pos reverse();
    Pos add(const Pos &pos);
    Pos add(double x, double y);
    Pos subtract(double x, double y);
    Pos subtract(const Pos &pos);
    Pos multiply(double x, double y);
    Pos multiply(const Pos &pos);
    Pos multiply(double d);
    Pos normalize(const Pos &pos);
    Pos rotateInt(int angle);
    Pos rotateDouble(double angle);
    double getX() const;
    double getY() const;
    void setX(double x);
    void setY(double y);
};


#endif //OSM_CPP_POS_H
