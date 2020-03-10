#include "mainwindow.h"
#include "scene.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent) {

    auto scene = new Scene();

    this->setCentralWidget(scene);
    this->setFixedSize(scene->size());
    this->setWindowTitle("osm model");
}

MainWindow::~MainWindow() {

}

