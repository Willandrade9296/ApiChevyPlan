namespace ApiDatalife.Config.DBContext
{
    public class DbContextMiddleware
    {
        private readonly RequestDelegate _next;

        public DbContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext httpContext,
            IDbContextFactory dbContextFactory,
            IUserConnectionManager userConnectionManager)
        {
            
            var username = httpContext.User.Identity?.Name;

            if (username != null)
            {
                var connectionString = userConnectionManager.GetUserConnection(username);
                if (connectionString != null)
                {
                    var context = dbContextFactory.CreateDbContext(connectionString);
                    httpContext.Items["DynamicDbContext"] = context;
                }
            }

            await _next(httpContext);

            // Limpiar el contexto si existe
            if (httpContext.Items.TryGetValue("DynamicDbContext", out var dbContextObj) &&
                dbContextObj is DynamicDbContext dbContext)
            {
                await dbContext.DisposeAsync();
            }
        }
    }
}
