#include "qthread.h"
#include "ui_qthread.h"

QThread::QThread(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::QThread)
{
    ui->setupUi(this);
}

QThread::~QThread()
{
    delete ui;
}
