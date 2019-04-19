#include "labelclicked.h"

LabelClicked::LabelClicked(const QString& text, QWidget * parent) : QLabel(parent)
{
    this->setText(text);
}

LabelClicked::~LabelClicked()
{

}

void LabelClicked::mouseReleaseEvent(QMouseEvent *event)
{
    //qDebug() << "Lable Clicked";
    //emit clicked();
}
