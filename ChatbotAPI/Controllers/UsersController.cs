using AutoMapper;
using ChatbotAPI.Helpers;
using ChatbotAPI.Models;
using ChatbotAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthorizeAttribute = ChatbotAPI.Helpers.AuthorizeAttribute;

namespace ChatbotAPI.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings) {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model) {
            var user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
           
            // return basic user info and authentication token
            return Ok(new {
                Id = user.Id,
                Role = user.Role,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [Authorize(Role = "Admin")]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model) {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex) {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Role = "Admin")]
        [HttpGet]
        public IActionResult GetAll() {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [Authorize(Role = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModel model) {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex) {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Role = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            _userService.Delete(id);
            return Ok();
        }
    }

    public class AuthenticateModel {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RegisterModel {
        [Required]
        public string Role { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UpdateModel {
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserModel {
        public int Id { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public static class Role {
        public const string Admin = "Admin";
        public const string Staff = "Staff";
    }
}
