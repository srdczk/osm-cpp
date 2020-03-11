#include "space.h"

std::vector<Ped> *Space::getPeds() {
    return &peds;
}

std::vector<Wall> *Space::getWalls() {
    return &walls;
}

void Space::addPed(Ped &&ped) {
    peds.push_back(ped);
}

void Space::addWall(Wall &&wall) {
    walls.push_back(wall);
}

void Space::init() {
    // 初始化, 场景加人
    addWall(Wall{10, 20, 10, 40});
    addWall(Wall{20, 40, 30, 40});
}

void Space::addPed(Ped &ped) {
    peds.push_back(ped);
}

void Space::addWall(Wall &wall) {
    walls.push_back(wall);
}






