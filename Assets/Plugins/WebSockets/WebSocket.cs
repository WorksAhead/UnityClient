using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using WebSocket4Net;
using ArkCrossEngine.Network;

#if UNITY_WEBGL

public class WebGLSocket : WebSocketWrapper.WebSocketBase
{
	private Uri mUri;
    
    public WebGLSocket()
	{
#if UNITY_WEBGL && !UNITY_EDITOR
        m_NativeRef = SocketCreate("ws://10.1.9.84:9001");
#endif
    }

	public override void Send(string str)
	{
        byte[] text = Encoding.UTF8.GetBytes(str);
        Send(text);
	}

    public void Tick()
    {
#if UNITY_WEBGL && !UNITY_EDITOR

        if (m_NativeRef == -1)
        {
            return;
        }

        int stat = SocketState(m_NativeRef);
        if (stat != 1)
        {
            Debug.LogError("Socket State=" + stat);
            return;
        }

        Recv();
#endif
    }
    
#if !UNITY_EDITOR
    [DllImport("__Internal")]
	private static extern int SocketCreate (string url);

	[DllImport("__Internal")]
	private static extern int SocketState (int socketInstance);

	[DllImport("__Internal")]
	private static extern void SocketSend (int socketInstance, byte[] ptr, int length);

	[DllImport("__Internal")]
	private static extern void SocketRecv (int socketInstance, byte[] ptr, int length);

	[DllImport("__Internal")]
	private static extern int SocketRecvLength (int socketInstance);

	[DllImport("__Internal")]
	private static extern void SocketClose (int socketInstance);

	[DllImport("__Internal")]
	private static extern int SocketError (int socketInstance, byte[] ptr, int length);

	int m_NativeRef = -1;

    public override void Send(byte[] buffer)
	{
		SocketSend (m_NativeRef, buffer, buffer.Length);
	}

	private void Recv()
	{
        //Debug.LogError("Trying Recv Msg.");
		int length = SocketRecvLength (m_NativeRef);
        while (length > 0)
        {
            byte[] buffer = new byte[length];
            SocketRecv(m_NativeRef, buffer, length);

            //Debug.LogError("msg recved: " + buffer);

            MessageReceivedEventArgs args = new MessageReceivedEventArgs(Encoding.UTF8.GetString(buffer));
            if (WebSocketWrapper.Instance.MessageReceived != null)
            {
                WebSocketWrapper.Instance.MessageReceived(this, args);
                //Debug.LogError("msg recv: "+args.Message);
            }

            length = SocketRecvLength(m_NativeRef);
        }
	}

	public override void Open()
	{
        //mUri = new Uri(mUrl);
        //m_NativeRef = SocketCreate (mUrl);

		if (error != null)
        {
            throw new Exception("fail to create websocket with error: " + error);
        }

        WebSocketWrapper.Instance.IsSocketConnected = true;
        if (WebSocketWrapper.Instance.Opened != null)
        {
            WebSocketWrapper.Instance.State = WebSocketState.Open;
            WebSocketWrapper.Instance.Opened(this, null);
        }
    }
 
	public override void Close()
	{
		SocketClose(m_NativeRef);
        WebSocketWrapper.Instance.State = WebSocketState.Closed;
    }

	public string error
	{
		get {
			const int bufsize = 1024;
			byte[] buffer = new byte[bufsize];
			int result = SocketError (m_NativeRef, buffer, bufsize);

			if (result == 0)
				return null;

			return Encoding.UTF8.GetString (buffer);				
		}
	}
#else
    WebSocketSharp.WebSocket m_Socket;
	Queue<byte[]> m_Messages = new Queue<byte[]>();
	
	string m_Error = null;

	public override void Open()
	{
        mUri = new Uri(mUrl);
		m_Socket = new WebSocketSharp.WebSocket(mUri.ToString());

        m_Socket.OnMessage += (sender, e) =>
        {
            //m_Messages.Enqueue(e.RawData);
            MessageReceivedEventArgs args = new MessageReceivedEventArgs(e.Data);
            if (WebSocketWrapper.Instance.MessageReceived != null)
            {
                WebSocketWrapper.Instance.MessageReceived(sender, args);
            }
        };

        m_Socket.OnOpen += (sender, e) =>
        {
            WebSocketWrapper.Instance.IsSocketConnected = true;
            if (WebSocketWrapper.Instance.Opened != null)
            {
                WebSocketWrapper.Instance.State = WebSocketState.Open;
                WebSocketWrapper.Instance.Opened(sender, e);
            }
        };

        m_Socket.OnError += (sender, e) =>
        {
            m_Error = e.Message;
            if (WebSocketWrapper.Instance.Error != null)
            {
                SuperSocket.ClientEngine.ErrorEventArgs err = new SuperSocket.ClientEngine.ErrorEventArgs(new Exception(e.Message));
                WebSocketWrapper.Instance.Error(sender, err);
            }
        };

        m_Socket.OnClose += (sender, e) =>
        {
            if (WebSocketWrapper.Instance.Closed != null)
            {
                WebSocketWrapper.Instance.Closed(sender, e);
            }
        };

        m_Socket.Connect();
		//while (!IsSocketConnected && m_Error == null)
			//yield return 0;
	}

	public override void Send(byte[] buffer)
	{
        m_Socket.Send(buffer);
	}

	public override void Close()
	{
		m_Socket.Close();
	}
#endif
}
#endif