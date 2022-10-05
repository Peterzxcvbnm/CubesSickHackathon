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
-- Create TCP ip client for QR Camera
-- luacheck: globals gClient
gClient = TCPIPClient.create()
TCPIPClient.setIPAddress(gClient, '192.168.0.10')
TCPIPClient.setPort(gClient, 2112)
TCPIPClient.setFraming(gClient, '\02', '\03', '\02', '\03') -- STX/ETX framing for transmit and receive
TCPIPClient.register(gClient, 'OnReceive', 'gHandleReceive')
TCPIPClient.connect(gClient)
-- Create MQTT client
local client = MQTTClient.create()

-- IP address of broker, must be adapted to match actual address of the used broker
local BROKER_IP = '192.168.0.69'

local USE_TLS = false

-- Configure connection parameters
client:setIPAddress(BROKER_IP)
client:setPort(1883)
if (USE_TLS) then
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

-- Setup a timer to send the QR Code to MQTT
local tmr1 = Timer.create()
tmr1:setPeriodic(true)
tmr1:setExpirationTime(1000)
--End of Global Scope-----------------------------------------------------------

--Start of Function and Event Scope---------------------------------------------
-- Main Loop
local function handleOnStarted()
  -- Try to open connection.
  client:connect()
  -- Starting timer for reconnection
  tmr:start()
  tmr1:start()
  print("handleOnStarted")
end
Script.register('Engine.OnStarted', handleOnStarted)

-- Function is called every time a connection is established.
-- Subscribing to topics is done here.
local function handleOnConnected()
  print('handleOnConnected')
end
client:register('OnConnected', handleOnConnected)

-- Function is called on disconnect event
local function handleOnDisconnected()
  print('handleOnDisconnected')
end
client:register('OnDisconnected', handleOnDisconnected)

-- Function is called periodically on timer expiration
local function handleReconnectTimer()
  if (not client:isConnected()) then
    print('try to reconnect')
    client:connect()
  end
end

local function SendQR()
  -- Configure QR Camera --
  print("Trigger sent")
  gClient:transmit("sMN mTCgateon")
  Script.sleep(500)
  gClient:transmit("sMN mTCgateoff")

  function gHandleReceive(data)
    if data == "NoRead" then
      print("#####")
      print(data)
      print("#####")
    elseif data=="sAN mTCgateon 1" or data=="sAN mTCgateoff 1" then
      do end
    else
      client:publish( '/card_scanner/1/current_employee',data)
      print("$$$$")
      print(data)
      print("$$$$")
    end
  end
end

tmr:register('OnExpired', handleReconnectTimer)
tmr1:register('OnExpired', SendQR)

--End of Function and Event Scope------------------------------------------------
