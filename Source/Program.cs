using progjog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentity();
builder.Services.AddTrainingSessionService();

builder.Services.AddRazorPages(options => 
{
    options.Conventions.AuthorizePage("/Dashboard");
    options.Conventions.AuthorizePage("/Calendar");
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// map controller options
// app.MapControllerRoute(
//    name: "default",
//

app.MapControllers();

app.Run();
