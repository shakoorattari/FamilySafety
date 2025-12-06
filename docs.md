
# **Family Safety Application Development Guide**

## Version 1.0 - Complete Implementation Documentation for Windows Parental Control System

## **Table of Contents**

- [System Overview & Architecture](#system-overview--architecture)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Phase 1: Web Dashboard Development](#phase-1-web-dashboard-development)
- [Phase 2: Windows Client Application](#phase-2-windows-client-application)
- [Phase 3: Real-time Communication](#phase-3-real-time-communication)
- [Phase 4: Core Monitoring Features](#phase-4-core-monitoring-features)
- [Phase 5: Advanced Features](#phase-5-advanced-features)
- [Deployment & Distribution](#deployment--distribution)
- [Security Considerations](#security-considerations)
- [Testing Strategy](#testing-strategy)

## **System Overview & Architecture**

### **Application Vision**

Build a comprehensive parental control system that enables parents to monitor and manage their children's digital activities on Windows devices through a centralized web dashboard, similar to Qustodio's premium features.

### **Core Features (Based on Qustodio Analysis)**

- **Web Filtering & Content Blocking**: Block inappropriate websites and apps
- **Activity Monitoring**: Track browsing history, app usage, and screen time
- **Time Limits & Scheduling**: Set daily limits and screen-free schedules
- **Real-time Alerts**: AI-powered notifications for concerning activities
- **Detailed Reporting**: Daily/weekly activity reports
- **Remote Control**: Pause internet, block apps remotely

### **System Architecture Diagram**

```text
┌─────────────────────────────────────────────────────────────┐
│                    Parent Web Dashboard                      │
│                   (ASP.NET Core MVC/Razor)                   │
│  • Real-time monitoring UI                                  │
│  • Rule configuration                                       │
│  • Reports & analytics                                      │
└───────────────┬─────────────────────────────────────────────┘
                │ HTTPS/REST API + SignalR WebSockets
                │
┌───────────────┴─────────────────────────────────────────────┐
│                   Backend API Server                         │
│                 (ASP.NET Core Web API)                       │
│  • Authentication & authorization                           │
│  • Business logic & rules engine                            │
│  • Data aggregation & reporting                             │
│  • Alert processing                                         │
└───────────────┬─────────────────────────────────────────────┘
                │ Database (PostgreSQL/SQL Server)
                │
┌───────────────┴─────────────────────────────────────────────┐
│                Windows Client Applications                   │
│            (Windows Service + WPF/WinForms)                  │
│  • System monitoring (apps, web, time)                      │
│  • Rule enforcement                                         │
│  • Data collection & upload                                 │
│  • Local policy application                                 │
└─────────────────────────────────────────────────────────────┘
```

### **Data Flow**

1. **Windows Client** monitors system activities locally
2. **Collected data** sent to backend API periodically
3. **Backend processes** data, applies rules, generates alerts
4. **Web Dashboard** displays real-time data and historical reports
5. **Parent actions** (block, limit, pause) pushed to Windows Client

## **Technology Stack**

### **Backend & Web Dashboard**

- **.NET 10 SDK** - Primary development framework
- **ASP.NET Core Web API** - REST API backend
- **ASP.NET Core MVC/Razor Pages** - Web dashboard
- **Entity Framework Core 10** - Database ORM
- **PostgreSQL/SQL Server** - Primary database
- **SignalR** - Real-time communication
- **Serilog** - Structured logging
- **FluentValidation** - Input validation
- **AutoMapper** - Object mapping

### **Windows Client**

- **.NET 10 Windows Desktop** (WPF/WinForms)
- **Windows Service** - Background monitoring
- **Microsoft.Extensions.Hosting** - Dependency injection
- **System.Diagnostics** - Process monitoring
- **Windows API Code Pack** - System integration
- **Topshelf** - Windows service management

### **Infrastructure & DevOps**

- **Docker** - Containerization
- **Redis** - Caching and SignalR backplane
- **Hangfire** - Background job processing
- **xUnit/NUnit** - Testing framework
- **GitHub Actions/Azure DevOps** - CI/CD

## **Project Structure**

Following Clean Architecture/Onion Architecture principles for maintainability:

```text
FamilySafety/
├── src/
│   ├── FamilySafety.Web/                    # Web Dashboard (Presentation)
│   │   ├── Controllers/
│   │   ├── Views/Pages/
│   │   ├── ViewModels/
│   │   ├── wwwroot/
│   │   └── Program.cs
│   │
│   ├── FamilySafety.Api/                    # REST API
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── Filters/
│   │   └── Program.cs
│   │
│   ├── FamilySafety.Application/            # Business Logic Layer
│   │   ├── Features/
│   │   │   ├── Monitoring/
│   │   │   ├── Rules/
│   │   │   ├── Alerts/
│   │   │   └── Reports/
│   │   ├── Interfaces/
│   │   ├── DTOs/
│   │   └── Services/
│   │
│   ├── FamilySafety.Domain/                 # Domain Layer
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Enums/
│   │   └── Exceptions/
│   │
│   ├── FamilySafety.Infrastructure/         # Infrastructure Layer
│   │   ├── Persistence/
│   │   ├── Services/
│   │   ├── Notifications/
│   │   └── External/
│   │
│   └── FamilySafety.WindowsClient/          # Windows Client
│       ├── Services/
│       ├── Monitoring/
│       ├── UI/
│       └── Installers/
│
├── tests/
│   ├── FamilySafety.UnitTests/
│   ├── FamilySafety.IntegrationTests/
│   └── FamilySafety.E2ETests/
│
└── docs/
    ├── architecture.md
    ├── api-documentation.md
    └── deployment-guide.md
```

## **Phase 1: Web Dashboard Development**

### **Step 1.1: Create ASP.NET Core Web Application**

```bash
# Create solution
dotnet new sln -n FamilySafety

# Create Web Dashboard project
dotnet new webapp -n FamilySafety.Web -o src/FamilySafety.Web
dotnet sln add src/FamilySafety.Web

# Create API project
dotnet new webapi -n FamilySafety.Api -o src/FamilySafety.Api
dotnet sln add src/FamilySafety.Api
```

### **Step 1.2: Configure Authentication & Authorization**

```csharp
// Program.cs in FamilySafety.Web
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();
```

### **Step 1.3: Implement Parent Dashboard Layout**

```html
<!-- Views/Shared/_Layout.cshtml -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Family Safety Dashboard</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css">
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-primary border-bottom box-shadow mb-3">
            <div class="container-fluid">
                <a class="navbar-brand" asp-area="" asp-page="/Index">
                    <i class="bi bi-shield-check"></i> Family Safety
                </a>
                <partial name="_LoginPartial" />
            </div>
        </nav>
    </header>
    
    <div class="container-fluid">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### **Step 1.4: Create Dashboard Main Page**

```csharp
// Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IDashboardService _dashboardService;
    
    public DashboardViewModel Dashboard { get; set; }
    
    public IndexModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }
    
    public async Task OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Dashboard = await _dashboardService.GetFamilyDashboardAsync(userId);
    }
}

// Dashboard ViewModel
public class DashboardViewModel
{
    public List<ChildDeviceStatus> ChildDevices { get; set; }
    public DailyUsageSummary DailySummary { get; set; }
    public List<AlertNotification> RecentAlerts { get; set; }
    public ScreenTimeOverview ScreenTime { get; set; }
    
    public class ChildDeviceStatus
    {
        public string ChildName { get; set; }
        public string DeviceName { get; set; }
        public bool IsOnline { get; set; }
        public TimeSpan TodayUsage { get; set; }
        public DateTime? LastActive { get; set; }
    }
}
```

### **Step 1.5: Implement Device Management Interface**

```csharp
// Pages/Devices/Index.cshtml.cs
public class DeviceManagementModel : PageModel
{
    private readonly IDeviceService _deviceService;
    
    public List<DeviceDto> Devices { get; set; }
    
    [BindProperty]
    public AddDeviceModel NewDevice { get; set; }
    
    public DeviceManagementModel(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    
    public async Task OnGetAsync()
    {
        var familyId = User.FindFirstValue("FamilyId");
        Devices = await _deviceService.GetFamilyDevicesAsync(familyId);
    }
    
    public async Task<IActionResult> OnPostAddDeviceAsync()
    {
        if (ModelState.IsValid)
        {
            var result = await _deviceService.RegisterDeviceAsync(
                NewDevice.DeviceCode, 
                NewDevice.ChildName,
                User.FindFirstValue("FamilyId"));
                
            if (result.Success)
                return RedirectToPage();
        }
        return Page();
    }
}
```

## **Phase 2: Windows Client Application**

### **Step 2.1: Create Windows Service Project**

```xml
<!-- FamilySafety.WindowsClient.csproj -->
<Project Sdk="Microsoft.NET.Sdk.Worker">
  <PropertyGroup>
    <TargetFramework>net10.0-windows</TargetFramework>
    <OutputType>WinExe</OutputType>
    <UseWindowsForms>true</UseWindowsForms>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="10.0.0" />
    <PackageReference Include="Microsoft.Extensions.Hosting.WindowsServices" Version="10.0.0" />
    <PackageReference Include="Topshelf" Version="4.3.0" />
    <PackageReference Include="System.ServiceProcess.ServiceController" Version="8.0.0" />
  </ItemGroup>
</Project>
```

### **Step 2.2: Implement System Monitoring Service**

```csharp
// Services/SystemMonitorService.cs
using System.Diagnostics;
using System.Management;

public class SystemMonitorService : BackgroundService
{
    private readonly IActivityCollector _activityCollector;
    private readonly IDataUploadService _uploadService;
    private readonly ILogger<SystemMonitorService> _logger;
    
    private Timer _monitoringTimer;
    private Timer _uploadTimer;
    
    public SystemMonitorService(
        IActivityCollector activityCollector,
        IDataUploadService uploadService,
        ILogger<SystemMonitorService> logger)
    {
        _activityCollector = activityCollector;
        _uploadService = uploadService;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("System monitoring service starting...");
        
        // Start monitoring timers
        _monitoringTimer = new Timer(CollectSystemData, null, 
            TimeSpan.Zero, TimeSpan.FromSeconds(5));
            
        _uploadTimer = new Timer(UploadCollectedData, null,
            TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5));
            
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
    
    private void CollectSystemData(object state)
    {
        try
        {
            // Collect active processes
            var processes = Process.GetProcesses()
                .Where(p => !string.IsNullOrEmpty(p.MainWindowTitle))
                .Select(p => new ProcessInfo
                {
                    Name = p.ProcessName,
                    WindowTitle = p.MainWindowTitle,
                    StartTime = p.StartTime,
                    MemoryUsage = p.WorkingSet64
                });
                
            // Collect browser history (simplified example)
            var browserHistory = CollectBrowserHistory();
            
            // Store locally
            _activityCollector.RecordActivity(new DeviceActivity
            {
                Timestamp = DateTime.UtcNow,
                ActiveApplications = processes.ToList(),
                BrowserHistory = browserHistory,
                SystemUptime = TimeSpan.FromTicks(Environment.TickCount64 * TimeSpan.TicksPerMillisecond)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting system data");
        }
    }
    
    private List<BrowserHistoryItem> CollectBrowserHistory()
    {
        var history = new List<BrowserHistoryItem>();
        
        // Example: Read Chrome history (requires appropriate permissions)
        string chromeHistoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            @"Google\Chrome\User Data\Default\History");
            
        if (File.Exists(chromeHistoryPath))
        {
            try
            {
                // Use SQLite to read Chrome history
                // Note: Chrome locks the file, need to copy it first
                string tempCopy = Path.GetTempFileName();
                File.Copy(chromeHistoryPath, tempCopy, true);
                
                using (var connection = new SQLiteConnection($"Data Source={tempCopy};Version=3;"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = 
                        "SELECT url, title, last_visit_time FROM urls ORDER BY last_visit_time DESC LIMIT 50";
                        
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new BrowserHistoryItem
                            {
                                Url = reader.GetString(0),
                                Title = reader.GetString(1),
                                VisitTime = DateTime.FromFileTimeUtc(reader.GetInt64(2))
                            });
                        }
                    }
                }
                
                File.Delete(tempCopy);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not read Chrome history");
            }
        }
        
        return history;
    }
    
    private async void UploadCollectedData(object state)
    {
        try
        {
            var activities = _activityCollector.GetPendingActivities();
            if (activities.Any())
            {
                await _uploadService.UploadActivitiesAsync(activities);
                _activityCollector.ClearPendingActivities();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading data");
        }
    }
    
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("System monitoring service stopping...");
        
        _monitoringTimer?.Dispose();
        _uploadTimer?.Dispose();
        
        await base.StopAsync(stoppingToken);
    }
}
```

### **Step 2.3: Implement Application Blocking**

```csharp
// Services/ApplicationBlocker.cs
using System.Diagnostics;
using Microsoft.Win32;

public class ApplicationBlocker : IApplicationBlocker
{
    private readonly List<string> _blockedApplications;
    private readonly Timer _blockingTimer;
    private readonly ILogger<ApplicationBlocker> _logger;
    
    public ApplicationBlocker(ILogger<ApplicationBlocker> logger)
    {
        _logger = logger;
        _blockedApplications = new List<string>();
        _blockingTimer = new Timer(CheckAndBlockApplications, null, 
            TimeSpan.Zero, TimeSpan.FromSeconds(2));
    }
    
    public void BlockApplication(string processName)
    {
        if (!_blockedApplications.Contains(processName))
        {
            _blockedApplications.Add(processName);
            _logger.LogInformation("Blocked application: {ProcessName}", processName);
        }
    }
    
    public void UnblockApplication(string processName)
    {
        _blockedApplications.Remove(processName);
        _logger.LogInformation("Unblocked application: {ProcessName}", processName);
    }
    
    private void CheckAndBlockApplications(object state)
    {
        foreach (var processName in _blockedApplications)
        {
            var processes = Process.GetProcessesByName(processName);
            foreach (var process in processes)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        _logger.LogDebug("Killed blocked process: {ProcessName} (PID: {PID})", 
                            processName, process.Id);
                        
                        // Log the blocking event
                        LogBlockingEvent(processName, process.MainWindowTitle);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to kill process: {ProcessName}", processName);
                }
            }
        }
    }
    
    private void LogBlockingEvent(string processName, string windowTitle)
    {
        // Store blocking event for reporting
        var blockingEvent = new ApplicationBlockEvent
        {
            Timestamp = DateTime.UtcNow,
            ProcessName = processName,
            WindowTitle = windowTitle,
            UserName = Environment.UserName
        };
        
        // Save to local database or file
    }
    
    // Implement web content filtering through proxy settings
    public void EnableWebFiltering(List<string> blockedUrls)
    {
        try
        {
            // Set proxy settings for content filtering
            RegistryKey registry = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
                
            if (registry != null)
            {
                registry.SetValue("ProxyEnable", 1);
                registry.SetValue("ProxyServer", "127.0.0.1:8080"); // Local filtering proxy
                registry.SetValue("ProxyOverride", "<local>"); // Bypass local addresses
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enable web filtering");
        }
    }
}
```

### **Step 2.4: Create Installation & Configuration**

```csharp
// Program.cs for Windows Service
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureServices((hostContext, services) =>
            {
                // Add configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                    .Build();
                    
                services.AddSingleton<IConfiguration>(configuration);
                
                // Register services
                services.AddHostedService<SystemMonitorService>();
                services.AddSingleton<IActivityCollector, ActivityCollector>();
                services.AddSingleton<IDataUploadService, DataUploadService>();
                services.AddSingleton<IApplicationBlocker, ApplicationBlocker>();
                services.AddSingleton<IWebFilter, WebFilterService>();
                services.AddSingleton<ITimeLimitEnforcer, TimeLimitEnforcer>();
                
                // Add HTTP client for API communication
                services.AddHttpClient<FamilySafetyApiClient>(client =>
                {
                    client.BaseAddress = new Uri(configuration["Api:BaseUrl"]);
                    client.DefaultRequestHeaders.Add("User-Agent", "FamilySafety-WindowsClient");
                });
                
                // Add logging
                services.AddLogging(configure =>
                {
                    configure.AddConfiguration(configuration.GetSection("Logging"));
                    configure.AddConsole();
                    configure.AddEventLog(settings =>
                    {
                        settings.SourceName = "FamilySafetyService";
                        settings.LogName = "Application";
                    });
                });
            });
}
```

```xml
<!-- appsettings.json for Windows Client -->
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "EventLog": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  },
  "Api": {
    "BaseUrl": "https://api.yourfamilysafety.com",
    "DeviceId": "",
    "AuthToken": ""
  },
  "Monitoring": {
    "DataCollectionInterval": 5,
    "DataUploadInterval": 300,
    "MaxLocalStorageDays": 7,
    "EnableProcessMonitoring": true,
    "EnableWebMonitoring": true,
    "EnableTimeTracking": true
  },
  "Blocking": {
    "BlockedApplications": [],
    "BlockedWebsites": [],
    "AllowedApplications": []
  },
  "TimeLimits": {
    "DailyLimit": "02:00:00",
    "BedtimeStart": "21:00",
    "BedtimeEnd": "07:00",
    "BreakDuration": "00:15:00"
  }
}
```

### **Step 2.5: Create Setup & Installation Project**

```xml
<!-- Wix Toolset Setup Project (FamilySafety.Setup.wixproj) -->
<?xml version="1.0" encoding="UTF-8"?>
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi">
  <Product Id="*" Name="Family Safety Client" Language="1033" 
           Version="1.0.0.0" Manufacturer="Your Company" UpgradeCode="YOUR-GUID-HERE">
    
    <Package InstallerVersion="200" Compressed="yes" InstallScope="perMachine" />
    
    <MajorUpgrade DowngradeErrorMessage="A newer version of Family Safety is already installed." />
    <MediaTemplate EmbedCab="yes" />
    
    <Feature Id="ProductFeature" Title="Family Safety Client" Level="1">
      <ComponentGroupRef Id="ProductComponents" />
      <ComponentGroupRef Id="ServiceComponents" />
    </Feature>
    
    <CustomAction Id="InstallService" BinaryKey="ServiceInstaller" DllEntry="InstallService" />
    <CustomAction Id="UninstallService" BinaryKey="ServiceInstaller" DllEntry="UninstallService" />
    
    <InstallExecuteSequence>
      <Custom Action="InstallService" After="InstallFiles">NOT REMOVE</Custom>
      <Custom Action="UninstallService" After="InstallFiles">REMOVE</Custom>
    </InstallExecuteSequence>
  </Product>
  
  <Fragment>
    <Directory Id="TARGETDIR" Name="SourceDir">
      <Directory Id="ProgramFilesFolder">
        <Directory Id="INSTALLFOLDER" Name="Family Safety" />
      </Directory>
    </Directory>
  </Fragment>
  
  <Fragment>
    <ComponentGroup Id="ProductComponents" Directory="INSTALLFOLDER">
      <Component Id="MainExecutable" Guid="YOUR-GUID-HERE">
        <File Id="FamilySafetyClient.exe" Source="$(var.FamilySafety.WindowsClient.TargetPath)" 
              KeyPath="yes" Checksum="yes" />
        <File Id="appsettings.json" Source="$(var.FamilySafety.WindowsClient.TargetDir)\appsettings.json" />
        
        <!-- Create registry entry for auto-start -->
        <RegistryValue Root="HKLM" Key="SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
                       Name="FamilySafety" Type="string" Value="[INSTALLFOLDER]FamilySafetyClient.exe" 
                       KeyPath="no" />
      </Component>
    </ComponentGroup>
    
    <ComponentGroup Id="ServiceComponents" Directory="INSTALLFOLDER">
      <Component Id="WindowsService" Guid="YOUR-GUID-HERE">
        <File Id="FamilySafetyService.exe" Source="$(var.FamilySafety.Service.TargetPath)" />
        <ServiceInstall Id="FamilySafetyServiceInstall"
                        Type="ownProcess"
                        Name="FamilySafetyService"
                        DisplayName="Family Safety Monitoring Service"
                        Description="Monitors system activity for parental control"
                        Start="auto"
                        Account="LocalSystem"
                        ErrorControl="normal" />
        <ServiceControl Id="FamilySafetyServiceControl"
                        Name="FamilySafetyService"
                        Start="install"
                        Stop="both"
                        Remove="uninstall" />
      </Component>
    </ComponentGroup>
  </Fragment>
</Wix>
```

## **Phase 3: Real-time Communication**

### **Step 3.1: Implement SignalR for Real-time Updates**

```csharp
// Hubs/DashboardHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

[Authorize]
public class DashboardHub : Hub
{
    private readonly IConnectionManager _connectionManager;
    private readonly IAlertService _alertService;
    
    public DashboardHub(IConnectionManager connectionManager, IAlertService alertService)
    {
        _connectionManager = connectionManager;
        _alertService = alertService;
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var familyId = GetFamilyIdFromUser(userId);
        
        await _connectionManager.AddConnection(userId, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"family-{familyId}");
        
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.UserIdentifier;
        await _connectionManager.RemoveConnection(userId, Context.ConnectionId);
        
        await base.OnDisconnectedAsync(exception);
    }
    
    // Client can call this method to request real-time updates
    public async Task SubscribeToDevice(string deviceId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"device-{deviceId}");
    }
    
    // Server-side method to send alerts to clients
    public async Task SendAlertToFamily(string familyId, AlertNotification alert)
    {
        await Clients.Group($"family-{familyId}").SendAsync("ReceiveAlert", alert);
    }
    
    // Send activity updates to dashboard
    public async Task SendActivityUpdate(string deviceId, DeviceActivity activity)
    {
        await Clients.Group($"device-{deviceId}").SendAsync("ActivityUpdated", activity);
    }
}

// Program.cs configuration
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 65536;
})
.AddMessagePackProtocol() // For smaller message size
.AddAzureSignalR(); // For scaling (optional)

// Configure endpoints
app.MapHub<DashboardHub>("/realtime/dashboard");
app.MapHub<DeviceHub>("/realtime/device");
```

### **Step 3.2: Web Dashboard JavaScript Client**

```javascript
// wwwroot/js/realtime-dashboard.js
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/realtime/dashboard", {
        accessTokenFactory: () => {
            return document.querySelector("input[name='__RequestVerificationToken']").value;
        }
    })
    .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: retryContext => {
            // Exponential backoff
            return Math.min(1000 * Math.pow(2, retryContext.previousRetryCount), 30000);
        }
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Connection event handlers
connection.on("ReceiveAlert", (alert) => {
    showAlertNotification(alert);
    updateAlertBadge();
});

connection.on("ActivityUpdated", (activity) => {
    updateActivityChart(activity);
    updateDeviceStatus(activity.deviceId, activity.isActive);
});

connection.on("ScreenTimeLimitReached", (deviceId) => {
    showScreenTimeWarning(deviceId);
});

connection.on("BlockedContentAccessed", (event) => {
    logBlockedContent(event);
    showBlockedContentAlert(event);
});

// Start connection
async function startConnection() {
    try {
        await connection.start();
        console.log("Connected to real-time dashboard");
        
        // Subscribe to family devices
        const familyId = document.getElementById("familyId").value;
        await connection.invoke("SubscribeToFamily", familyId);
        
        // Request initial data
        await connection.invoke("RequestInitialData");
        
    } catch (err) {
        console.error("Connection error:", err);
        setTimeout(startConnection, 5000);
    }
}

// Send commands to Windows client
async function pauseDevice(deviceId, durationMinutes) {
    await connection.invoke("PauseDevice", deviceId, durationMinutes);
}

async function blockApplication(deviceId, appName) {
    await connection.invoke("BlockApplication", deviceId, appName);
}

async function setTimeLimit(deviceId, dailyLimitMinutes) {
    await connection.invoke("SetTimeLimit", deviceId, dailyLimitMinutes);
}

// Initialize
document.addEventListener("DOMContentLoaded", () => {
    startConnection();
});
```

## **Phase 4: Core Monitoring Features**

### **Step 4.1: Web Content Filtering**

```csharp
// Services/WebFilterService.cs
using System.Net;
using System.Text.RegularExpressions;

public class WebFilterService : IWebFilter
{
    private readonly List<FilterRule> _filterRules;
    private readonly ILogger<WebFilterService> _logger;
    private readonly HttpClient _httpClient;
    
    public WebFilterService(ILogger<WebFilterService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        _filterRules = new List<FilterRule>();
    }
    
    public async Task<bool> IsUrlAllowed(string url, string category, int ageRating)
    {
        // Check against blocked URLs
        if (_filterRules.Any(r => r.MatchesUrl(url)))
        {
            _logger.LogInformation("Blocked URL: {Url} (Rule: {RuleName})", 
                url, _filterRules.First(r => r.MatchesUrl(url)).Name);
            return false;
        }
        
        // Check category-based filtering
        if (ShouldBlockCategory(category, ageRating))
        {
            return false;
        }
        
        // AI-based content analysis (simplified example)
        if (await ContainsInappropriateContent(url))
        {
            return false;
        }
        
        return true;
    }
    
    private async Task<bool> ContainsInappropriateContent(string url)
    {
        try
        {
            // For production, use a proper content analysis API
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                // Simple keyword matching (expand this list)
                var inappropriateKeywords = new[]
                {
                    "porn", "xxx", "adult", "violence", "hate", "drugs",
                    "weapons", "gambling", "casino", "betting"
                };
                
                var regexPattern = string.Join("|", inappropriateKeywords
                    .Select(k => $@"\b{Regex.Escape(k)}\b"));
                    
                return Regex.IsMatch(content, regexPattern, 
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to analyze content for URL: {Url}", url);
        }
        
        return false;
    }
    
    public void AddFilterRule(FilterRule rule)
    {
        _filterRules.Add(rule);
        _logger.LogInformation("Added filter rule: {RuleName}", rule.Name);
    }
    
    public void RemoveFilterRule(string ruleName)
    {
        _filterRules.RemoveAll(r => r.Name == ruleName);
    }
    
    public List<WebsiteAccessEvent> GetBlockedAccessLogs(DateTime fromDate)
    {
        // Retrieve from database
        return new List<WebsiteAccessEvent>();
    }
}

// Filter rule class
public class FilterRule
{
    public string Name { get; set; }
    public RuleType Type { get; set; }
    public List<string> Patterns { get; set; }
    public List<string> Exceptions { get; set; }
    public TimeSpan? ActiveTimeRange { get; set; }
    
    public bool MatchesUrl(string url)
    {
        foreach (var pattern in Patterns)
        {
            if (pattern.StartsWith("*."))
            {
                // Domain pattern like *.example.com
                var domain = pattern.Substring(2);
                if (url.Contains(domain))
                    return !Exceptions.Any(e => url.Contains(e));
            }
            else if (pattern.Contains("*"))
            {
                // Wildcard pattern
                var regexPattern = "^" + Regex.Escape(pattern)
                    .Replace("\\*", ".*") + "$";
                    
                if (Regex.IsMatch(url, regexPattern, RegexOptions.IgnoreCase))
                    return !Exceptions.Any(e => url.Contains(e));
            }
            else
            {
                // Exact match
                if (url.Equals(pattern, StringComparison.OrdinalIgnoreCase))
                    return !Exceptions.Any(e => url.Contains(e));
            }
        }
        
        return false;
    }
}
```

### **Step 4.2: Time Limit Enforcement**

```csharp
// Services/TimeLimitEnforcer.cs
public class TimeLimitEnforcer : ITimeLimitEnforcer
{
    private readonly Dictionary<string, TimeLimitPolicy> _policies;
    private readonly Dictionary<string, DailyUsage> _dailyUsage;
    private readonly Timer _enforcementTimer;
    private readonly ILogger<TimeLimitEnforcer> _logger;
    
    public TimeLimitEnforcer(ILogger<TimeLimitEnforcer> logger)
    {
        _logger = logger;
        _policies = new Dictionary<string, TimeLimitPolicy>();
        _dailyUsage = new Dictionary<string, DailyUsage>();
        _enforcementTimer = new Timer(EnforceTimeLimits, null, 
            TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }
    
    public void SetTimeLimitPolicy(string deviceId, TimeLimitPolicy policy)
    {
        _policies[deviceId] = policy;
        _logger.LogInformation("Set time limit policy for device {DeviceId}: {Policy}", 
            deviceId, policy.DailyLimit);
    }
    
    public void RecordUsage(string deviceId, TimeSpan usage)
    {
        var today = DateTime.Today;
        var key = $"{deviceId}-{today:yyyyMMdd}";
        
        if (!_dailyUsage.ContainsKey(key))
        {
            _dailyUsage[key] = new DailyUsage
            {
                DeviceId = deviceId,
                Date = today,
                TotalUsage = TimeSpan.Zero
            };
        }
        
        _dailyUsage[key].TotalUsage += usage;
        
        // Check if limit is reached
        var policy = GetPolicyForDevice(deviceId);
        if (policy != null && _dailyUsage[key].TotalUsage >= policy.DailyLimit)
        {
            OnTimeLimitReached(deviceId);
        }
    }
    
    private void EnforceTimeLimits(object state)
    {
        var now = DateTime.Now;
        
        foreach (var policy in _policies)
        {
            // Check bedtime
            if (policy.Value.BedtimeStart.HasValue && policy.Value.BedtimeEnd.HasValue)
            {
                if (IsInBedtime(now, policy.Value.BedtimeStart.Value, 
                    policy.Value.BedtimeEnd.Value))
                {
                    BlockDeviceDuringBedtime(policy.Key);
                }
            }
            
            // Check break time enforcement
            if (policy.Value.BreakAfterMinutes > 0)
            {
                EnforceBreakTime(policy.Key, policy.Value);
            }
        }
    }
    
    private bool IsInBedtime(DateTime time, TimeSpan bedtimeStart, TimeSpan bedtimeEnd)
    {
        var timeOfDay = time.TimeOfDay;
        
        if (bedtimeStart < bedtimeEnd)
        {
            // Bedtime within same day (e.g., 21:00 to 07:00)
            return timeOfDay >= bedtimeStart || timeOfDay < bedtimeEnd;
        }
        else
        {
            // Bedtime spans midnight (e.g., 22:00 to 06:00)
            return timeOfDay >= bedtimeStart || timeOfDay < bedtimeEnd;
        }
    }
    
    private void BlockDeviceDuringBedtime(string deviceId)
    {
        // Implement device blocking logic
        _logger.LogInformation("Blocking device {DeviceId} during bedtime", deviceId);
        
        // This would trigger actual blocking in the Windows client
        var blockEvent = new BedtimeBlockEvent
        {
            DeviceId = deviceId,
            BlockTime = DateTime.UtcNow,
            Reason = "Bedtime enforcement"
        };
        
        // Send blocking command to Windows client
        SendBlockCommand(deviceId, blockEvent);
    }
    
    private void OnTimeLimitReached(string deviceId)
    {
        _logger.LogInformation("Daily time limit reached for device {DeviceId}", deviceId);
        
        // Send notification to parent dashboard
        var alert = new TimeLimitAlert
        {
            DeviceId = deviceId,
            AlertTime = DateTime.UtcNow,
            Message = $"Daily time limit reached for device {deviceId}",
            Usage = _dailyUsage[$"{deviceId}-{DateTime.Today:yyyyMMdd}"].TotalUsage
        };
        
        SendAlertToDashboard(alert);
        
        // Block device
        BlockDeviceForRemainderOfDay(deviceId);
    }
    
    public class TimeLimitPolicy
    {
        public TimeSpan DailyLimit { get; set; }
        public TimeSpan? BedtimeStart { get; set; }
        public TimeSpan? BedtimeEnd { get; set; }
        public int BreakAfterMinutes { get; set; }
        public TimeSpan BreakDuration { get; set; }
        public List<DayOfWeek> ActiveDays { get; set; }
    }
}
```

### **Step 4.3: Activity Reporting Engine**

```csharp
// Services/ReportGenerator.cs
public class ReportGenerator : IReportGenerator
{
    private readonly IActivityRepository _activityRepository;
    private readonly ITemplateEngine _templateEngine;
    private readonly IEmailService _emailService;
    
    public ReportGenerator(
        IActivityRepository activityRepository,
        ITemplateEngine templateEngine,
        IEmailService emailService)
    {
        _activityRepository = activityRepository;
        _templateEngine = templateEngine;
        _emailService = emailService;
    }
    
    public async Task<Report> GenerateDailyReport(string familyId, DateTime date)
    {
        var activities = await _activityRepository.GetFamilyActivitiesAsync(
            familyId, date, date.AddDays(1));
            
        var report = new DailyReport
        {
            ReportDate = date,
            FamilyId = familyId,
            TotalScreenTime = CalculateTotalScreenTime(activities),
            MostUsedApplications = GetTopApplications(activities, 5),
            WebsiteCategories = CategorizeWebsites(activities),
            Alerts = activities.Where(a => a.AlertLevel > 0).ToList(),
            TimeLimitCompliance = CalculateCompliance(activities)
        };
        
        // Generate HTML report
        report.HtmlContent = await _templateEngine.RenderAsync(
            "Templates/DailyReport.cshtml", report);
            
        return report;
    }
    
    public async Task<Report> GenerateWeeklyReport(string familyId, DateTime startDate)
    {
        var endDate = startDate.AddDays(7);
        var activities = await _activityRepository.GetFamilyActivitiesAsync(
            familyId, startDate, endDate);
            
        var report = new WeeklyReport
        {
            StartDate = startDate,
            EndDate = endDate,
            FamilyId = familyId,
            DailySummaries = new List<DailySummary>(),
            Trends = AnalyzeTrends(activities),
            Recommendations = GenerateRecommendations(activities)
        };
        
        // Generate daily summaries
        for (int i = 0; i < 7; i++)
        {
            var day = startDate.AddDays(i);
            var dayActivities = activities.Where(a => a.Timestamp.Date == day.Date);
            
            report.DailySummaries.Add(new DailySummary
            {
                Date = day,
                TotalScreenTime = CalculateTotalScreenTime(dayActivities),
                TopApplications = GetTopApplications(dayActivities, 3),
                AlertCount = dayActivities.Count(a => a.AlertLevel > 0)
            });
        }
        
        report.HtmlContent = await _templateEngine.RenderAsync(
            "Templates/WeeklyReport.cshtml", report);
            
        return report;
    }
    
    public async Task ScheduleReportDelivery(string familyId, ReportSchedule schedule)
    {
        // Use Hangfire or similar for scheduling
        switch (schedule.Frequency)
        {
            case ReportFrequency.Daily:
                RecurringJob.AddOrUpdate(
                    $"daily-report-{familyId}",
                    () => GenerateAndSendDailyReport(familyId),
                    schedule.DeliveryTime,
                    TimeZoneInfo.Local);
                break;
                
            case ReportFrequency.Weekly:
                RecurringJob.AddOrUpdate(
                    $"weekly-report-{familyId}",
                    () => GenerateAndSendWeeklyReport(familyId),
                    $"{schedule.DayOfWeek} {schedule.DeliveryTime}",
                    TimeZoneInfo.Local);
                break;
        }
    }
    
    private async Task GenerateAndSendDailyReport(string familyId)
    {
        var report = await GenerateDailyReport(familyId, DateTime.Today.AddDays(-1));
        var recipients = await GetReportRecipientsAsync(familyId);
        
        foreach (var recipient in recipients)
        {
            await _emailService.SendReportAsync(recipient.Email, report);
        }
    }
}
```

## **Phase 5: Advanced Features**

### **Step 5.1: AI-Powered Alert System**

```csharp
// Services/AIAlertService.cs
using Microsoft.ML;
using System.Text.Json;

public class AIAlertService : IAlertService
{
    private readonly MLContext _mlContext;
    private readonly ITransformer _model;
    private readonly ILogger<AIAlertService> _logger;
    private readonly HttpClient _httpClient;
    
    public AIAlertService(ILogger<AIAlertService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        _mlContext = new MLContext();
        
        // Load pre-trained model
        _model = LoadAlertModel();
    }
    
    public async Task<List<Alert>> AnalyzeActivitiesAsync(List<DeviceActivity> activities)
    {
        var alerts = new List<Alert>();
        
        foreach (var activity in activities)
        {
            // Analyze browsing history
            var browsingAlerts = await AnalyzeBrowsingHistoryAsync(activity.BrowserHistory);
            alerts.AddRange(browsingAlerts);
            
            // Analyze application usage patterns
            var appAlerts = AnalyzeApplicationUsage(activity.ActiveApplications);
            alerts.AddRange(appAlerts);
            
            // Analyze time patterns
            var timeAlerts = AnalyzeTimePatterns(activity);
            alerts.AddRange(timeAlerts);
            
            // Analyze social media content (if available)
            var socialAlerts = await AnalyzeSocialMediaContentAsync(activity);
            alerts.AddRange(socialAlerts);
        }
        
        return alerts.Where(a => a.ConfidenceScore > 0.7).ToList();
    }
    
    private async Task<List<Alert>> AnalyzeBrowsingHistoryAsync(List<BrowserHistoryItem> history)
    {
        var alerts = new List<Alert>();
        
        // Use ML model to classify websites
        var predictionEngine = _mlContext.Model.CreatePredictionEngine
            <WebsiteData, WebsitePrediction>(_model);
        
        foreach (var item in history.TakeLast(50)) // Analyze recent history
        {
            var prediction = predictionEngine.Predict(new WebsiteData
            {
                Url = item.Url,
                Title = item.Title,
                VisitDuration = item.VisitDuration.TotalMinutes
            });
            
            if (prediction.IsRisky)
            {
                alerts.Add(new Alert
                {
                    Type = AlertType.RiskyWebsite,
                    Severity = prediction.RiskLevel,
                    Message = $"Visited potentially risky website: {item.Title}",
                    Timestamp = item.VisitTime,
                    ConfidenceScore = prediction.Probability,
                    Details = new
                    {
                        Url = item.Url,
                        Category = prediction.Category,
                        RiskFactors = prediction.RiskFactors
                    }
                });
            }
        }
        
        // Check for excessive browsing in certain categories
        var categoryCounts = history
            .GroupBy(h => GetWebsiteCategory(h.Url))
            .ToDictionary(g => g.Key, g => g.Count());
            
        foreach (var category in new[] { "Gaming", "Social Media", "Streaming" })
        {
            if (categoryCounts.ContainsKey(category) && 
                categoryCounts[category] > GetCategoryThreshold(category))
            {
                alerts.Add(new Alert
                {
                    Type = AlertType.ExcessiveCategoryUsage,
                    Severity = AlertSeverity.Medium,
                    Message = $"Excessive {category} browsing detected",
                    Timestamp = DateTime.UtcNow,
                    ConfidenceScore = 0.8,
                    Details = new
                    {
                        Category = category,
                        VisitCount = categoryCounts[category],
                        Threshold = GetCategoryThreshold(category)
                    }
                });
            }
        }
        
        return alerts;
    }
    
    private List<Alert> AnalyzeApplicationUsage(List<ProcessInfo> applications)
    {
        var alerts = new List<Alert>();
        
        // Detect gaming applications during study hours
        var gamingApps = applications.Where(IsGamingApplication).ToList();
        if (gamingApps.Any() && IsDuringStudyHours(DateTime.Now))
        {
            alerts.Add(new Alert
            {
                Type = AlertType.GamingDuringStudyTime,
                Severity = AlertSeverity.High,
                Message = "Gaming detected during study hours",
                Timestamp = DateTime.UtcNow,
                ConfidenceScore = 0.9,
                Details = new
                {
                    Applications = gamingApps.Select(a => a.Name),
                    StudyHours = GetStudyHours()
                }
            });
        }
        
        // Detect unauthorized applications
        var unauthorized = applications.Where(IsUnauthorizedApplication).ToList();
        if (unauthorized.Any())
        {
            alerts.Add(new Alert
            {
                Type = AlertType.UnauthorizedApplication,
                Severity = AlertSeverity.High,
                Message = "Unauthorized applications detected",
                Timestamp = DateTime.UtcNow,
                ConfidenceScore = 1.0,
                Details = new
                {
                    Applications = unauthorized.Select(a => a.Name)
                }
            });
        }
        
        return alerts;
    }
    
    private async Task<List<Alert>> AnalyzeSocialMediaContentAsync(DeviceActivity activity)
    {
        var alerts = new List<Alert>();
        
        // This would integrate with social media monitoring APIs
        // For example, analyzing message content for concerning patterns
        
        // Example: Check for cyberbullying keywords
        var concerningKeywords = new[]
        {
            "kill yourself", "I hate you", "you're worthless",
            "nobody likes you", "you should die"
        };
        
        // In a real implementation, this would analyze actual message content
        // from social media apps (with proper privacy considerations)
        
        return alerts;
    }
    
    public async Task TrainModelAsync(List<TrainingExample> examples)
    {
        // Load training data
        var dataView = _mlContext.Data.LoadFromEnumerable(examples);
        
        // Define pipeline
        var pipeline = _mlContext.Transforms.Text.FeaturizeText(
                outputColumnName: "Features", 
                inputColumnName: nameof(TrainingExample.Text))
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());
        
        // Train model
        var model = pipeline.Fit(dataView);
        
        // Save model
        _mlContext.Model.Save(model, dataView.Schema, "AlertModel.zip");
    }
}
```

### **Step 5.2: Location Tracking (if applicable)**

```csharp
// Services/LocationService.cs
public class LocationService : ILocationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LocationService> _logger;
    
    public LocationService(HttpClient httpClient, ILogger<LocationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<LocationInfo> GetCurrentLocationAsync()
    {
        try
        {
            // For Windows, we might use IP-based location or GPS if available
            var publicIp = await GetPublicIpAddressAsync();
            var location = await GetLocationFromIpAsync(publicIp);
            
            return location;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get location");
            return null;
        }
    }
    
    public async Task<List<LocationHistory>> GetLocationHistoryAsync(
        string deviceId, DateTime fromDate, DateTime toDate)
    {
        // Retrieve from database
        return new List<LocationHistory>();
    }
    
    public void SetGeofence(string deviceId, Geofence geofence)
    {
        // Store geofence configuration
    }
    
    public async Task<bool> CheckGeofenceViolationAsync(
        string deviceId, LocationInfo location)
    {
        var geofence = GetGeofenceForDevice(deviceId);
        if (geofence == null)
            return false;
            
        var distance = CalculateDistance(
            location.Latitude, location.Longitude,
            geofence.CenterLatitude, geofence.CenterLongitude);
            
        return distance > geofence.RadiusMeters;
    }
    
    private async Task<string> GetPublicIpAddressAsync()
    {
        var response = await _httpClient.GetStringAsync("https://api.ipify.org");
        return response.Trim();
    }
    
    private async Task<LocationInfo> GetLocationFromIpAsync(string ipAddress)
    {
        try
        {
            var response = await _httpClient.GetStringAsync(
                $"http://ip-api.com/json/{ipAddress}");
                
            var result = JsonSerializer.Deserialize<IpApiResponse>(response);
            
            if (result.Status == "success")
            {
                return new LocationInfo
                {
                    Latitude = result.Lat,
                    Longitude = result.Lon,
                    City = result.City,
                    Region = result.RegionName,
                    Country = result.Country,
                    Timestamp = DateTime.UtcNow
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get location from IP");
        }
        
        return null;
    }
}
```

## **Deployment & Distribution**

### **Step 6.1: Docker Configuration**

```dockerfile
# Dockerfile for API
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["FamilySafety.Api/FamilySafety.Api.csproj", "FamilySafety.Api/"]
COPY ["FamilySafety.Application/FamilySafety.Application.csproj", "FamilySafety.Application/"]
COPY ["FamilySafety.Domain/FamilySafety.Domain.csproj", "FamilySafety.Domain/"]
COPY ["FamilySafety.Infrastructure/FamilySafety.Infrastructure.csproj", "FamilySafety.Infrastructure/"]
RUN dotnet restore "FamilySafety.Api/FamilySafety.Api.csproj"
COPY . .
WORKDIR "/src/FamilySafety.Api"
RUN dotnet build "FamilySafety.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FamilySafety.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamilySafety.Api.dll"]
```

```yaml
# docker-compose.yml
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: FamilySafety.Api/Dockerfile
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=FamilySafety;User=sa;Password=YourPassword123;
    depends_on:
      - db
      - redis
    networks:
      - familysafety-network

  web:
    build:
      context: .
      dockerfile: FamilySafety.Web/Dockerfile
    ports:
      - "8080:80"
      - "8081:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - Api__BaseUrl=http://api:80
    depends_on:
      - api
    networks:
      - familysafety-network

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123
    ports:
      - "1433:1433"
    volumes:
      - sql-data:/var/opt/mssql
    networks:
      - familysafety-network

  redis:
    image: redis:alpine
    ports:
      - "6379:6379"
    networks:
      - familysafety-network

  hangfire:
    build:
      context: .
      dockerfile: FamilySafety.Hangfire/Dockerfile
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Database=FamilySafety;User=sa;Password=YourPassword123;
    depends_on:
      - db
    networks:
      - familysafety-network

networks:
  familysafety-network:
    driver: bridge

volumes:
  sql-data:
```

### **Step 6.2: CI/CD Pipeline (GitHub Actions)**

```yaml
# .github/workflows/build-deploy.yml
name: Build and Deploy

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

env:
  REGISTRY: ghcr.io
  IMAGE_NAME: ${{ github.repository }}

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '10.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Test
      run: dotnet test --configuration Release --no-build --verbosity normal
    
    - name: Publish
      run: dotnet publish -c Release -o ./publish
    
    - name: Upload artifact
      uses: actions/upload-artifact@v3
      with:
        name: published-app
        path: ./publish

  docker:
    runs-on: ubuntu-latest
    needs: build
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Log in to container registry
      uses: docker/login-action@v2
      with:
        registry: ${{ env.REGISTRY }}
        username: ${{ github.actor }}
        password: ${{ secrets.GITHUB_TOKEN }}
    
    - name: Extract metadata
      id: meta
      uses: docker/metadata-action@v4
      with:
        images: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}
    
    - name: Build and push API image
      uses: docker/build-push-action@v4
      with:
        context: .
        file: ./FamilySafety.Api/Dockerfile
        push: true
        tags: ${{ steps.meta.outputs.tags }}
        labels: ${{ steps.meta.outputs.labels }}
    
    - name: Build and push Web image
      uses: docker/build-push-action@v4
      with:
        context: .
        file: ./FamilySafety.Web/Dockerfile
        push: true
        tags: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}-web:${{ github.sha }}

  deploy:
    runs-on: ubuntu-latest
    needs: [build, docker]
    if: github.ref == 'refs/heads/main'
    
    steps:
    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'familysafety-api'
        publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
        package: ./publish
```

## **Security Considerations**

### **Data Protection**

```csharp
// Services/DataEncryptionService.cs
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;

public class DataEncryptionService : IDataEncryptionService
{
    private readonly IDataProtector _protector;
    private readonly ILogger<DataEncryptionService> _logger;
    
    public DataEncryptionService(
        IDataProtectionProvider dataProtectionProvider,
        ILogger<DataEncryptionService> logger)
    {
        _protector = dataProtectionProvider.CreateProtector("FamilySafety.Data");
        _logger = logger;
    }
    
    public string EncryptSensitiveData(string plainText)
    {
        try
        {
            return _protector.Protect(plainText);
        }
        catch (CryptographicException ex)
        {
            _logger.LogError(ex, "Failed to encrypt data");
            throw;
        }
    }
    
    public string DecryptSensitiveData(string cipherText)
    {
        try
        {
            return _protector.Unprotect(cipherText);
        }
        catch (CryptographicException ex)
        {
            _logger.LogError(ex, "Failed to decrypt data");
            throw;
        }
    }
    
    public string HashData(string data)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}

// GDPR compliance methods
public class GdprService : IGdprService
{
    public async Task AnonymizeUserDataAsync(string userId)
    {
        // Replace identifiable data with anonymous markers
        await _userRepository.AnonymizeUserAsync(userId);
        await _activityRepository.AnonymizeActivitiesAsync(userId);
        await _deviceRepository.AnonymizeDevicesAsync(userId);
    }
    
    public async Task ExportUserDataAsync(string userId)
    {
        var userData = new UserDataExport
        {
            UserInfo = await _userRepository.GetUserForExportAsync(userId),
            Activities = await _activityRepository.GetUserActivitiesForExportAsync(userId),
            Devices = await _deviceRepository.GetUserDevicesForExportAsync(userId),
            Settings = await _settingsRepository.GetUserSettingsForExportAsync(userId)
        };
        
        return JsonSerializer.Serialize(userData, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
    
    public async Task DeleteUserDataAsync(string userId)
    {
        // Soft delete with retention period
        await _userRepository.SoftDeleteUserAsync(userId);
        
        // Schedule permanent deletion (after 30 days)
        BackgroundJob.Schedule(
            () => PermanentDeleteUserData(userId),
            TimeSpan.FromDays(30));
    }
}
```

### **Security Headers & Policies**

```csharp
// Middleware/SecurityHeadersMiddleware.cs
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    
    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Add security headers
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
        context.Response.Headers.Append(
            "Content-Security-Policy", 
            "default-src 'self'; script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
            "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
            "img-src 'self' data: https:; font-src 'self' https://cdn.jsdelivr.net;");
        
        await _next(context);
    }
}

// Program.cs configuration
app.UseHsts(options => options
    .MaxAge(days: 365)
    .IncludeSubdomains()
    .Preload());

app.UseCors(policy => policy
    .WithOrigins("https://dashboard.yourfamilysafety.com")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseRateLimiter(new RateLimiterOptions
{
    GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        return RateLimitPartition.GetSlidingWindowLimiter(
            context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString(),
            partition => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 10
            });
    })
});
```

## **Testing Strategy**

### **Unit Tests**

```csharp
// Tests for TimeLimitEnforcer
public class TimeLimitEnforcerTests
{
    private Mock<ILogger<TimeLimitEnforcer>> _loggerMock;
    private TimeLimitEnforcer _enforcer;
    
    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<TimeLimitEnforcer>>();
        _enforcer = new TimeLimitEnforcer(_loggerMock.Object);
    }
    
    [Test]
    public void RecordUsage_WhenDailyLimitReached_ShouldTriggerAlert()
    {
        // Arrange
        var deviceId = "test-device-1";
        var policy = new TimeLimitPolicy
        {
            DailyLimit = TimeSpan.FromHours(2)
        };
        _enforcer.SetTimeLimitPolicy(deviceId, policy);
        
        // Act - Record usage exceeding limit
        _enforcer.RecordUsage(deviceId, TimeSpan.FromHours(1.5));
        _enforcer.RecordUsage(deviceId, TimeSpan.FromHours(1));
        
        // Assert
        // Check that alert was triggered
        // This would require mocking or checking internal state
    }
    
    [Test]
    public void IsInBedtime_WhenTimeInRange_ShouldReturnTrue()
    {
        // Arrange
        var bedtimeStart = new TimeSpan(21, 0, 0); // 9 PM
        var bedtimeEnd = new TimeSpan(7, 0, 0);    // 7 AM
        
        // Act & Assert
        Assert.IsTrue(_enforcer.IsInBedtime(
            new DateTime(2024, 1, 1, 22, 0, 0), // 10 PM
            bedtimeStart, bedtimeEnd));
            
        Assert.IsTrue(_enforcer.IsInBedtime(
            new DateTime(2024, 1, 2, 6, 0, 0),  // 6 AM
            bedtimeStart, bedtimeEnd));
            
        Assert.IsFalse(_enforcer.IsInBedtime(
            new DateTime(2024, 1, 1, 15, 0, 0), // 3 PM
            bedtimeStart, bedtimeEnd));
    }
}

// Integration tests for API
[TestFixture]
public class DashboardApiIntegrationTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    
    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Replace real services with test doubles
                    services.AddScoped<IActivityRepository, TestActivityRepository>();
                    services.AddScoped<IAlertService, TestAlertService>();
                });
            });
            
        _client = _factory.CreateClient();
        
        // Add authentication token
        var token = GenerateTestToken();
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }
    
    [Test]
    public async Task GetDashboard_AuthenticatedUser_ReturnsDashboardData()
    {
        // Act
        var response = await _client.GetAsync("/api/dashboard");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var dashboard = await response.Content.ReadFromJsonAsync<DashboardViewModel>();
        Assert.NotNull(dashboard);
        Assert.NotNull(dashboard.ChildDevices);
    }
    
    [Test]
    public async Task BlockApplication_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new BlockApplicationRequest
        {
            DeviceId = "test-device-1",
            ApplicationName = "forbidden-game.exe",
            DurationMinutes = 60
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/devices/block", request);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.IsTrue(result.Success);
    }
}
```

## **Next Steps & Gradual Development**

### **Week 1-2: Foundation**

1. Set up solution structure with Clean Architecture
2. Implement basic ASP.NET Core Web API
3. Create database schema with Entity Framework Core
4. Implement basic authentication with Identity

### **Week 3-4: Core Features**

1. Develop Windows client monitoring service
2. Implement basic activity collection
3. Create web dashboard with Razor Pages
4. Set up SignalR for real-time updates

### **Week 5-6: Monitoring & Control**

1. Implement web content filtering
2. Add application blocking
3. Create time limit enforcement
4. Develop reporting engine

### **Week 7-8: Advanced Features**

1. Implement AI-powered alert system
2. Add scheduling and routines
3. Create detailed reporting
4. Implement remote control features

### **Week 9-10: Polish & Deployment**

1. Add comprehensive testing
2. Implement security hardening
3. Create installation packages
4. Set up CI/CD pipeline

### **Development Tips**

- Start with a minimal viable product and iterate
- Use feature flags to gradually roll out functionality
- Implement comprehensive logging from day one
- Consider privacy implications at every step
- Regularly test on actual Windows 10/11 machines
- Use Windows Event Log for client-side logging
- Implement proper error handling and user feedback

This comprehensive guide provides you with a complete roadmap for developing a family safety application similar to Qustodio. Remember to:

1. Always prioritize user privacy and data security
2. Follow Windows development best practices
3. Test thoroughly on both Windows 10 and 11
4. Keep the parent dashboard intuitive and responsive
5. Implement proper error handling and recovery
6. Consider scalability from the beginning

The architecture outlined here follows modern .NET practices and provides a solid foundation for building a robust parental control system.
