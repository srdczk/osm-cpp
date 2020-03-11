#ifndef OSM_CPP_UTIL_H
#define OSM_CPP_UTIL_H

#pragma once

#include <vector>
#include <QPainter>
#include <QtMath>
#include "config.h"
#include "wall.h"
#include "ped.h"

using namespace std;

bool isZero(double d);

struct Node {
    double len, s;
    Node(double l, double p): len(l), s(p) {}
    Node(): Node(0, 0) {}
};

// 生成转角场域
vector<vector<double>> generateField(double x);

void drawCircle(double x, double y, double r, QPainter *painter);

void drawWall(Wall *wall, QPainter *painter);

double getSff(double x, double y);

double calculateFormPed(Ped ped);

#endif //OSM_CPP_UTIL_H
