using System.Windows.Media.Media3D;
using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Shared.Interfaces;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Hosting;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using static ClassIsland.Core.Abstractions.Services.ILessonsService;
using ClassIsland.Shared.Models.Notification;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.IO;
using System.Formats.Tar;
using System.Text;
namespace PluginWithNotificationProviders.Services.NotificationProviders;

public class NamePickerNotification : INotificationProvider, IHostedService
{
    public ILessonsService LessonsService { get; }
    public string Name { get; set; } = "抽选结果提醒";
    public string Description { get; set; } = "显示NamePicker的抽选结果";
    public Guid ProviderGuid { get; set; } = new Guid("C0EB4196-98AA-4F4B-BDDB-E220BC8C3C38");
    public object? SettingsElement { get; set; }
    public object? IconElement { get; set; } = new PackIcon()
    {
        Kind = PackIconKind.TextLong,
        Width = 24,
        Height = 24
    };
    private INotificationHostService NotificationHostService { get; }
    

    public NamePickerNotification(INotificationHostService notificationHostService, ILessonsService lessonsService)
    {
        NotificationHostService = notificationHostService;
        LessonsService = lessonsService;
        NotificationHostService.RegisterNotificationProvider(this);
        LessonsService.PreMainTimerTicked += checkUnread;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
    private void checkUnread(object? sender, EventArgs e)
    {
        string result = Path.GetTempPath();
        if (File.Exists(result + "\\unread"))
        {
            File.Delete(result + "\\unread");
            string res = File.ReadAllText(result + "\\res.txt", Encoding.UTF8);
            NotificationHostService.ShowNotification(new NotificationRequest()
            {
                MaskContent = new TextBlock(new Run("抽选结果"))
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                },
                OverlayContent = new TextBlock(new Run("被抽中的幸运儿："+res))
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                },
                // 下面两个属性设置了语音播报的内容。
                MaskSpeechContent = "抽选结果",
                OverlaySpeechContent = "被抽中的幸运儿：" + res,
            });
        }
    }
}