using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace WebApplication3.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Legal> Legals { get; set; } = null!;
        public DbSet<Indiv> Individuals { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Subcon> Subcontractors { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Edit> Edits { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            Database.ExecuteSqlRaw(@"IF (SELECT COUNT(*) FROM [BDTesting].[sys].[objects] WHERE type = 'P' AND name = 'UpdateProjectEdits') = 1
DROP PROCEDURE [dbo].[UpdateProjectEdits]");
            Database.ExecuteSqlRaw(@"IF (SELECT COUNT(*) FROM [BDTesting].[sys].[objects] WHERE type = 'P' AND name = 'UpdateProjectMembers') = 1
DROP PROCEDURE [dbo].[UpdateProjectMembers]");
            Database.ExecuteSqlRaw(@"CREATE PROCEDURE [dbo].[UpdateProjectEdits] @projId tinyint, @editDesc text, @editDate nvarchar(20), @editReason nvarchar(50), @editBudget money AS
IF @editBudget = NULL OR @editBudget = 0
SET @editBudget = (SELECT [Бюджет] FROM [BDTesting].[dbo].[Projects] WHERE [Id] = @projId)
DECLARE @total nvarchar(max)
SET @total = CONCAT('>>Правка от ', @editDate, '|Причина: ', @editReason, '|Бюджет: ', CAST(@editBudget as nvarchar(max)), '|Текст правки: ', CAST(@editDesc AS nvarchar(max)), '||')
IF (SELECT COUNT(*) FROM [BDTesting].[dbo].[Edits] WHERE [Проект] = @projId) = 0
INSERT INTO [BDTesting].[dbo].[Edits]([Проект], [Время], [Правки проекта], [Изменение бюджета]) VALUES (@projId, CAST(@editDate AS DateTime2), @total, @editBudget)
ELSE
UPDATE [BDTesting].[dbo].[Edits] SET [Время] = @editDate, [Правки проекта] = CONCAT(@total, [Правки проекта]), [Причина] = NULL, [Описание] = NULL, [Изменение бюджета] = @editBudget WHERE [Проект] = @projId
UPDATE [BDTesting].[dbo].[Projects] SET [Наличие правок] = 1, [Бюджет] = @editBudget WHERE [Id] = @projId");
            Database.ExecuteSqlRaw(@"CREATE PROCEDURE [dbo].[UpdateProjectMembers] @projId tinyint, @clientId tinyint, @archId tinyint, @action nvarchar(10) AS
UPDATE [BDTesting].[dbo].[Clients] SET [Заказы] = CASE @action 
WHEN 'Добавить' THEN CONCAT([Заказы], ' ', @projId, ',') 
WHEN 'Удалить' THEN REPLACE([Заказы], CONCAT(' ', @projId, ','), '')
WHEN 'Изменить' THEN CONCAT(REPLACE([Заказы], CONCAT(' ', @projId, ','), ''), CONCAT(' ', @projId, ','))
END
WHERE [Id] = @clientId
UPDATE [BDTesting].[dbo].[Employees] SET [Проекты] = CASE @action
WHEN 'Добавить' THEN CONCAT([Проекты], ' ', @projId, ',')
WHEN 'Удалить' THEN REPLACE([Проекты], CONCAT(' ', @projId, ','), '')
WHEN 'Изменить' THEN CONCAT(REPLACE([Проекты], CONCAT(' ', @projId, ','), ''), CONCAT(' ', @projId, ','))
END
WHERE [Id] = @archId
IF @action = 'Изменить'
BEGIN
UPDATE [BDTesting].[dbo].[Clients] SET [Заказы] = REPLACE([Заказы], CONCAT(' ', @projId, ','), '') WHERE [Заказы] LIKE '% '+CAST(@projId AS nvarchar)+',%' AND [Id] != @clientId
UPDATE [BDTesting].[dbo].[Employees] SET [Проекты] = REPLACE([Проекты], CONCAT(' ', @projId, ','), '') WHERE [Проекты] LIKE '% '+CAST(@projId AS nvarchar)+',%'  AND [Id] != @archId
END");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasOne(p => p.Client).WithMany(c => c.Project).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Project>().HasOne(p => p.Employee).WithMany(e => e.Project).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Project>().HasOne(p => p.Subcon).WithMany(s => s.Project).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Project>().HasIndex(p => p.ClientId).IsUnique(false);
            modelBuilder.Entity<Project>().HasIndex(p => p.EmployeeId).IsUnique(false);
            modelBuilder.Entity<Project>().HasIndex(p => p.SubconId).IsUnique(false);

            modelBuilder.Entity<Indiv>().Property(i => i.Type).HasDefaultValue(false);
            modelBuilder.Entity<Project>().Property(i => i.IsEdit).HasDefaultValue(false);
            modelBuilder.Entity<Project>().Property(i => i.Gos).HasDefaultValue(false);
            modelBuilder.Entity<Edit>().Property(e => e.EditDate).HasDefaultValue(DateTime.Now);
            

            modelBuilder.Entity<Indiv>().HasData(new Indiv[]
            {
                new Indiv { Id=1, Type=false, Surname="Комарова", Name="Дарья", Midname="Матвеевна", Phone="+79361119108", INN="841306324259" },
                new Indiv { Id=2, Type=true, Surname="Иванов", Name="Давид", Midname="Макарович", Phone="+79959976836", INN="349736092272" },
                new Indiv { Id=3, Type=false, Surname="Новикова", Name="Екатерина", Midname="", Phone="+79018219057", INN="720149475900" },
                new Indiv { Id=4, Type=false, Surname="Жуков", Name="Тимур", Midname="Михайлович", Phone="+79956056540", INN="224066630513" },
                new Indiv { Id=5, Type=true, Surname="Гришин", Name="Алексей", Midname="", Phone="+77631092568", INN="191721860156" },
                new Indiv { Id=6, Type=true, Surname="Шилов", Name="Максим", Midname="Алексеевич", Phone="+79012235594", INN="429371690440" },
                new Indiv { Id=7, Type=false, Surname="Воронина", Name="Анна", Midname="Дмитриевна", Phone="+77183154661", INN="175328070414" },
                new Indiv { Id=8, Type=false, Surname="Филатов", Name="Дмитрий", Midname="", Phone="+77144480723", INN="250583571378" },
            });
            modelBuilder.Entity<Legal>().HasData(new Legal[]
            {
                new Legal { Id=1, Type=false, Name="ПАО Директор", ConPers="Егорова Мария Данииловна", Phone="+79043812091", INN="2853998716", OGRN="9125917834517" },
                new Legal { Id=2, Type=true, Name="НКО Рекордный врач", ConPers="Парфенов Вячеслав Ильич", Phone="+79361031691", INN="3012849997", OGRN="8041952900454" },
                new Legal { Id=3, Type=false, Name="ООО Революционный вопрос", ConPers="Губанов Юрий Романович", Phone="+79016312862", INN="0827884300", OGRN="3143928966343" },
                new Legal { Id=4, Type=false, Name="ПАО Жаркое слово", ConPers="Савин Кирилл Богданович", Phone="+77614348861", INN="0987330290", OGRN="4117546345330" },
                new Legal { Id=5, Type=false, Name="АО Немаловажная цена", ConPers="Наумов Владислав Сергеевич", Phone="+79920965326", INN="0368228108", OGRN="5134565430710" },
                new Legal { Id=6, Type=false, Name="ООО Качество", ConPers="Соколова Арина Сергеевна", Phone="+79002174621", INN="5924712961", OGRN="8058686402063" },
                new Legal { Id=7, Type=true, Name="НКО Сердце Вместе", ConPers="Захарова Агата Львовна", Phone="+77163413023", INN="0709954448", OGRN="8140950723015" },
                new Legal { Id=8, Type=false, Name="ООО Экстраординарная игра", ConPers="Николаева Вероника Дмитриевна", Phone="+78359894634", INN="9132652351", OGRN="7160769932646" },
            });
            modelBuilder.Entity<Client>().HasData(new Client[]
            {
                new Client { Id=1, LegalId=4, UID="LEG-004", Orders=" 1," },
                new Client { Id=2, LegalId=5, UID="LEG-005", Orders=" 2, 7," },
                new Client { Id=3, LegalId=1, UID="LEG-001", Orders="" },
                new Client { Id=4, LegalId=8, UID="LEG-008", Orders="" },
                new Client { Id=5, IndivId=1, UID="IND-001", Orders=" 4," },
                new Client { Id=6, IndivId=4, UID="IND-004", Orders="" },
                new Client { Id=7, LegalId=2, UID="LEG-002", Orders=" 3, 6," },
                new Client { Id=8, LegalId=3, UID="LEG-003", Orders=" 5, 8," },
            });
            modelBuilder.Entity<Employee>().HasData(new Employee[]
            {
                new Employee { Id=1, IndivId=2, UID="IND-2", Area="Промышленная арх.", Projects=" 5,", Salary=46602, Vac=false },
                new Employee { Id=2, IndivId=6, UID="IND-6", Area="Градостроительство", Projects=" 7,", Salary=195128, Vac=false },
                new Employee { Id=3, IndivId=7, UID="IND-7", Area="Ландшафт", Projects=" 3,", Salary=94765, Vac=true, VacStart=DateTime.Parse("2024-03-17").Date, VacEnd=DateTime.Parse("2024-05-01").Date },
                new Employee { Id=4, IndivId=8, UID="IND-8", Area="Экстерьер", Projects=" 4,", Salary=59529, Vac=false },
                new Employee { Id=5, IndivId=5, UID="IND-5", Area="Коммерческая и офисная арх.", Projects=" 1, 2,", Salary=125487, Vac=false },
                new Employee { Id=6, IndivId=4, UID="IND-4", Area="Интерьер", Projects="", Salary=65528, Vac=true, VacStart=DateTime.Parse("2024-02-29").Date, VacEnd=DateTime.Parse("2024-03-31").Date },
                new Employee { Id=7, IndivId=3, UID="IND-3", Area="Общественная арх.", Projects=" 8,", Salary=99510, Vac=false },
                new Employee { Id=8, IndivId=1, UID="IND-1", Area="Реставрация, реконструкция", Projects=" 6,", Salary=133785, Vac=false },
            });
            modelBuilder.Entity<Subcon>().HasData(new Subcon[]
            {
                new Subcon { Id=1, IndivId=6, UID="IND-006", Area="Строительно-монтажные работы", Zone="Ульяновск", Contract=0819 },
                new Subcon { Id=2, IndivId=5, UID="IND-005", Area="Строительный контроль", Zone="Димитровград", Contract=0506 },
                new Subcon { Id=3, LegalId=4, UID="LEG-004", Area="Гео. разведка", Zone="Карсунский", Contract=0180 },
                new Subcon { Id=4, LegalId=5, UID="LEG-005", Area="Строительный контроль", Zone="Димитровград", Contract=0747 },
                new Subcon { Id=5, IndivId=2, UID="IND-002", Area="Строительно-монтажные работы", Zone="Новоульяновск", Contract=0364 },
                new Subcon { Id=6, IndivId=4, UID="IND-004", Area="Гео. разведка", Zone="Ульяновск", Contract=0931 },
                new Subcon { Id=7, LegalId=8, UID="LEG-008", Area="Строительно-монтажные работы", Zone="Чердаклинский", Contract=0990 },
                new Subcon { Id=8, LegalId=3, UID="LEG-003", Area="Гео. разведка", Zone="Ульяновск", Contract=0980 },
            });

            modelBuilder.Entity<Project>().HasData(new Project[]
            {
                new Project { Id=1, Type="Бутик-отель", Address="пос. Новоспасское, ул. Центральная, д. 5а", StartDate=new DateTime(2023, 11, 3).Date, Budget=983146, ClientId=1, ClientUID="CLT-001", EmployeeId=5, ArchUID="EMP-005", SubconId=7, SubcUID="SUB-007", Gos=false },
                new Project { Id=2, Type="Павильон", Address="г. Димитровград, ул. Советская, д. 20", StartDate=new DateTime(2023, 11, 10).Date, Budget=351450, ClientId=2, ClientUID="CLT-002", EmployeeId=5, ArchUID="EMP-005", SubconId=3, SubcUID="SUB-003", Gos=false },
                new Project { Id=3, Type="Тематический парк", Address="г. Новоульяновск, ул. Комсомольская", StartDate=new DateTime(2023, 12, 17).Date, Budget=1484628, ClientId=7, ClientUID="CLT-007", EmployeeId=3, ArchUID="EMP-003", SubconId=5, SubcUID="SUB-005", Gos=true },
                new Project { Id=4, Type="Декорации", Address="с. Богдашкино, ул. Полевая, д. 37", StartDate=new DateTime(2023, 12, 22).Date, Budget=5433788, ClientId=5, ClientUID="CLT-005", EmployeeId=4, ArchUID="EMP-004", SubconId=2, SubcUID="SUB-002", Gos=false },
                new Project { Id=5, Type="Тематический парк", Address="г. Димитровград, ул. Свирская", StartDate=new DateTime(2023, 12, 25).Date, Budget=1262119, ClientId=8, ClientUID="CLT-008", EmployeeId=1, ArchUID="EMP-001", SubconId=7, SubcUID="SUB-007", Gos=false },
                new Project { Id=6, Type="Торговый центр", Address="г. Ульяновск, ул. Ленина, д. 50", StartDate=new DateTime(2023, 12, 25).Date, Budget=7528185, ClientId=7, ClientUID="CLT-007", EmployeeId=8, ArchUID="EMP-008", SubconId=5, SubcUID="SUB-005", Gos=false },
                new Project { Id=7, Type="Жилой дом многоквартирный", Address="пос. Барыш, ул. Победы, д. 10", StartDate=new DateTime(2024, 1, 27).Date, Budget=2142735, ClientId=2, ClientUID="CLT-002", EmployeeId=2, ArchUID="EMP-002", SubconId=2, SubcUID="SUB-002", Gos=false },
                new Project { Id=8, Type="Торговый центр", Address="г. Ульяновск, ул. Димитрова, д. 7/13", StartDate=new DateTime(2024, 2, 29).Date, Budget=2725630, ClientId=8, ClientUID="CLT-008", EmployeeId=7, ArchUID="EMP-007", SubconId=7, SubcUID="SUB-007", Gos=false },
            });
            modelBuilder.Entity<Edit>().HasData(new Edit[]
            {
                new Edit { Id=1, EditDate=new DateTime(2024, 1, 4, 6, 56, 0), Total=">>Правка от 2024-01-04T06:56:00|Причина: Просьба клиента|Бюджет: 983146.00|Текст правки: Изменить список фурнитуры||" },
                new Edit { Id=3, EditDate=new DateTime(2023, 12, 29, 12, 28, 0), Total=">>Правка от 2023-12-29T12:28:00|Причина: Просьба клиента|Бюджет: 1484628.00|Текст правки: Поменять зоны 2 и 7 местами||" },
                new Edit { Id=4, EditDate=new DateTime(2024, 3, 10, 11, 58, 0), Total=">>Правка от 2024-03-10T11:58:00|Причина: Просьба клиента|Бюджет: 5433788.00|Текст правки: Убрать из проекта помещение кухни||" },
                new Edit { Id=5, EditDate=new DateTime(2024, 1, 29, 7, 36, 0), Total=">>Правка от 2024-01-29T07:36:00|Причина: Просьба клиента|Бюджет: 1262119.00|Текст правки: Расширить пространство для коммерческих сооружений на 150м2||" },
                new Edit { Id=6, EditDate=new DateTime(2024, 3, 20, 9, 40, 0), Total=">>Правка от 2024-03-20T09:40:00|Причина: Просьба клиента|Бюджет: 7528185.00|Текст правки: Разместить клумбы перед южным входом||" },
                new Edit { Id=7, EditDate=new DateTime(2024, 2, 15, 20, 42, 0), Total=">>Правка от 2024-02-15T20:42:00|Причина: Просьба клиента|Бюджет: 2142735.00|Текст правки: Предусмотреть технологические помещения на цокольном этаже||" },
                new Edit { Id=8, EditDate=new DateTime(2024, 3, 21, 11, 50, 0), Total=">>Правка от 2024-03-21T11:50:00|Причина: Просьба клиента|Бюджет: 2725630.00|Текст правки: Убрать зону 13 и перераспределить пространство между соседними зонами||" }
            });
        }

        public async Task CreateProcedure()
        {
            int isProc = await Database.ExecuteSqlRawAsync("SELECT COUNT(*) FROM BDTesting.sys.objects WHERE type = 'P' AND name = 'UpdateProjectEdits'");
            if (isProc == 0)
            {
                await Database.ExecuteSqlRawAsync(@"GO
CREATE PROCEDURE [dbo].[UpdateProjectEdits] @projId tinyint, @editDesc text, @editDate nvarchar(max), @editReason nvarchar(50), @editBudget money AS
IF @editBudget = NULL
SET @editBudget = (SELECT [Бюджет] FROM [dbo].[Projects] WHERE [Id] = @projId)
DECLARE @total nvarchar(max)
SET @total = CONCAT('>>Правка от ', @editDate, '<br/>Причина: ', @editReason, '<br/>Бюджет: ', CAST(@editBudget as nvarchar(max)), '<br/>Текст правки: ', CAST(@editDesc AS nvarchar(max)), '<br/>')
IF (SELECT COUNT(*) FROM [dbo].[Edits] WHERE [Проект] = @projId) = 0
INSERT INTO [dbo].[Edits]([Проект], [Время], [Правки проекта], [Изменение бюджета]) VALUES (@projId, @editDate, @total, @editBudget)
ELSE
UPDATE [dbo].[Edits] SET [Время] = @editDate, [Правки проекта] = CONCAT(@total, [Правки проекта]), [Причина] = NULL, [Описание] = NULL, [Изменение бюджета] = @editBudget WHERE [Проект] = @projId
UPDATE [dbo].[Projects] SET [Наличие правок] = 1, [Бюджет] = @editBudget WHERE [Id] = @projId");
            }
        }
    }
}
