﻿namespace FCG.API.Infraestructure.Options
{
    public class JwtOptions
    {
        public string? Key { get; set; }
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
    }
}
