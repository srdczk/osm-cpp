cmake_minimum_required(VERSION 3.5)

project(osm-cpp LANGUAGES CXX)

set(CMAKE_INCLUDE_CURRENT_DIR ON)

set(CMAKE_AUTOUIC ON)
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

find_package(Qt5 COMPONENTS Widgets REQUIRED)

add_executable(osm-cpp
        main.cpp
        mainwindow.cpp
        mainwindow.h
        scene.cpp
        scene.h
        ped.cpp
        ped.h
        pos.cpp pos.h)

target_link_libraries(osm-cpp PRIVATE Qt5::Widgets)
