using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Core.Abstractions.Services.NotificationProviders;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Models.Notification;
using ClassIsland.Shared.Interfaces;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using static ClassIsland.Core.Abstractions.Services.ILessonsService;
using ClassIsland.Shared.Models.Notification;
using System.Windows.Documents;
using System.Windows;
using System.IO;
using System.Formats.Tar;
using NotificationRequest = ClassIsland.Core.Models.Notification.NotificationRequest;


namespace PluginWithNotificationProviders.Services.NotificationProviders;

[NotificationProviderInfo("A0C99B32-EFA4-4843-ADF6-54DE7C6FCD56", "NamePicker抽选结果", PackIconKind.Airplane, "显示NamePicker的抽选结果")]
public class NamePickerNotification : NotificationProviderBase
{
    public ILessonsService LessonsService { get; }

    public NamePickerNotification(ILessonsService lessonsService)
    {
        LessonsService = lessonsService; 
        LessonsService.PreMainTimerTicked += LessonsServiceOnOnTicked; 
    }

    private void LessonsServiceOnOnTicked(object? sender, EventArgs e)
    {
        string result = Path.GetTempPath();
        if (File.Exists(result + "\\unread"))
        {
            File.Delete(result + "\\unread");
            string res = File.ReadAllText(result + "\\res.txt", Encoding.UTF8);
            ShowNotification(new NotificationRequest()
            {
                MaskContent = NotificationContent.CreateTwoIconsMask("抽选结果"),
                OverlayContent = NotificationContent.CreateSimpleTextContent("被选中的幸运儿"+res, factory: x => {
                    // 这里的参数 x 就是创建好的提醒内容，可以在这里对提醒内容进行定制。
                    x.Duration = TimeSpan.FromSeconds(15);
                })
            });
        }
    }
}