using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using log4net;
using Monei.DataAccessLayer.Interfaces;
//using Monei.DataAccessLayer.MongoDB;
using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;

namespace Monei.MvcApplication.Helpers
{
	public class WebSecurity :WebMatrix.WebData.ExtendedMembershipProvider
	{
		public static readonly ILog logger = LogManager.GetLogger(typeof(WebSecurity));
		private readonly IAccountRepository accountRepository;
				
		public enum LoginResult
		{
			Ok, UsernameNotFound, WrongPassword
		}

		public WebSecurity(IAccountRepository accountRepository)
		{
			this.accountRepository = accountRepository;
		}		

		public LoginResult Login(string username, string password, bool persistCookie = false)
		{
			//todo register last login in database
			Account account = accountRepository.Read(username);

			if (account == null)
			{
				logger.InfoFormat("Login failed, username not found (\"{0}\").", username);
				return LoginResult.UsernameNotFound;
			}

			if (account.Password != password)
			{
				logger.InfoFormat("Login failed, wrong password (\"{0}\").", password);
				return LoginResult.WrongPassword;
			}

			account.LastLogin = DateTime.UtcNow;
			accountRepository.UpdateLastLogin(account.Id, DateTime.UtcNow);

			logger.InfoFormat("Login success. username: \"{0}\".", username);

			// store authentication cookie
			FormsAuthentication.SetAuthCookie(username, persistCookie);
			return LoginResult.Ok;
		}

		


		public static int GetUserId(string username)
		{
			//todo

			return 0;
		}


		public static void Logout()
		{ 
			// todo: register action in database
			FormsAuthentication.SignOut();
		}



		public override bool ConfirmAccount(string accountConfirmationToken)
		{
			throw new NotImplementedException();
		}

		public override bool ConfirmAccount(string username, string accountConfirmationToken)
		{
			throw new NotImplementedException();
		}

		public override string CreateAccount(string username, string password, bool requireConfirmationToken)
		{
			throw new NotImplementedException();
		}

		public override string CreateUserAndAccount(string username, string password, bool requireConfirmation, IDictionary<string, object> values)
		{
			// todo: get currency from user selection
			Account.AccountRole role = Account.AccountRole.User;
			Currency currency = new CurrencyRepository().Read(Currency.EUR_CODE);
			Account account = accountRepository.Create(username, password, role, currency);

			return account.Id.ToString();
		}

		public override bool DeleteAccount(string username)
		{
			throw new NotImplementedException();
		}

		public override string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow)
		{
			throw new NotImplementedException();
		}

		public override ICollection<WebMatrix.WebData.OAuthAccountData> GetAccountsForUser(string username)
		{
			throw new NotImplementedException();
		}

		public override DateTime GetCreateDate(string username)
		{
			throw new NotImplementedException();
		}

		public override DateTime GetLastPasswordFailureDate(string username)
		{
			throw new NotImplementedException();
		}

		public override DateTime GetPasswordChangedDate(string username)
		{
			throw new NotImplementedException();
		}

		public override int GetPasswordFailuresSinceLastSuccess(string username)
		{
			throw new NotImplementedException();
		}

		public override int GetUserIdFromPasswordResetToken(string token)
		{
			throw new NotImplementedException();
		}

		public override bool IsConfirmed(string username)
		{
			throw new NotImplementedException();
		}

		public override bool ResetPasswordWithToken(string token, string newPassword)
		{
			throw new NotImplementedException();
		}

		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			throw new NotImplementedException();
		}

		public override bool EnablePasswordReset
		{
			get { throw new NotImplementedException(); }
		}

		public override bool EnablePasswordRetrieval
		{
			get { throw new NotImplementedException(); }
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new NotImplementedException();
		}

		public override string GetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			throw new NotImplementedException();
		}

		public override string GetUserNameByEmail(string email)
		{
			throw new NotImplementedException();
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { throw new NotImplementedException(); }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { throw new NotImplementedException(); }
		}

		public override int MinRequiredPasswordLength
		{
			get { throw new NotImplementedException(); }
		}

		public override int PasswordAttemptWindow
		{
			get { throw new NotImplementedException(); }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { throw new NotImplementedException(); }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { throw new NotImplementedException(); }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { throw new NotImplementedException(); }
		}

		public override bool RequiresUniqueEmail
		{
			get { throw new NotImplementedException(); }
		}

		public override string ResetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

		public override bool UnlockUser(string username)
		{
			throw new NotImplementedException();
		}

		public override void UpdateUser(MembershipUser user)
		{
			throw new NotImplementedException();
		}

		public override bool ValidateUser(string username, string password)
		{
			throw new NotImplementedException();
		}


		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			throw new NotImplementedException();
		}
	}//class
}