using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newsletter.API.Messages;

namespace Newsletter.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NewslettersController : ControllerBase
	{
		private readonly IBus _bus;

		public NewslettersController(IBus bus)
		{
			_bus = bus;
		}

		[HttpPost]
		public IActionResult SubscribeToNewsletter([FromBody] string email)
		{
			_bus.Publish(new SubscribeToNewsletter(email));

			return Accepted();
		}
	}
}
