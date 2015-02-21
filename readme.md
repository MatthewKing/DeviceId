DeviceId
========

A simple library providing functionality to generate a 'device ID' that can be used to uniquely identify a computer.

Simple usage
============

Use the `DeviceIdBuilder` class to build up a device ID.

    string deviceId = new DeviceIdBuilder()
        .AddMachineName()
        .AddMacAddress()
        .AddProcessorId()
        .AddMotherboardSerialNumber()
        .GetDeviceId();

License and copyright
---------------------
Copyright Matthew King 2015.
Distributed under the [MIT License](http://opensource.org/licenses/MIT). Refer to license.txt for more information.
