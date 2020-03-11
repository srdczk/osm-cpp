#ifndef OSM_CPP_SPACE_H
#define OSM_CPP_SPACE_H

#include <vector>
#include "ped.h"
#include "wall.h"

class Space {
private:
    // 容器
    std::vector<Ped> peds;
    std::vector<Wall> walls;
public:
    void init();
    std::vector<Ped> *getPeds();
    std::vector<Wall> *getWalls();
    void addPed(Ped &ped);
    void addWall(Wall &wall);
    void addPed(Ped &&ped);
    void addWall(Wall &&wall);
};


#endif //OSM_CPP_SPACE_H
