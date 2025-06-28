using Microsoft.AspNetCore.Mvc;

namespace MainProject.Controllers
{
    public class ConfigController :  ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("")]
        public ActionResult GetConfig()
        {
            var config = new
            {
                // launchSettings overrides appSettings and appSettingEnvironment
                // appSettingEnvironment overrides appSettings 
                EnvName = _configuration["ASPNETCORE_ENVIRONMENT"],
                AllowedHosts = _configuration["AllowedHosts"],
                //DefaultConnection = _configuration["ConnectionStrings:DefaultConnection"],
                DefaultConnection = _configuration.GetConnectionString("DefaultConnection"),
                DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
                TestKey = _configuration["TestKey"],
                SigningKey = _configuration["SigningKey"],
            };
            return Ok(config);
        }
    }
}
