using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
//TODO: Cleanup

public class TCPConnection
{
    private TcpClient Socket;
    private Thread clientReceiveThread;
    public Event Events = new Event();

    public TCPConnection(Host host)
    {
        //TODO: connect and errors events
        this.Socket = new TcpClient(host.ip, host.port);
        clientReceiveThread = new Thread(() => ListenForData(host));
        clientReceiveThread.IsBackground = true;
        clientReceiveThread.Start();
    }

    private void ListenForData(Host host)
    {
        try
        {
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = this.Socket.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.UTF8.GetString(incommingData);
                        //Debug.Log("server message received as: " + serverMessage);
                        this.Events.emit("data", serverMessage as object);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException); //TODO: error handling
        }
    }
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    public void write(String msg)
    {
        if (this.Socket == null)
            return;

        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = this.Socket.GetStream();
            if (stream.CanWrite)
            {
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(msg);
                // Write byte array to this.Socket stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                //Debug.Log("Client sent his message - " + msg + " - should be received by server");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
