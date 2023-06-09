﻿using System.Net.Mail;
using Gses.Services.Mail.ServiceLayer;
using Gses.Services.Rate.ServiceLayer;
using Gses.Services.Subscription.DAL;
using Gses.Services.Subscription.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gses.Services.Subscription.ServiceLayer
{
	public interface ISubscriptionService
	{
		bool Subscribe(string email, ModelStateDictionary modelState);

		Task NotifySubscribersAsync(ModelStateDictionary modelState);
	}

	public class SubscriptionService : ISubscriptionService
	{
		private readonly ISubscriptionEmailFileRepository _subscriptionEmailFileRepository;
		private readonly IRateService _rateService;
		private readonly IMailService _mailService;

		public SubscriptionService(
			ISubscriptionEmailFileRepository subscriptionEmailFileRepository,
			IRateService rateService,
			IMailService mailService)
		{
			_subscriptionEmailFileRepository = subscriptionEmailFileRepository;
			_rateService = rateService;
			_mailService = mailService;
		}

		public bool Subscribe(string email, ModelStateDictionary modelState)
		{
			if (!validateEmail(email))
			{
				modelState.AddModelError("Error", "Invalid email address");
				return false;
			}

			return _subscriptionEmailFileRepository.Add(email);
		}

		public async Task NotifySubscribersAsync(ModelStateDictionary modelState)
		{
			var emails = _subscriptionEmailFileRepository.GetAll();
			if (emails.Length == default)
			{
				return;
			}

			var rate = await _rateService.GetBtcToUahActualRateAsync(modelState);
			if (!modelState.IsValid)
			{
				return;
			}

			var templateModel = new BtcToUahRateMailTemplateModel(rate.Value);
			await _mailService.NotifyAsync("BtcToUahRate", templateModel, emails, modelState);
		}

		private bool validateEmail(string email)
		{
			try
			{
				// ReSharper disable once ObjectCreationAsStatement
				new MailAddress(email);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
	}
}
