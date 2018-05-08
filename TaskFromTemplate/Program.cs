using System;
using TaskFromTemplate.Classes;
using TaskFromTemplate.Controller;

namespace TaskFromTemplate
{
	public class Program
	{
	    public static void Main(string[] args)
	    {
	        IvantiRestController controller =
	            new IvantiRestController(
	                new RestAccessProperties
	                {
	                    CoreServerName = "EPMCore2018",         // enter your CoreServerName
	                    ClientId = "ResourceOnly",              // fill from EPM Core: C:\ProgramData\LANDesk\ServiceDesk\My.IdentityServer\IdentityServer3.Core.Models.Client.json
	                    ClientSecret = "Interchange",           // fill from EPM Core: C:\ProgramData\LANDesk\ServiceDesk\My.IdentityServer\IdentityServer3.Core.Models.Client.json
                        Username = "domain\\user",              // authorized user
	                    Password = "Interchange2018$"           // password of user
	                }, ignoreServerCertificateValidation: true  // if the EPM Core doesn't have a valid ssl certificate, set ignoreHttpsErrors to true
                );

	        var errorMessage = string.Empty;
	        var successful = controller.CreateTaskFromTemplate("Run", "New sample task", "1087", ref errorMessage);

	        var result = successful ? "was successful" : $"failed: {errorMessage}";
	        Console.WriteLine($"Create task {result}");
	    }
	}
}
