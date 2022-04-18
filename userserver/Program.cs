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
            // �������� ������������ �� id
            Person? user = db.Users.FirstOrDefault(u => u.Id == id);
            // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
            if (user == null) return Results.NotFound(new { message = "������������ �� ������" });
            // ���� ������������ ������, ���������� ���
            return Results.Json(user);
        });

        app.MapDelete("/api/users/{id}", (int id) =>
        {
            using ApplicationContext db = new ApplicationContext();
            // �������� ������������ �� id
            Person? user = db.Users.FirstOrDefault(u => u.Id == id);
            // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
            if (user == null) return Results.NotFound(new { message = "������������ �� ������" });
            // ���� ������������ ������, ������� ���
            db.Users.Remove(user);
            db.SaveChanges();
            return Results.Json(user);
        });

        app.MapPost("/api/users", (Person user) =>
        {
            using ApplicationContext db = new ApplicationContext();
            // ��������� ������������ � ������
            user.Id = 0;
            var u = db.Users.Add(user);
            db.SaveChanges();
            return Results.Json(u.Entity);
        });

        app.MapPut("/api/users", (Person userData) =>
        {
            using ApplicationContext db = new ApplicationContext();
            // ���� ������������ �� id
            var user = db.Users.FirstOrDefault(u => u.Id == userData.Id);
            // ���� ������������ �� ������ �� id, �� ���� �� Name � Age
            if (user == null) user = db.Users.FirstOrDefault(u => (u.Name == userData.Name) && (u.Age == userData.Age));
            // ���� ������������ �� ������ �� id � Name+Age, ���������� ��������� ��� � ��������� �� ������
            if (user == null) return Results.NotFound(new { message = "������������ �� ������" });
                
            // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������
            user.Name = userData.Name;
            user.Age = userData.Age;
            db.Users.Update(user);
            db.SaveChanges();
                
            return Results.Json(user);
        });

        app.Run();
    }
}
