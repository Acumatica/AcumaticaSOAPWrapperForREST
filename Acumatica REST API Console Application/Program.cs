using System;

namespace AcumaticaSoapLikeApiExample
{
    class Program
	{
		const string SiteURL = "http://localhost/test/entity/Default/22.200.001/";
		const string Username = "admin";
		const string Password = "123";
		const string Tenant = null;
		const string Branch = null;
		const string Locale = null;

		static void Main(string[] args)
		{
            Console.WriteLine("SOAP-like example");
            SOAPLikeExample.ExampleMethod(SiteURL, Username, Password, Tenant, Branch, Locale);
            SOAPLikeExample2.ExampleMethod(SiteURL, Username, Password, Tenant, Branch, Locale);
			Console.ReadLine();
		}

	}
}
