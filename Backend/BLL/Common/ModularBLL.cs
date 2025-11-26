namespace BLL.Common
{
    public static class ModularBLL
    {
        public static IServiceCollection AddBuissinesInBLL(this IServiceCollection services)
        {
            // notification
            services.AddScoped<INotificationService, NotificationService>();
            // messages
            services.AddScoped<IMessageService, MessageService>();
            // identity
            services.AddScoped<IIdentityService, IdentityService>();
            // reviews
            services.AddScoped<IReviewService, ReviewService>();
            // admin
            services.AddScoped<IAdminService, AdminService>();

            // bookings & payments
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPaymentService, PaymentService>();

            // face recognition
            services.AddScoped<IFaceRecognitionService, FaceRecognitionService>();

            services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));
            // Token service
            services.AddSingleton<ITokenService, TokenService>();
            // Ensure IdentityService is registered with token service injected
            services.AddScoped<IIdentityService, IdentityService>();
            return services;
        }
    }
}