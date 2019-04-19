#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QMessageBox>
#include <iostream>

MainWindow::MainWindow(QWidget *parent) : QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    // text: before edit lineEdit
    ui->lineEdit_id_2->setPlaceholderText("Enter your student ID");
    ui->lineEdit_pw_2->setPlaceholderText("Enter your password");

    char buf[16] = "127.0.0.1";
    int port = 3490;

    this->chattingClient = new ChattingClient(*this, buf, port);
    this->chattingClient->start();
    // about after login page
    // ***server: get room numbers

    //connect: when changeIndex trigger -> changeStack slot start
    connect(this, SIGNAL(changeIndex(int)), this, SLOT(changeStack(int)));
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_Button_register_clicked()
{
    // create object 'Registration'
    regis_window = new Registration(this);
    regis_window->show();
}

void MainWindow::changeStack(int index)
{
    std::cout<<"in changeStack slot"<<std::endl;
    if(index != currentIndex) {
        currentIndex = index;
        ui->stackedWidget->setCurrentIndex(currentIndex);
        //emit changeIndex(currentIndex);
    }
}

void MainWindow::on_Button_login_2_clicked()
{
    // log-in function
    Json::Value root;
    Json::FastWriter fastWriter;
    std::string str;

    root["type"] = MessageType::LOGIN_PASS;
    root["id"] = ui->lineEdit_id_2->text().toStdString();
    root["password"] = ui->lineEdit_pw_2->text().toStdString();

    str = fastWriter.write(root);
    this->chattingClient->sendMessage(str);
}

// Making Room
// admin: professor --> insert db info
// this from server? or in ui?
void MainWindow::on_Button_makeRoom_clicked()
{
    //***DB check needed
    //makeRoom_window = new MakeRoom(this);
    //makeRoom_window->show();

    //***Server
    //get roomNo
    //get number of students
    QString roomNo = "517";
    QString numStudents = "30";
    ui->label_makeRoom->setText(QString("RoomNo:%1 students:%2/40").arg(roomNo, numStudents));
}

// Log out
void MainWindow::on_Button_toHome_clicked()
{
    ui->stackedWidget->setCurrentIndex(0);
}
