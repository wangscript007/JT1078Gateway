﻿using JT1078.Gateway.Metadata;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT1078.Gateway.Extensions
{
    public static class JT1078HttpContextExtensions
    {
        public static async ValueTask Http401(this HttpListenerContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes("auth error");
            context.Response.AddHeader("Access-Control-Allow-Headers", "*");
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.KeepAlive = false;
            context.Response.ContentLength64 = b.Length;
            var output = context.Response.OutputStream;
            await output.WriteAsync(b, 0, b.Length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static async ValueTask Http400(this HttpListenerContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes($"sim and channel parameter required.");
            context.Response.AddHeader("Access-Control-Allow-Headers", "*");
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.KeepAlive = false;
            context.Response.ContentLength64 = b.Length;
            var output = context.Response.OutputStream;
            await output.WriteAsync(b, 0, b.Length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static void Http404(this HttpListenerContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Headers", "*");
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.KeepAlive = false;
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static async ValueTask Http500(this HttpListenerContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes("inner error");
            context.Response.AddHeader("Access-Control-Allow-Headers", "*");
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.KeepAlive = false;
            context.Response.ContentLength64 = b.Length;
            var output = context.Response.OutputStream;
            await output.WriteAsync(b, 0, b.Length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static async ValueTask HttpSendFirstChunked(this JT1078HttpContext context, ReadOnlyMemory<byte> buffer)
        {
            context.Context.Response.AddHeader("Access-Control-Allow-Headers", "*");
            context.Context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Context.Response.SendChunked = true;
            await context.Context.Response.OutputStream.WriteAsync(buffer);
        }

        public static async ValueTask HttpSendChunked(this JT1078HttpContext context, ReadOnlyMemory<byte> buffer)
        {
            context.Context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Context.Response.OutputStream.WriteAsync(buffer);
        }

        public static async ValueTask HttpClose(this JT1078HttpContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes("close");
            context.Context.Response.AddHeader("Access-Control-Allow-Headers", "*");
            context.Context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Context.Response.KeepAlive = false;
            context.Context.Response.ContentLength64 = b.Length;
            var output = context.Context.Response.OutputStream;
            await output.WriteAsync(b, 0, b.Length);
            context.Context.Response.OutputStream.Close();
            context.Context.Response.Close();
        }

        public static async ValueTask WebSocketClose(this JT1078HttpContext context,string content)
        {
           await context.WebSocketContext.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, content, CancellationToken.None);
        }

        public static async ValueTask WebSocketSendTextAsync(this JT1078HttpContext context, ReadOnlyMemory<byte> buffer)
        {
            await context.WebSocketContext.WebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public static async ValueTask WebSocketSendBinaryAsync(this JT1078HttpContext context, ReadOnlyMemory<byte> buffer)
        {
            await context.WebSocketContext.WebSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        private static ReadOnlyMemory<byte> Hello = Encoding.UTF8.GetBytes("hello,jt1078");

        public static async ValueTask WebSocketSendHelloAsync(this JT1078HttpContext context)
        {
            await context.WebSocketContext.WebSocket.SendAsync(Hello, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
