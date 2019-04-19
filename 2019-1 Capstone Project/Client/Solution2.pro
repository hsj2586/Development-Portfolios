#-------------------------------------------------
#
# Project created by QtCreator 2019-04-09T16:45:33
#
#-------------------------------------------------

QT       += core gui sql network

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = Solution2
TEMPLATE = app

# The following define makes your compiler emit warnings if you use
# any feature of Qt which has been marked as deprecated (the exact warnings
# depend on your compiler). Please consult the documentation of the
# deprecated API in order to know how to port your code away from it.
DEFINES += QT_DEPRECATED_WARNINGS

# You can also make your code fail to compile if you use deprecated APIs.
# In order to do so, uncomment the following line.
# You can also select to disable deprecated APIs only up to a certain version of Qt.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0


SOURCES += \
    main.cpp \
    mainwindow.cpp \
    registration.cpp \
    makeroom.cpp \
    labelclicked.cpp \
    cthread.cpp \
    json.cpp \
    chattingclient.cpp \
    gthread.cpp

HEADERS += \
        mainwindow.h \
    registration.h \
    makeroom.h \
    labelclicked.h \
    chatexception.h \
    chattingclient.h \
    cthread.h \
    json.h \
    gthread.h

FORMS += \
    mainwindow.ui \
    registration.ui \
    makeroom.ui

RESOURCES += \
    src.qrc

LIBS += -lwsock32 \
