#include "makeroom.h"
#include "ui_makeroom.h"
#include <iostream>

using namespace std;


MakeRoom::MakeRoom(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::MakeRoom)
{
    ui->setupUi(this);
}

MakeRoom::~MakeRoom()
{
    delete ui;
}

void MakeRoom::on_pushButton_makeRoom_clicked()
{

}
