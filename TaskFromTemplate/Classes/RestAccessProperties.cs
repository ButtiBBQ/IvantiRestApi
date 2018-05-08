namespace TaskFromTemplate.Classes
{
    public class RestAccessProperties
    {
        public string CoreServerName { get; set; }
        public string ClientId {get; set;}
        public string IdentityServer => $@"https://{CoreServerName}/my.identityserver/identity";
        public string DistributionApi => $@"https://{CoreServerName}/DistributionApi";
        public string ClientSecret  {get; set;}
        public string GrandType => "password";
        public string Scope => "openid";
        public string Username { get; set; }
        public string Password { get; set; }  
    }
}
