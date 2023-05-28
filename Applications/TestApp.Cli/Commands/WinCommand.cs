using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TestApp.Cli.Commands;

class WinCommand
{
    /// <summary>
    /// _note_ (for building/compiling):
    /// 
    /// In project property, Set: 
    ///     1. "Target OS" to "Window"; 
    ///     2. "Supported OS version" to "10.0.22621.0"
    ///     
    /// See doc:
    ///     Call Windows Runtime APIs in desktop apps - Windows apps | Microsoft Learn
    ///     https://learn.microsoft.com/en-us/windows/apps/desktop/modernize/desktop-to-uwp-enhance
    /// </summary>
    public void Notify1()
    {
        Console.WriteLine("Win.Notify1 works - v5");
        string h2 = """
            test test test
              test test test
            """;

        Console.WriteLine(h2);

        string p1 = """
            p1-test p1-test p1-test
            p1-test p1-test p1-test
            """;

        DemoImpl1("test-app-id", @"D:\sync_ii\files\TP_图片\69bbca83ly1gwp68kcfrcj20hs0hsq49.jpg", "h1 text", h2, p1);
    }

    public void Notify2()
    {
        Console.WriteLine("Win.Notify1 works - v2");

        var notifier = ToastNotificationManager.CreateToastNotifier("test app2");
        var notification = new ToastNotification(DemoImpl2());
        notifier.Show(notification);
    }

    /**
     * from: https://stackoverflow.com/questions/65054564/netcore-show-windows-10-notification-toast
     */
    private void DemoImpl1(string appid, string imageFullPath, string h1, string h2, string p1)
    {

        // _note_|see doc: https://learn.microsoft.com/en-us/uwp/api/windows.ui.notifications.toasttemplatetype?view=winrt-22621
        var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

        var textNodes = template.GetElementsByTagName("text");

        textNodes[0].AppendChild(template.CreateTextNode(h1));
        textNodes[1].AppendChild(template.CreateTextNode(h2));
        textNodes[2].AppendChild(template.CreateTextNode(p1));

        if (File.Exists(imageFullPath))
        {
            XmlNodeList toastImageElements = template.GetElementsByTagName("image");
            ((XmlElement)toastImageElements[0]).SetAttribute("src", imageFullPath);
        }

        Console.WriteLine(template.GetXml().ToString());

        // _note_|see <toast> xml schema: https://learn.microsoft.com/en-us/uwp/schemas/tiles/toastschema/element-toast
        IXmlNode toastNode = template.SelectSingleNode("/toast");
        ((XmlElement)toastNode).SetAttribute("duration", "long");

        var notifier = ToastNotificationManager.CreateToastNotifier(appid);
        var notification = new ToastNotification(template);
    
        // _note_Activated-1/2: how to make the following code work?
        notification.Activated += (sender, args) =>
        {
            Console.WriteLine("notification.Activated received");
        };

        notifier.Show(notification);
        // Console.ReadKey();  // _note_Activated-2/2: for demonstrating notification.Activated above
    }

    /**
     * _note_:
     * - from: https://www.c-sharpcorner.com/article/creating-a-toast-notification-containing-image-selection-box-and-buttons/
     * - The max number of selection seems to be 5
     * - ref:
     *   See <toast> xml schema: https://learn.microsoft.com/en-us/uwp/schemas/tiles/toastschema/element-toast
     */
    private XmlDocument DemoImpl2()
    {
        string toastXml = """
            <toast scenario="alarm">  
                <visual>  
                    <binding template="ToastImageAndText02">  
                        <image id="1" src="D:\sync_ii\files\TP_图片\FZ5LjSeXoAAMLv_.jpeg"/>
                        <text id="1">Notification Message</text>  
                        <text id="2">This is a Notification Message</text>  
                    </binding>  
                </visual>  
                <actions>  
                    <input id="snoozeTime" type="selection" defaultInput="15">  
                        <selection id="1" content="1 minute"/>  
                        <selection id="15" content="15 minutes"/>  
                        <selection id="60" content="1 hour"/>  
                        <selection id="240" content="4 hours"/>  
                        <selection id="1440" content="1 day"/>  
                    </input>  
                    <action  
                     activationType="system"  
                     arguments="snooze"  
                     hint-inputId="snoozeTime"  
                     content=""/>  
                    <action  
                     activationType="system"  
                     arguments="dismiss"  
                     content=""/>  
                </actions>  
            </toast>  
            """;
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(toastXml);
        return doc;
    }
}
