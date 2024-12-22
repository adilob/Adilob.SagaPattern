using MassTransit;
using Newsletter.API.Database;
using Newsletter.API.Messages;

namespace Newsletter.API.Handlers;

public class SubscribeToNewsletterHandler(AppDbContext dbContext) : IConsumer<SubscribeToNewsletter>
{
	public async Task Consume(ConsumeContext<SubscribeToNewsletter> context)
	{
		var subscriber = dbContext.Subscribers.Add(new Subscriber
		{
			Id = Guid.NewGuid(),
			Email = context.Message.Email,
			SubscribedOnUtc = DateTime.UtcNow
		});

		await dbContext.SaveChangesAsync();

		await context.Publish(new SubscriberCreated
		{
			SubscriberId = subscriber.Entity.Id,
			Email = context.Message.Email
		});
	}
}
