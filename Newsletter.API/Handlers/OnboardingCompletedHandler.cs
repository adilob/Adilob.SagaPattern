using MassTransit;
using Newsletter.API.Messages;

namespace Newsletter.API.Handlers;

public class OnboardingCompletedHandler(ILogger<OnboardingCompleted> logger) : IConsumer<OnboardingCompleted>
{
	public Task Consume(ConsumeContext<OnboardingCompleted> context)
	{
		logger.LogInformation("Onboarding completed");
		return Task.CompletedTask;
	}
}
