using System.Management;

class Program
{
    static int Main(string[] args)
    {
        var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 3");

        // create event watcher object
        using var watcher = new ManagementEventWatcher(query);

        // register delegate method
        watcher.EventArrived += new EventArrivedEventHandler(HandleUSBEvent);
        watcher.Start();

        Console.WriteLine("Enter to exit");
        Console.ReadLine();

        watcher.Stop();

        return 0;
    }

    // delegate method
    private static void HandleUSBEvent(object _, EventArrivedEventArgs e)
    {
        /*
        Configuration Changed (1)
        Device Arrival(2)
        Device Removal(3)
        Docking(4)
        */
        ushort ev = (ushort)e.NewEvent.Properties["EventType"].Value;

        if (ev == 2)
        {
            Console.WriteLine("[+] USB device connected");
        }
        else if (ev == 3)
        {
            Console.WriteLine("[-] USB device disconnected");
        }
    }
}
