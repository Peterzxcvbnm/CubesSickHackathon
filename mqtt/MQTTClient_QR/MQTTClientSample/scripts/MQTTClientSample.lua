--[[----------------------------------------------------------------------------

  Application Name:
  MQTTClientSample

  Summary:
  Connecting an MQTT client to an MQTT broker and subscribing and publishing to topics.

  Description:
  This sample shows how to connect to a mqtt broker, register a topic and publish
  data on this topic. It also shows how to subscribe to a topic and receive data.
  The client subscribes to topic "test/topic1". Any messages that are received under
  this topic are then published to topic "test/topic2".

  How to run:
  For this sample to run a mqtt server / broker has to be set up.
  The sample can be run on the Emulator with a FullFeatured Device Model or any
  device which has the MQTTClient functionality and can connect to the mqtt broker.

  Implementation:
  TLS can be used by setting USE_TLS to true.
  Important when using TLS:
  - Certificate verification will fail if the current time is not set on the device.
    Time can be set using DateTime.setDateTime or NTPClient API.
  - Resolving of hostnames only works when a DNS name server is configured for the device.
    This can be done using DHCP.

  More Information:
  This sample app includes a self-signed server certificate / private key pair, as well as
  a client certificate / private key pair.
  It also contains sample configuration file for two different brokers:
  - HiveMQ (tested with version 3.2.5)
  - mosquitto (tested with version 1.4.14)

  There is an additional mosquitto/readme.txt that describes how to start mosquitto with the
  provided config and how to use the mosquitto client tools to test this sample.

------------------------------------------------------------------------------]]
--Start of Global Scope---------------------------------------------------------

-- Create client
local client = MQTTClient.create()

-- IP address of broker, must be adapted to match actual address of the used broker
local BROKER_IP = '10.151.8.213'

local USE_TLS = false

-- Configure connection parameters
client:setIPAddress(BROKER_IP)
if (USE_TLS) then
  client:setPort(8883)
  client:setTLSEnabled(true)
  client:setTLSVersion('TLS_V12') -- Use TLS protocol version 1.2
  client:setCABundle('resources/app/mybroker-cert.pem')
  client:setClientCertificate(
    'resources/app/mqtt-client-cert-2.pem',
    'resources/app/mqtt-client-key-2.pem',
    'changemeclient'
  )
end

-- Setup a timer to manually reconnect in case connection gets lost
local tmr = Timer.create()
tmr:setPeriodic(true)
tmr:setExpirationTime(5000)

--End of Global Scope-----------------------------------------------------------

--Start of Function and Event Scope---------------------------------------------

local function handleOnStarted()
  -- Try to open connection.
  client:connect()
  -- Starting timer for reconnection
  tmr:start()
end
Script.register('Engine.OnStarted', handleOnStarted)

-- Function is called every time a connection is established.
-- Subscribing to topics is done here.
local function handleOnConnected()
  print('handleOnConnected')
  client:subscribe('test/topic1')
end
client:register('OnConnected', handleOnConnected)

-- Function is called on disconnect event
local function handleOnDisconnected()
  print('handleOnDisconnected')
end
client:register('OnDisconnected', handleOnDisconnected)

-- Function is called when a message is published to a subscribed topic
local function handleOnReceive(topic, data, _, _)
  print("handleOnReceive: topic '" .. topic .. "' message '" .. data .. "'")
  -- This sample publishes the received messages to test/topic2
  client:publish( 'test/topic2', "MQTTClient API works. Received '" .. data .. "'" )
end
client:register('OnReceive', handleOnReceive)

-- Function is called periodically on timer expiration
local function handleReconnectTimer()
  if (not client:isConnected()) then
    print('try to reconnect')
    client:connect()
  end
end
tmr:register('OnExpired', handleReconnectTimer)

--End of Function and Event Scope------------------------------------------------
