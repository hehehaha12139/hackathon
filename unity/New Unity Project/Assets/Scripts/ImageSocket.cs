using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;
using System.IO;

public class ImageSocket : MonoBehaviour
{
    //Client Setting
    private TcpClient client;

    private void Client() {
        client = new TcpClient("127.0.0.1", 6666);
        try
        {
            //Translate message to byte 
            //String message = "First connection!";

            //Translate pictures to byte
            FileInfo fileInfo = new FileInfo("Assets/Pictures/freeAbrams.jpg");
            byte[] buffer = new byte[fileInfo.Length];
            using (FileStream fStream = fileInfo.OpenRead()) {
                fStream.Read(buffer, 0, buffer.Length);
            }

            string base64Image = Convert.ToBase64String(buffer);
            buffer = System.Text.Encoding.ASCII.GetBytes(base64Image);

            Debug.Log("The length of base64Buffer is:" + buffer.Length);

            //Get a stream for reading and writing
            NetworkStream stream = client.GetStream();

            //Send message
            stream.Write(buffer, 0, buffer.Length);
            Console.WriteLine("Sent: freeAbrams!");

            buffer = new byte[256];
            String responseData = String.Empty;
            Int32 bytes = stream.Read(buffer, 0, buffer.Length);
            responseData = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
            stream.Close();
            client.Close();
        }
        catch (Exception ex) {
            Debug.Log("Connection Failed: " + ex);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Client();
        }
    }
}
