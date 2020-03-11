

#ifndef OSM_CPP_SCENE_H
#define OSM_CPP_SCENE_H

#include <QWidget>
#include <QTimer>
#include <QTimerEvent>
#include <QPushButton>
#include <QFormLayout>
#include "space.h"

class Scene : public QWidget {
Q_OBJECT
public:
    explicit Scene(QWidget *parent = nullptr);

    void keyPressEvent(QKeyEvent *event) override;
    void paintEvent(QPaintEvent *event) override;
    void timerEvent(QTimerEvent *event) override;
signals:

private:
    int timerId_;
    double x, y;
    // 开始按钮, 初始化按钮 (输入层数 + init)
    QPushButton *startButton_, *initButton_;
    QFormLayout *layout_;
    Space space;
};


#endif //OSM_CPP_SCENE_H
