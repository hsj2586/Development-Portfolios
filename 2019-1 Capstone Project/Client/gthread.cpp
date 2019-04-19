#include "gthread.h"

GThread::GThread()
{

}

void GThread::run()
{
    emit changeStack(1);
}
