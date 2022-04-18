using WebApp1;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapGet("/api/users", () =>
        {
            using ApplicationContext db = new ApplicationContext();
            return Results.Json(db.Users.ToList());
        });

        app.MapGet("/api/users/{id}", (int id) =>
        {
            using ApplicationContext db = new ApplicationContext();
            // получаем пользователя по id
            Person? user = db.Users.FirstOrDefault(u => u.Id == id);
            // если не найден, отправляем статусный код и сообщение об ошибке
            if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
            // если пользователь найден, отправляем его
            return Results.Json(user);
        });

        app.MapDelete("/api/users/{id}", (int id) =>
        {
            using ApplicationContext db = new ApplicationContext();
            // получаем пользователя по id
            Person? user = db.Users.FirstOrDefault(u => u.Id == id);
            // если не найден, отправляем статусный код и сообщение об ошибке
            if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
            // если пользователь найден, удаляем его
            db.Users.Remove(user);
            db.SaveChanges();
            return Results.Json(user);
        });

        app.MapPost("/api/users", (Person user) =>
        {
            using ApplicationContext db = new ApplicationContext();
            // добавляем пользователя в список
            user.Id = 0;
            var u = db.Users.Add(user);
            db.SaveChanges();
            return Results.Json(u.Entity);
        });

        app.MapPut("/api/users", (Person userData) =>
        {
            using ApplicationContext db = new ApplicationContext();
            // ищем пользователя по id
            var user = db.Users.FirstOrDefault(u => u.Id == userData.Id);
            // если пользователь не найден по id, то ищем по Name и Age
            if (user == null) user = db.Users.FirstOrDefault(u => (u.Name == userData.Name) && (u.Age == userData.Age));
            // если пользователь не найден по id и Name+Age, отправляем статусный код и сообщение об ошибке
            if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
                
            // если пользователь найден, изменяем его данные и отправляем обратно клиенту
            user.Name = userData.Name;
            user.Age = userData.Age;
            db.Users.Update(user);
            db.SaveChanges();
                
            return Results.Json(user);
        });

        app.Run();
    }
}
