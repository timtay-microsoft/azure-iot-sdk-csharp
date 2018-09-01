using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceExplorer
{
    public partial class CLIForm : Form
    {
        string activeDeviceId;
        string activeHubConnectionString;
        public CLIForm(string HubConnectionsTring, string DeviceId)
        {
            InitializeComponent();
            this.activeDeviceId = DeviceId;
            this.activeHubConnectionString = HubConnectionsTring;
            //richTextBox1.Text = ">" + DeviceId + "\n" + HubConnectionsTring;

            //MonitorEventHubAsync(DateTime.Now, "$Default");
        }

        private async void MonitorEventHubAsync(DateTime startTime, string consumerGroupName)
        {
            EventHubClient eventHubClient = null;
            EventHubReceiver eventHubReceiver = null;

            try
            {
                string selectedDevice = this.activeDeviceId;
                eventHubClient = EventHubClient.CreateFromConnectionString(this.activeHubConnectionString, "messages/events");
                richTextBox1.Text = "Receiving events...\n";
                var eventHubPartitionsCount = eventHubClient.GetRuntimeInformation().PartitionCount;
                string partition = EventHubPartitionKeyResolver.ResolveToPartition(selectedDevice, eventHubPartitionsCount);
                eventHubReceiver = eventHubClient.GetConsumerGroup(consumerGroupName).CreateReceiver(partition, startTime);

                //receive the events from startTime until current time in a single call and process them
                var events = await eventHubReceiver.ReceiveAsync(int.MaxValue, TimeSpan.FromSeconds(20));

                foreach (var eventData in events)
                {
                    var data = Encoding.UTF8.GetString(eventData.GetBytes());
                    var enqueuedTime = eventData.EnqueuedTimeUtc.ToLocalTime();
                    var connectionDeviceId = eventData.SystemProperties["iothub-connection-device-id"].ToString();

                    if (string.CompareOrdinal(selectedDevice.ToUpper(), connectionDeviceId.ToUpper()) == 0)
                    {
                        richTextBox1.Text += $"{enqueuedTime}> Device: [{connectionDeviceId}], Data:[{data}]";

                        if (eventData.Properties.Count > 0)
                        {
                            richTextBox1.Text += "Properties:\r\n";
                            foreach (var property in eventData.Properties)
                            {
                                richTextBox1.Text += $"'{property.Key}': '{property.Value}'\r\n";
                            }
                        }
                        //if (enableSystemProperties.Checked)
                        {
                            if (eventData.Properties.Count == 0)
                            {
                                richTextBox1.Text += "\r\n";
                            }
                            foreach (var item in eventData.SystemProperties)
                            {
                                richTextBox1.Text += $"SYSTEM>{item.Key}={item.Value}\r\n";
                            }
                        }
                        richTextBox1.Text += "\r\n";

                        // scroll text box to last line by moving caret to the end of the text
                        richTextBox1.SelectionStart = richTextBox1.Text.Length - 1;
                        richTextBox1.SelectionLength = 0;
                        richTextBox1.ScrollToCaret();
                    }
                }

                //Func<bool> processor = new Func<bool>(() => ProcessOutput(eventHubReceiver, selectedDevice, ct));
                //await Task.Run(processor, ct);

            }
            catch (Exception ex)
            {
                if (eventHubReceiver != null)
                {
                    eventHubReceiver.Close();
                }
                if (eventHubClient != null)
                {
                    eventHubClient.Close();
                }
            }
        }

        private async void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string cloudToDeviceMessage = richTextBox1.Text;
                ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(this.activeHubConnectionString);

                var serviceMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(cloudToDeviceMessage));
                serviceMessage.Ack = DeliveryAcknowledgement.Full;
                serviceMessage.MessageId = Guid.NewGuid().ToString();


                await serviceClient.SendAsync(this.activeDeviceId, serviceMessage);
                richTextBox1.Clear();

                await serviceClient.CloseAsync();

            }
        }
    }
}
