import socket
import threading
import time
import sys
import os
import base64


# Server Socket
def socket_service():
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        # 防止socket server重启后端口被占用（socket.error: [Errno 98] Address already in use）
        s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        s.bind(('127.0.0.1', 6666))
        s.listen(10)
    except socket.error as msg:
        print(msg)
        sys.exit(1)
    print('Waiting connection...')

    while 1:
        conn, addr = s.accept()
        t = threading.Thread(target=deal_data, args=(conn, addr))
        t.start()


# Dealing with data
def deal_data(conn, addr):
    print('Accept new connection from {0}'.format(addr))
    conn.send(bytes('Hi, Welcome to the server!'.encode("utf-8")))
    while 1:
        data = conn.recv(300000)
        print('{0} client send data is {1}'.format(addr, data))
        # Decode image data
        base64Str = data.decode("ascii")
        imgData = base64.b64decode(base64Str)
        print("The base64 string is:{0}", base64Str)
        file = open('1.jpg', 'wb')
        file.write(imgData)
        file.close()

        # time.sleep(1)
        if data == 'exit' or not data:
            print('{0} connection close'.format(addr))
            conn.send(bytes('Connection closed!').encode("utf-8"))
            break
        conn.send(('Hello, {0}'.format(data)).encode("utf-8"))
    conn.close()


if __name__ == '__main__':
    socket_service()
