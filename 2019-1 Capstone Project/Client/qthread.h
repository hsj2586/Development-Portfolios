#ifndef QTHREAD_H
#define QTHREAD_H

#include <QDialog>

namespace Ui {
class QThread;
}

class QThread : public QDialog
{
    Q_OBJECT

public:
    explicit QThread(QWidget *parent = 0);
    ~QThread();

private:
    Ui::QThread *ui;
};

#endif // QTHREAD_H
