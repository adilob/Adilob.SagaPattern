using MassTransit;
using Newsletter.API.Emails;
using Newsletter.API.Messages;

namespace Newsletter.API.Handlers;

public class SendWelcomeEmailHandler(IEmailService emailService) : IConsumer<SendWelcomeEmail>
{
	public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
	{
		await emailService.SendWelcomeEmailAsync(context.Message.Email);

		await context.Publish(new WelcomeEmailSent
		{
			SubscriberId = context.Message.SubscriberId,
			Email = context.Message.Email
		});
	}
}
