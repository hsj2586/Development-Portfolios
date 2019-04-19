#include "registration.h"
#include "ui_registration.h"
#include <iostream>
#include <QMessageBox>

using namespace std;

Registration::Registration(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Registration)
{
    ui->setupUi(this);
    ui->lineEdit_id->setPlaceholderText("put you id here");
    ui->lineEdit_pw->setPlaceholderText("put you pw here");
    ui->lineEdit_pw->setPlaceholderText("put you phone-number here");
    ui->lineEdit_email->setPlaceholderText("put you email here");
}

Registration::~Registration()
{
    delete ui;
}

void Registration::on_Button_registration_clicked()
{

}
