using MassTransit;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using ModelLayer;

namespace CollabProducer
{
    public class CollabConsumer: IConsumer<CollabModel>
    {
       
        public async Task Consume(ConsumeContext<CollabModel> context)
        {
            var data = context.Message;
            string subject = "Fundoo Notes Application";
            string body = $"Hi ,\nYou have been added as a collabortator to a Note by {data.email}.";
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("fundoosupp@gmail.com", "wehsitaysxdbppzl"),
                EnableSsl = true,
            };
            smtp.Send("fundoosupp@gmail.com", data.collabemail, subject, body);
        }
    }
}
