

#include "wall.h"
#include <QtMath>

bool Wall::isZero(double d) {
    return qAbs(d) < 1e-6;
}

Wall::Wall(const Pos &begin, const Pos end) {
    begin_ = Pos(begin.getX(), begin.getY());
    end_ = Pos(end.getX(), end.getY());
}

Wall::Wall(double x1, double y1, double x2, double y2) {
    begin_ = Pos(x1, y1);
    end_ = Pos(x2, y2);
}

Pos Wall::crossPoint(const Pos &pos) {
    // 如果是 x 轴平行线或者 y 轴平行线
    if (isZero(begin_.getX() - end_.getX())) {
        // 直线方程 x = begin_.getX();
        return Pos(begin_.getX(), pos.getY());
    } else if (isZero(begin_.getY() - end_.getY())) {
        // 直线方程 y = begin_.getY();
        return Pos(pos.getX(), end_.getY());
    } else {
        double aW = (end_.getY() - begin_.getY()) / (end_.getX() - begin_.getX())
                , bW = begin_.getY() - aW * begin_.getX(), aV = (-1) / aW
                        , bV = pos.getY() - aV * pos.getX();
        return Pos((bV - bW) / (aW - aV), aW * ((bV - bW) / (aW - aV)) + bW);
    }
}

bool Wall::isIn(const Pos &pos, double r) {
    Pos metaDir = end_.normalize(begin_).multiply(r / 2.0);
    Pos xBegin = begin_.subtract(metaDir);
    Pos xEnd = end_.add(metaDir);
    // 判断是否在直线内
    return pos.getX() >= qMin(xBegin.getX(), xEnd.getX())
    && pos.getX() <= qMax(xBegin.getX(), xEnd.getX())
    && pos.getY() >= qMin(xBegin.getY(), xEnd.getY())
    && pos.getY() <= qMax(xBegin.getY(), xEnd.getY());
}

double Wall::distanceTo(const Pos &pos) {
    // 点到直线的距离
    Pos tmpPos = crossPoint(pos);
    return tmpPos.distanceTo(pos);
}

Pos Wall::getBegin() {
    return Pos(begin_.getX(), begin_.getY());
}

Pos Wall::getEnd() {
    return Pos(end_.getX(), end_.getY());
}











