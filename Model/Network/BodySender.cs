﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Rug.Osc;

namespace KinectV2OSC.Model.Network
{
    public class BodySender
    {
        private OscSender oscSender;
        private MessageBuilder messageBuilder;
        private string ipAddress;
        private string port;

        public BodySender(string ipAddress, string port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.messageBuilder = new MessageBuilder();
            this.TryConnect();
        }

        private void TryConnect()
        {
            try
            {
                this.oscSender = new OscSender(IPAddress.Parse(this.ipAddress), int.Parse(this.port));
                this.oscSender.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception on OSC connection...");
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Send(Body[] bodies)
        {
            foreach (Body body in bodies)
            {
                if (body.IsTracked)
                {
                    Send(body);
                }
            }
        }

        private void Send(Body body)
        {
            foreach (var joint in body.Joints)
            {
                var message = messageBuilder.Build(body, joint);
                this.oscSender.Send(message);
            }
        }
    }
}