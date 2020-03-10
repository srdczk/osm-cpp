//
// Created by srdczk on 20-3-10.
//

#include <iostream>
#include <QPainter>
#include <QKeyEvent>
#include <QDebug>
#include "scene.h"


Scene::Scene(QWidget *parent) {

    setFixedSize(800, 800);
    this->setFocusPolicy(Qt::StrongFocus);
    x = 30;
    y = 30;
    startButton_ = new QPushButton("start");
    initButton_ = new QPushButton("init");
    startButton_->setFixedSize(40, 20);
    initButton_->setFixedSize(40, 20);
    layout_ = new QFormLayout();
    layout_->addRow(startButton_, initButton_);
    setLayout(layout_);
    // start 按钮监听, -> 暂停和开始的转换
    connect(startButton_, &QPushButton::clicked, [=]() {
        if (startButton_->text() == "start") {
            timerId_ = startTimer(20);
            startButton_->setText("pause");
        } else {
            killTimer(timerId_);
            startButton_->setText("start");
        }
    });
}

void Scene::keyPressEvent(QKeyEvent *event) {
    qDebug() << "KeyPress";
}

void Scene::timerEvent(QTimerEvent *event) {
    if (event->timerId() == timerId_) {
        x++;
        y++;
        this->update();
    }
}


void Scene::paintEvent(QPaintEvent *event) {
    Q_UNUSED(event);

    QPainter painter(this);

    painter.setBrush(QBrush(QColor("#92877d")));
    painter.drawRect(this->rect());

    painter.setBrush(Qt::red);
    painter.drawEllipse(x, y, 30, 30);
}