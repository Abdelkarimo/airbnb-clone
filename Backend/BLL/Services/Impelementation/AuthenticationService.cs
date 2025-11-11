namespace BLL.Services.Impelementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user with email and password
        public async Task<AuthResponseVM> RegisterAsync(RegisterVM registerVM)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerVM.Email);
            if (existingUser != null)
                throw new ArgumentException("User with this email already exists");

            // Map RegisterVM to User entity using AutoMapper
            var user = _mapper.Map<User>(registerVM);

            // Create user with password
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException($"User creation failed: {errors}");
            }

            // Generate JWT token and return response
            return await GenerateAuthResponse(user);
        }

        /// Authenticates user with email and password
        public async Task<AuthResponseVM> LoginAsync(LoginVM loginVM)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("Invalid credentials");

            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid credentials");

            // Generate JWT token and return response
            return await GenerateAuthResponse(user);
        }

        /// Authenticates user using Firebase social login
        public async Task<AuthResponseVM> FirebaseLoginAsync(FirebaseLoginVM firebaseLoginVM)
        {
            // Verify Firebase token
            FirebaseToken decodedToken = await VerifyFirebaseToken(firebaseLoginVM.FirebaseToken);

            // Extract user info from Firebase token
            var (email, name, picture, firebaseUid) = ExtractUserInfoFromToken(decodedToken);

            // Find or create user
            var user = await FindOrCreateUserFromFirebase(email, name, picture, firebaseUid);

            // Generate JWT token and return response
            return await GenerateAuthResponse(user);
        }

        /// Changes user password
        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordVM changePasswordVM)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(
                user,
                changePasswordVM.CurrentPassword,
                changePasswordVM.NewPassword);

            return result.Succeeded;
        }

        #region Private Methods

        /// Verifies Firebase authentication token
        private async Task<FirebaseToken> VerifyFirebaseToken(string firebaseToken)
        {
            try
            {
                return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseToken);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Invalid Firebase token");
            }
        }

        /// <summary>
        /// Extracts user information from Firebase token
        /// </summary>
        private (string email, string name, string picture, string firebaseUid) ExtractUserInfoFromToken(FirebaseToken decodedToken)
        {
            var claims = decodedToken.Claims;
            var email = claims.GetValueOrDefault("email")?.ToString();
            var name = claims.GetValueOrDefault("name")?.ToString() ?? "User";
            var picture = claims.GetValueOrDefault("picture")?.ToString();
            var firebaseUid = decodedToken.Uid;

            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email not found in Firebase token");

            return (email, name, picture, firebaseUid);
        }

        /// <summary>
        /// Finds existing user or creates new one from Firebase login
        /// </summary>
        private async Task<User> FindOrCreateUserFromFirebase(string email, string name, string picture, string firebaseUid)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid)
                      ?? await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // Create new user for Firebase authentication
                user = User.Create(fullName: name, profileImg: picture, firebaseUid: firebaseUid);
                user.Email = email;
                user.UserName = email;

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new ArgumentException($"User creation failed: {errors}");
                }
            }
            else
            {
                // Update existing user with Firebase info
                user.Update(fullName: name, profileImg: picture, firebaseUid: firebaseUid);
                await _userManager.UpdateAsync(user);
            }

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated");

            return user;
        }

        /// Generates authentication response with JWT token and user info
        private async Task<AuthResponseVM> GenerateAuthResponse(User user)
        {
            var token = _tokenService.GenerateJwtToken(user);
            var userVM = _mapper.Map<UserVM>(user);

            return new AuthResponseVM
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(7),
                User = userVM
            };
        }
        #endregion

    }
}
