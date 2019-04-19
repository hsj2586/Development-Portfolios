#pragma once

#ifndef __CHATTINGCLIENT_CONF__
#define __CHATTINGCLIENT_CONF__

#include <WinSock2.h>
#include <Windows.h>
#include "mainwindow.h"
#include "json.h"
#include "cthread.h"
#include "chatexception.h"
#include <iostream>
#include <cstring>
#include <cstdio>
#include <QObject>
#include <gthread.h>

using namespace std;

class ChattingClient;
class SendThread;
class RecvThread;
class SendRecvInterface;

class MainWindow;

namespace UserCommand {
const char* const EXIT = "/exit";
};

typedef struct _MSG {
    char data[256];
} Message;

namespace MessageType
{
enum Type { LOGIN_PASS = 1, TEXT_MESSAGE = 2, ENTERROOM_REQUSET = 3 };
}

class ChattingClient : public CThread{
private:
    MainWindow *mainwindow;
    SendThread *st;
    RecvThread *rt;
    SOCKET client_socket;
    SOCKADDR_IN server_address;
    void connectServer();

public:
    ChattingClient(MainWindow& mainWindow, const char *ip, int port);
    ~ChattingClient();

    ChattingClient& getChattingClient();
    SOCKET& getClientSocket();
    void RedirectConnection(const char *ip, int port);
    void sendMessage(std::string message);
    virtual DWORD run(void);

    static const int MAXSTRLEN;
};


class SendRecvInterface : public CThread {
public:
    virtual DWORD run(void) = 0;
    int sendMessage(SOCKET socket, const char *buf);
    int recvMessage(SOCKET socket, char *buf);
};


class SendThread : public SendRecvInterface {
private:
    SOCKET client_socket;
    ChattingClient chatting_client;
    std::string message;
public:
    SendThread(SOCKET cs, ChattingClient& cc, std::string message);
    void RedirectSocket(SOCKET sock);
    virtual DWORD run(void);
    bool exitUser(const char *buf);
};

class RecvThread : public SendRecvInterface {
private:
    SOCKET client_socket;
    ChattingClient chatting_client;
public:
    RecvThread(SOCKET cs, ChattingClient& cc);
    void RedirectSocket(SOCKET sock);
    virtual DWORD run(void);
    Json::Value ParseMessage(std::string message);
};

#endif
