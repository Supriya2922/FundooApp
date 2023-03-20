﻿using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ModelLayer
{
    public class MSMQ
    {
        MessageQueue messageQueue = new MessageQueue();
        public void sendData2Queue(string token)
        {
            messageQueue.Path = @".\private$\FundooNotes";
            if(!MessageQueue.Exists(messageQueue.Path))
            {
                MessageQueue.Create(messageQueue.Path);
               
            }
          
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
            messageQueue.Send(token);
            messageQueue.BeginReceive();
            messageQueue.Close();
        }

        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQueue.EndReceive(e.AsyncResult);
                string token = msg.Body.ToString();
               
                string subject = "Fundoo Notes Reset Link";
                string body = $"Fundoo Notes Reset Password: <a href=http://localhost:44354/resetPassword/{token}> Click Here</a>";
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("fundoosupp@gmail.com", "wehsitaysxdbppzl"),
                    EnableSsl = true,
                };
                smtp.Send("fundoosupp@gmail.com", "fundoosupp@gmail.com", subject, body);
                messageQueue.BeginReceive();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
