#include "util.h"


bool isZero(double d) {
    return qAbs(d) < 1e-6;
}


// 生成转角场域
vector<vector<double>> generateField(double x) {
    int len = (int)(x * 10);
    int n = 200;
    double dx = x / len;
    vector<vector<double>> res(len, vector<double>(len));
    // 每个各点 一个 Node 数组
    vector<vector<vector<Node>>> nodes(len, vector<vector<Node>>(len));

    for (int k = 1; k <= n / 2; ++k) {
        double tanA = qTan(M_PI / 2.0 / n * k);
        double sinA = qSin(M_PI / 2.0 / n * k);
        double cosA = qCos(M_PI / 2.0 / n * k);
        if (k < n / 2) {
            for (int i = 1; i <= len; i++) {
                double dy = (i * dx) * tanA;
                int s = (int)(dy / dx);
                double pd = dx * tanA;
                double py = dy - s * dx;
                if (!isZero(py)) {
                    if (py > pd) {
                        nodes[i - 1][s].push_back(Node(dx / cosA, ((double)k / n) * M_PI / 2.0 * x));
                        nodes[s][i - 1].push_back(Node(dx / cosA, ((double)(n - k) / n) * M_PI / 2.0 * x));
                    } else {

                        nodes[i - 1][s].push_back(Node(py / sinA, ((double)k / n) * M_PI / 2.0 * x));
                        nodes[s][i - 1].push_back(Node(py / sinA, ((double)(n - k) / n) * M_PI / 2.0 * x));
                    }
                }
            }
        } else {
            for (int i = 0; i < len; ++i) {
                nodes[i][i].push_back(Node(dx / qCos(M_PI / 4.0), 0.5 * M_PI / 2.0 * x));
            }
        }
    }
    for (int i = 0; i < len; ++i) {
        nodes[0][i].push_back(Node(dx, M_PI / 2.0 * x));
    }

    for (int i = 0; i < len; ++i) {
        for (int j = 0; j < len; ++j) {
            if (nodes[i][j].empty()) res[i][j] = 0;
            else {
                double p = 0, q = 0;
                for (auto &node : nodes[i][j]) {
                    p += node.len * node.s;
                    q += node.len;
                }
                res[i][j] = p / q;
            }
        }
    }
    return res;
}

void drawCircle(double x, double y, double r, QPainter *painter) {
    // 半径
    painter->drawEllipse(QPointF((x + WIDTH) * SCALE, (y + HEIGHT) * SCALE), r * SCALE, r * SCALE);
}

void drawWall(Wall *wall, QPainter *painter) {
    painter->drawLine(QPointF((wall->getBegin().getX() + WIDTH) * SCALE, (wall->getBegin().getY() + HEIGHT) * SCALE),\
    QPointF((wall->getEnd().getX() + WIDTH) * SCALE, (wall->getEnd().getY() + HEIGHT) * SCALE));
}

static auto sff = generateField(1.5);

double getSff(Pos vector) {
    // 计算场域的值
    if (vector.getX() <= 5) {
        return 1000000 - vector.getX() * 100;
    }
    if (vector.getX() >= 5 && vector.getX() <= 6.5
        && vector.getY() >= 5 && vector.getY() <= 6.5) {
        int x = (int) ((vector.getX() - 5.0) * 10)
        , y = (int) ((6.5 - vector.getY()) * 10);
        if (x > 14 || y > 14) return 1000000;
        return 900000 + sff[x][y] * 50;
    }
    if (vector.getX() >= 5 && vector.getX() <= 6.5
        && vector.getY() >= 6.5 && vector.getY() <= 10.1) {
        return 900000 - (vector.getY() - 6.5) * 100;
    }
    if (vector.getX() >= 5 && vector.getX() <= 6.5
        && vector.getY() >= 10.1) {
        int x = (int) ((vector.getY() - 10.1) * 10)
        , y = (int) ((6.5 - vector.getX()) * 10);
        if (x > 14 || y > 14) return 1000000;
        return 800000 + sff[x][y] * 50;
    }
    if (vector.getX() >= 6.5 && vector.getX() <= 7.0) {
        return 800000 - (vector.getX() - 6.5) * 100;
    }
    if (vector.getX() >= 7 && vector.getX() <= 8.5
        && vector.getY() <= 11.6 && vector.getY() >= 10.1) {
        int x = (int) ((vector.getX() - 7.0) * 10)
        , y = (int) ((vector.getY() - 10.1) * 10);
        if (x > 14 || y > 14) return 1000000;
        return 700000 + sff[x][y] * 50;
    }
    if (vector.getX() >= 7 && vector.getX() <= 8.5
        && vector.getY() <= 10.1 && vector.getY() >= 6.5) {
        return 700000 + (vector.getY() - 10.1) * 100;
    }
    if (vector.getX() >= 7 && vector.getX() <= 8.5
        && vector.getY() <= 6.5 && vector.getY() >= 5) {
        int x = (int) ((6.5 - vector.getY()) * 10)
        , y = (int) ((8.5 - vector.getX()) * 10);
        if (x > 14 || y > 14) return 1000000;
        return 600000 + sff[x][y] * 50;
    }
    if (vector.getX() >= 8.5) {
        return 600000 - (vector.getX() - 8.5) * 100;
    }
    return 1000000;
}

double calculateFormPed(Ped *ped, double x, double y) {
    double dis = qSqrt((x - ped->getX()) * (x - ped->getX()) \
    + (y - ped->getY()) * (y - ped->getY()));
    if (dis <= GP) {
        return MIUP;
    } else if (dis > GP && dis <= GP + HP) {
        return VP * qExp(-AP * qPow(dis, BP));
    } else {
        return 0;
    }
}

double calculateFormWall(Wall *wall, double x, double y) {
    Pos pos(x, y);
    if (wall->isIn(pos, 0)) {

        double dis = wall->distanceTo(pos);
        if (dis < GP / 2.0) {
            return MIUO;
        } else if (dis >= GP / 2.0 && dis <= HO) {
            return VO * qExp(-AO * qPow(dis, BO));
        } else return 0;

    } else return 0;
}

bool canMove(Pos target) {
    if (target.getX() <= 5 && target.getY() >= 6.5) {
        return false;
    }
    if (target.getY() >= 11.6) return false;
    if (target.getX() >= 8.5 && target.getY() <= 11.6
        && target.getY() >= 6.5) return false;
    if (target.getY() <= 5) return false;
    if (target.getX() <= 7 && target.getX() >= 6.5
        && target.getY() <= 10.1) return false;
    return true;
}
