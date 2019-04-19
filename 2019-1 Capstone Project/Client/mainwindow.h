#pragma once

#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include "chattingclient.h"
#include "registration.h"
#include "makeroom.h"
#include <QMainWindow>
#include <gthread.h>

class ChattingClient;

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

private slots:
    void on_Button_register_clicked();
    void on_Button_login_2_clicked();
    void on_Button_makeRoom_clicked();
    void on_Button_toHome_clicked();

public slots:
    void changeStack(int index);

private:
    Ui::MainWindow *ui;
    ChattingClient *chattingClient;
    Registration *regis_window;
    MakeRoom *makeRoom_window;
    GThread *gThread;
    int currentIndex;

signals:
    void changeIndex(int index);
};

#endif // MAINWINDOW_H
