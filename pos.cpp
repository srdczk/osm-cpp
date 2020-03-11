

#include <QtMath>
#include "pos.h"

bool Pos::isZero(double d) {
    return qAbs(d) < 1e-6;
}

Pos::Pos() {
    x_ = 0;
    y_ = 0;
}


Pos::Pos(double x, double y) {
    x_ = x;
    y_ = y;
}

double Pos::length() {
    // 二维向量的长度
    return qSqrt(x_ * x_ + y_ * y_);
}

double Pos::distanceTo(const Pos &pos) {
    // 两个向量之间的距离
    return qSqrt((x_ - pos.x_) * (x_ - pos.x_) + (y_ - pos.y_) * (y_ - pos.y_));
}

Pos Pos::reverse() {
    return Pos(-x_, -y_);
}

Pos Pos::add(const Pos &pos) {
    return Pos(x_ + pos.x_, y_ + pos.y_);
}

Pos Pos::add(double x, double y) {
    return Pos(x_ + x, y_ + y);
}

Pos Pos::subtract(double x, double y) {
    // 向量相减
    return Pos(x_ - x, y_ - y);
}

Pos Pos::subtract(const Pos &pos) {
    return subtract(pos.x_, pos.y_);
}

Pos Pos::multiply(double x, double y) {
    return Pos(x_ * x, y_ * y);
}

Pos Pos::multiply(const Pos &pos) {
    return multiply(pos.x_, pos.y_);
}

Pos Pos::multiply(double d) {
    return Pos(d * x_, d * y_);
}

Pos Pos::normalize(const Pos &pos) {
    if (isZero(x_ - pos.x_) && isZero(y_ - pos.y_)) return Pos(1, 0);
    double distance = distanceTo(pos);
    double x = x_ - pos.x_;
    double y = y_ - pos.y_;
    return Pos((x_ - pos.x_) / distance, (y_ - pos.y_) / distance);
}

Pos Pos::rotateInt(int angle) {
    double res = angle / 180.0 * M_PI;
    return rotateDouble(res);
}

Pos Pos::rotateDouble(double angle) {
    // 逆时针转动弧度制的值
    return Pos(x_ * qCos(angle) - y_ * qSin(angle), y_ * qCos(angle) + x_ * qSin(angle));
}

double Pos::getX() const {
    return x_;
}

double Pos::getY() const {
    return y_;
}

void Pos::setX(double x) {
    x_ = x;
}

void Pos::setY(double y) {
    y_ = y;
}




















