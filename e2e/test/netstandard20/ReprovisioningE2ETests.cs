// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Provisioning.Security.Samples;
using Microsoft.Azure.Devices.Provisioning.Service;
using Microsoft.Azure.Devices.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.Azure.Devices.E2ETests.ProvisioningServiceClientE2ETests;

namespace Microsoft.Azure.Devices.E2ETests
{
    [TestClass]
    [TestCategory("Provisioning-E2E")]
    public class ReprovisioningE2ETests : IDisposable, ProvisioningE2ETests
    {
        public ReprovisioningE2ETests()
        {
            _listener = TestConfig.StartEventListener();
        }

        public enum EnrollmentType
        {
            Individual,
            Group
        }
        
        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceResetsTwin_MqttWs_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Mqtt_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceResetsTwin_Mqtt_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Mqtt_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceResetsTwin_AmqpWs_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Amqp_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceResetsTwin_Amqp_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_KeepTwin(Client.TransportType.Amqp_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceKeepsTwin_MqttWs_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Mqtt_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceKeepsTwin_Mqtt_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Mqtt_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceKeepsTwin_AmqpWs_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_KeepTwin(Client.TransportType.Amqp_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceKeepsTwin_Amqp_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_KeepTwin(Client.TransportType.Amqp_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningWorks_Http_SymmetricKey_RegisterOk_Individual()
        {
            //twin is irrelevant since HTTP Device Clients can't use twin
            await ProvisioningDeviceClient_ReprovisioningFlow_KeepTwin(Client.TransportType.Http1, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_Http_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Http1, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_Mqtt_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Mqtt_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_Amqp_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Amqp_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_AmqpWs_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Amqp_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_MqttWs_SymmetricKey_RegisterOk_Individual()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Http1, AttestationType.SymmetricKey, EnrollmentType.Individual, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceResetsTwin_MqttWs_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Mqtt_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceResetsTwin_Mqtt_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Mqtt_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceResetsTwin_AmqpWs_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Amqp_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceResetsTwin_Amqp_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_KeepTwin(Client.TransportType.Amqp_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceKeepsTwin_MqttWs_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Mqtt_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceKeepsTwin_Mqtt_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType.Mqtt_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceKeepsTwin_AmqpWs_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_KeepTwin(Client.TransportType.Amqp_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisionedDeviceKeepsTwin_Amqp_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_KeepTwin(Client.TransportType.Amqp_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningWorks_Http_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_KeepTwin(Client.TransportType.Http1, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_Http_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Http1, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_Mqtt_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Mqtt_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_Amqp_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Amqp_Tcp_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_AmqpWs_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Amqp_WebSocket_Only, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ProvisioningDeviceClient_ReprovisioningBlockingWorks_MqttWs_SymmetricKey_RegisterOk_Group()
        {
            await ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType.Http1, AttestationType.SymmetricKey, EnrollmentType.Group, false).ConfigureAwait(false);
        }

        
        /// <summary>
        /// This test flow reprovisions a device after that device created some twin updates on its original hub.
        /// The expected behaviour is that, with ReprovisionPolicy set to not migrate data, the twin updates from the original hub are not present at the new hub
        /// </summary>
        private async Task ProvisioningDeviceClient_ReprovisioningFlow_ResetTwin(Client.TransportType transportProtocol, AttestationType attestationType, EnrollmentType enrollmentType, bool setCustomProxy, string customServerProxy = null)
        {
            var connectionString = IotHubConnectionStringBuilder.Create(Configuration.IoTHub.ConnectionString);
            ICollection<string> iotHubsToStartAt = new List<string>() { Configuration.Provisioning.FarAwayIotHubHostName };
            ICollection<string> iotHubsToReprovisionTo = new List<string>() { connectionString.HostName };
            await ProvisioningDeviceClient_ReprovisioningFlow(transportProtocol, attestationType, enrollmentType, setCustomProxy, new ReprovisionPolicy { MigrateDeviceData = false, UpdateHubAssignment = true }, AllocationPolicy.Hashed, null, iotHubsToStartAt, iotHubsToReprovisionTo, customServerProxy).ConfigureAwait(false);
        }

        /// <summary>
        /// This test flow reprovisions a device after that device created some twin updates on its original hub.
        /// The expected behaviour is that, with ReprovisionPolicy set to migrate data, the twin updates from the original hub are present at the new hub
        /// </summary>
        private async Task ProvisioningDeviceClient_ReprovisioningFlow_KeepTwin(Client.TransportType transportProtocol, AttestationType attestationType, EnrollmentType enrollmentType, bool setCustomProxy, string customServerProxy = null)
        {
            var connectionString = IotHubConnectionStringBuilder.Create(Configuration.IoTHub.ConnectionString);
            ICollection<string> iotHubsToStartAt = new List<string>() { Configuration.Provisioning.FarAwayIotHubHostName };
            ICollection<string> iotHubsToReprovisionTo = new List<string>() { connectionString.HostName };
            await ProvisioningDeviceClient_ReprovisioningFlow(transportProtocol, attestationType, enrollmentType, setCustomProxy, new ReprovisionPolicy { MigrateDeviceData = true, UpdateHubAssignment = true }, AllocationPolicy.Hashed, null, iotHubsToStartAt, iotHubsToReprovisionTo, customServerProxy).ConfigureAwait(false);
        }

        /// <summary>
        /// The expected behaviour is that, with ReprovisionPolicy set to never update hub, the a device is not reprovisioned, even when other settings would suggest it should
        /// </summary>
        private async Task ProvisioningDeviceClient_ReprovisioningFlow_DoNotReprovision(Client.TransportType transportProtocol, AttestationType attestationType, EnrollmentType enrollmentType, bool setCustomProxy, string customServerProxy = null)
        {
            var connectionString = IotHubConnectionStringBuilder.Create(Configuration.IoTHub.ConnectionString);
            ICollection<string> iotHubsToStartAt = new List<string>() { Configuration.Provisioning.FarAwayIotHubHostName };
            ICollection<string> iotHubsToReprovisionTo = new List<string>() { connectionString.HostName };
            await ProvisioningDeviceClient_ReprovisioningFlow(transportProtocol, attestationType, enrollmentType, setCustomProxy, new ReprovisionPolicy { MigrateDeviceData = false, UpdateHubAssignment = false }, AllocationPolicy.Hashed, null, iotHubsToStartAt, iotHubsToReprovisionTo, customServerProxy).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Provisions a device to a starting hub, tries to open a connection, send telemetry, 
        /// and (if supported by the protocol) send a twin update. Then, this method updates the enrollment
        /// to provision the device to a different hub. Based on the provided reprovisioning settings, this 
        /// method then checks that the device was/was not reprovisioned as expected, and that the device 
        /// did/did not migrate twin data as expected.
        /// </summary>
        public async Task ProvisioningDeviceClient_ReprovisioningFlow(
            Client.TransportType transportProtocol,
            AttestationType attestationType,
            EnrollmentType? enrollmentType,
            bool setCustomProxy,
            ReprovisionPolicy reprovisionPolicy,
            AllocationPolicy allocationPolicy,
            CustomAllocationDefinition customAllocationDefinition,
            ICollection<string> iotHubsToStartAt,
            ICollection<string> iotHubsToReprovisionTo,
            string proxyServerAddress = null)
        {
            ProvisioningServiceClient provisioningServiceClient = CreateProvisioningService(ProxyServerAddress);
            string groupId = IdPrefix + AttestationTypeToString(attestationType) + "-" + Guid.NewGuid();

            bool twinOperationsAllowed = transportProtocol != Client.TransportType.Http1;

            using (ProvisioningTransportHandler transport = CreateTransportHandlerFromName(transportProtocol))
            using (SecurityProvider security = await CreateSecurityProviderFromName(attestationType, enrollmentType, groupId, reprovisionPolicy, allocationPolicy, customAllocationDefinition, iotHubsToStartAt).ConfigureAwait(false))
            {
                //Check basic provisioning
                if (ImplementsWebProxy(transportProtocol) && setCustomProxy)
                {
                    transport.Proxy = (proxyServerAddress != null) ? new WebProxy(ProxyServerAddress) : null;
                }

                ProvisioningDeviceClient provClient = ProvisioningDeviceClient.Create(
                    s_globalDeviceEndpoint,
                    Configuration.Provisioning.IdScope,
                    security,
                    transport);
                var cts = new CancellationTokenSource(PassingTimeoutMiliseconds);
                DeviceRegistrationResult result = await provClient.RegisterAsync(cts.Token).ConfigureAwait(false);
                ValidateDeviceRegistrationResult(result);
                Client.IAuthenticationMethod auth = CreateAuthenticationMethodFromSecurityProvider(security, result.DeviceId);
                await ConfirmRegisteredDeviceWorks(result, auth, transportProtocol, twinOperationsAllowed).ConfigureAwait(false);

                //Check reprovisioning
                await UpdateEnrollmentToForceReprovision(enrollmentType, provisioningServiceClient, iotHubsToReprovisionTo, security, groupId).ConfigureAwait(false);
                result = await provClient.RegisterAsync(cts.Token).ConfigureAwait(false);
                ConfirmDeviceInExpectedHub(result, reprovisionPolicy, iotHubsToStartAt, iotHubsToReprovisionTo, allocationPolicy);
                await ConfirmDeviceWorksAfterReprovisioning(result, auth, transportProtocol, reprovisionPolicy, twinOperationsAllowed).ConfigureAwait(false);

                if (attestationType != AttestationType.x509) //x509 enrollments are hardcoded, should never be deleted
                {
                    await DeleteCreatedEnrollment(enrollmentType, provisioningServiceClient, security, groupId).ConfigureAwait(false);
                }
            }
        }

        public async Task ProvisioningDeviceClient_ProvisioningFlow_CustomAllocation_AllocateToHubWithLongestHostName(
            Client.TransportType transportProtocol,
            AttestationType attestationType,
            EnrollmentType? enrollmentType,
            bool setCustomProxy,
            ICollection<string> iotHubsToProvisionTo,
            string expectedDestinationHub,
            string proxyServerAddress = null)
        {
            ProvisioningServiceClient provisioningServiceClient = CreateProvisioningService(ProxyServerAddress);
            string groupId = IdPrefix + AttestationTypeToString(attestationType) + "-" + Guid.NewGuid();

            CustomAllocationDefinition customAllocationDefinition = new CustomAllocationDefinition() { WebhookUrl = Configuration.Provisioning.CustomAllocationPolicyWebhook, ApiVersion = "2018-11-01" };

            using (ProvisioningTransportHandler transport = CreateTransportHandlerFromName(transportProtocol))
            using (SecurityProvider security = await CreateSecurityProviderFromName(attestationType, enrollmentType, groupId, null, AllocationPolicy.Custom, customAllocationDefinition, iotHubsToProvisionTo).ConfigureAwait(false))
            {
                //Check basic provisioning
                if (ImplementsWebProxy(transportProtocol) && setCustomProxy)
                {
                    transport.Proxy = (proxyServerAddress != null) ? new WebProxy(ProxyServerAddress) : null;
                }

                ProvisioningDeviceClient provClient = ProvisioningDeviceClient.Create(
                    s_globalDeviceEndpoint,
                    Configuration.Provisioning.IdScope,
                    security,
                    transport);
                var cts = new CancellationTokenSource(PassingTimeoutMiliseconds);

                DeviceRegistrationResult result = await provClient.RegisterAsync(cts.Token).ConfigureAwait(false);
                ValidateDeviceRegistrationResult(result);
                Assert.AreEqual(expectedDestinationHub, result.AssignedHub);

                if (attestationType != AttestationType.x509) //x509 enrollments are hardcoded, should never be deleted
                {
                    await DeleteCreatedEnrollment(enrollmentType, provisioningServiceClient, security, groupId).ConfigureAwait(false);
                }
            }
        }

        
        /// <summary>
        /// Update the enrollment under test such that it forces it to reprovision to the hubs within <paramref name="iotHubsToReprovisionTo"/>
        /// </summary>
        private async Task UpdateEnrollmentToForceReprovision(EnrollmentType? enrollmentType, ProvisioningServiceClient provisioningServiceClient, ICollection<String> iotHubsToReprovisionTo, SecurityProvider security, string groupId)
        {
            if (enrollmentType == EnrollmentType.Individual)
            {
                IndividualEnrollment individualEnrollment = await provisioningServiceClient.GetIndividualEnrollmentAsync(security.GetRegistrationID()).ConfigureAwait(false);
                individualEnrollment.IotHubs = iotHubsToReprovisionTo;
                IndividualEnrollment individualEnrollmentResult = await provisioningServiceClient.CreateOrUpdateIndividualEnrollmentAsync(individualEnrollment).ConfigureAwait(false);
            }
            else
            {
                EnrollmentGroup enrollmentGroup = await provisioningServiceClient.GetEnrollmentGroupAsync(groupId).ConfigureAwait(false);
                enrollmentGroup.IotHubs = iotHubsToReprovisionTo;
                EnrollmentGroup enrollmentGroupResult = await provisioningServiceClient.CreateOrUpdateEnrollmentGroupAsync(enrollmentGroup).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Confirm that the hub the device belongs to did or did not change, depending on the reprovision policy
        /// </summary>
        private void ConfirmDeviceInExpectedHub(DeviceRegistrationResult result, ReprovisionPolicy reprovisionPolicy, ICollection<string> iotHubsToStartAt, ICollection<string> iotHubsToReprovisionTo, AllocationPolicy allocationPolicy)
        {
            if (reprovisionPolicy.UpdateHubAssignment)
            {
                Assert.IsTrue(iotHubsToReprovisionTo.Contains(result.AssignedHub));
                Assert.IsFalse(iotHubsToStartAt.Contains(result.AssignedHub));

                if (allocationPolicy == AllocationPolicy.GeoLatency)
                {
                    Assert.AreNotEqual(result.AssignedHub, Configuration.Provisioning.FarAwayIotHubHostName);
                }
            }
            else
            {
                Assert.IsFalse(iotHubsToReprovisionTo.Contains(result.AssignedHub));
                Assert.IsTrue(iotHubsToStartAt.Contains(result.AssignedHub));
            }
        }

        private async Task ConfirmDeviceWorksAfterReprovisioning(DeviceRegistrationResult result, Client.IAuthenticationMethod auth, Client.TransportType transportProtocol, ReprovisionPolicy reprovisionPolicy, bool twinOperationsAllowed)
        {
            using (DeviceClient iotClient = DeviceClient.Create(result.AssignedHub, auth, transportProtocol))
            {
                _log.WriteLine("DeviceClient OpenAsync.");
                await iotClient.OpenAsync().ConfigureAwait(false);
                _log.WriteLine("DeviceClient SendEventAsync.");
                await iotClient.SendEventAsync(
                    new Client.Message(Encoding.UTF8.GetBytes("TestMessage"))).ConfigureAwait(false);

                //twin can be configured to revert back to default twin when provisioned, or to keep twin
                // from previous hub's records.
                if (twinOperationsAllowed)
                {
                    Twin twin = await iotClient.GetTwinAsync().ConfigureAwait(false);

                    if (reprovisionPolicy.MigrateDeviceData)
                    {
                        Assert.AreNotEqual(1, twin.Properties.Desired.Count);
                        Assert.AreNotEqual(0, twin.Properties.Desired.Version);
                        Assert.AreEqual(ProvisioningRegistrationSubstatusType.DeviceDataMigrated, result.Substatus);
                    }
                    else if (reprovisionPolicy.UpdateHubAssignment)
                    {
                        Assert.AreEqual(twin.Properties.Desired.Count, 0);
                        Assert.AreEqual(ProvisioningRegistrationSubstatusType.DeviceDataReset, result.Substatus);
                    }
                    else
                    {
                        Assert.AreNotEqual(twin.Properties.Desired.Count, 1);
                        Assert.AreEqual(ProvisioningRegistrationSubstatusType.InitialAssignment, result.Substatus);
                    }
                }

                _log.WriteLine("DeviceClient CloseAsync.");
                await iotClient.CloseAsync().ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
