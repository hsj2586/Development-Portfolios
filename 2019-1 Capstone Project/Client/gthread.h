#ifndef GTHREAD_H
#define GTHREAD_H
#include<QThread>
#include<QObject>

class GThread : public QThread
{
    Q_OBJECT

public:
    GThread();

private:
    void run();

signals:
    void changeStack(int index);
};

#endif // GTHREAD_H
