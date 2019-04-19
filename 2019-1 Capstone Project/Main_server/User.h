#pragma once

#ifndef __USER_CONF__
#define __USER_CONF__

#include <iostream>
#include <vector>
#include <sstream>
#include <WinSock2.h>
#include "ChatException.h"
#include "CThread.h"

using std::cout;
using std::cin;
using std::endl;
using std::vector;
using std::stringstream;
#include "json.h"

class RecvThread;
class User;

typedef struct _MSG{
	char data[256];
} Message;

class User : public CThread {
private:
	SOCKET client_socket;
	SOCKADDR_IN client_address;
	static const int MAXSTRLEN;

	void recvMessage(char *buf);
	void ParseMessage(std::string message);
	void sendMessage(SOCKET socket, const char *buf=nullptr);

public:
	User(SOCKET cs, SOCKADDR_IN ca);
	User(const User &user) {}
	void operator=(const User &user) {}
	~User();

	char* getIP() const;
	int getPort() const;
	SOCKET getSocket() const;
	void closeSession();

	void sendMessageAll(const char *buf=nullptr);
	void sendMessageInRoom(const char *buf = nullptr);
	void User::printRoomInfoList();

	DWORD run(void);
};

#endif