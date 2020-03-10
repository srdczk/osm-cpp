//
// Created by srdczk on 20-3-10.
//

#ifndef OSM_CPP_SCENE_H
#define OSM_CPP_SCENE_H

#include <QWidget>
#include <QTimer>
#include <QTimerEvent>
#include <QPushButton>
#include <QFormLayout>

class Scene : public QWidget {
Q_OBJECT
public:
    explicit Scene(QWidget *parent = nullptr);

    void keyPressEvent(QKeyEvent *event);
    void paintEvent(QPaintEvent *event);
    void timerEvent(QTimerEvent *event);
signals:

private:
    int timerId_;
    int x, y;
    // 开始按钮, 初始化按钮 (输入层数 + init)
    QPushButton *startButton_, *initButton_;
    QFormLayout *layout_;
};


#endif //OSM_CPP_SCENE_H
