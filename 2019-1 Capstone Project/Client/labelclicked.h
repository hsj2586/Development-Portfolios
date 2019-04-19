#ifndef LABELCLICKED_H
#define LABELCLICKED_H

#include <QObject>
#include <QWidget>
#include <QLabel>

#include <QMouseEvent>

class LabelClicked : public QLabel
{
    Q_OBJECT

public:
    LabelClicked(const QString& text ="", QWidget * parent = 0);
    ~LabelClicked();

signals:
    void clicked();

protected:
    void mouseReleaseEvent( QMouseEvent * event ) ;
};

#endif // LABELCLICKED_H
