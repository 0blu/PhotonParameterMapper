# Photon Parameter Mapper
The probably fastest way to use custom contracts in [Photon](https://www.photonengine.com/en-US/Photon)

![img](https://i.imgur.com/uHjriaS.png)

## Features
 - Dynamicly build a mapper
 - Save a mapper as an assembly (Good for productive use)
 - Custom field types
 - Custom indices
 - Skips default(T) values

## Projects
 - PhotonParameterMapper.Core

   Is required to pre build and dynamiclly build assemblies
   
 - PhotonParameterMapper.Builder

   To build, run and save dynamic assemblies
   
 - PhotonParameterMapper.Example

   Contains a bunch of example use cases and speed tests

## What does it do?
The _Mapper_ converts custom contracts to `Dictionary<byte, object>`.

Made for [Photon's](https://www.photonengine.com/en-US/Photon) `{OperationResponse, OperationRequest, EventData}.Parameters` 
