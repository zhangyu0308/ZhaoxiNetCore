using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreWebApi.Utility
{
    public interface IJWTService
    {
        string GetToken(string username);
    }
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _iConfiguration;
        public JWTService(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }


        public string GetToken(string username)
        {
            /**
             * claims Payload 有效载荷，包含了一些和token有关的信息，建议配置在 配置文件中
             * 这些字段是关键字，除了这些可以包含其他任何json可以兼容的字段
             * issuer：给谁的
             * sub:主题
             * iat：时间戳
             * jti: token唯一标识
             * audience:签发人
             */
            Claim[] claims = new[] {
                new Claim(ClaimTypes.Name,username),
                new Claim("name", "zy"),
                new Claim("age", "18"),
                new Claim("money","5000000") //其他字段信息
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iConfiguration["SecurityKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: _iConfiguration["issuer"],audience: _iConfiguration["audience"],claims: claims,expires:DateTime.Now.AddMinutes(5),signingCredentials: creds);
            string returntoken = new JwtSecurityTokenHandler().WriteToken(token);
            return returntoken;
        }
    }
}
