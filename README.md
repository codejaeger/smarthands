# Smart-Hands
A project by EdWave - Expanding Horizons

## About
This project started as an entry to the Microsoft Imagine Cup, 2021 challenge and has been continued henceforth.

## Introduction

The project focuses on helping people to connect with the specially-abled. We will implement an integrated mobile application that combined with the hardware and available technologies can overcome the communication barrier and simplify the conversation.

## Concept

The project focuses on simplifying the communication barriers between normal people and specially-disabled deaf and blind people.

As we know, the most common way to communicate with deaf-blind people is using the Braille tactile writing system. We have built a mobile application that is able to translate text or speech to the Braille system and vice-versa.

The input for the first case is using normal text/plain file in the mobile storage or the audio recorded by the microphone. We will then use Azure Speech-to-text Cognitive Service API to recognize the speech and convert it to text. This text is then converted to Braille system and then sent to the sensors via a Bluetooth module using an Arduino. The person with whom we are trying to contact will then use the sensors implemented in ways similar to the Braille system and recognize the message.

For the reverse conversation, the input is taken using the mobile’s screen User Interface via the 6-dots system and then is converted to text or speech.

## Flow Diagram
![Alt text](diagram.png?raw=true "Architecture")

## Target Audience

Our project is aimed at the vast number of deaf-blind people/children with little or no support for education or access to technology. There is a large number of Braille readers without any way to access technology today. The only available solution is a very expensive device. Our product/device sacrifices on the refresh rate achieved by these high end devices but reduces the corresponding cost more than 50 times.

### Competition

There are already solutions out there but they are aimed for those deaf-blind people already established in their lives. The device we develop using Microsoft Azure Services aims to provide a completely different outlook on the solution to this problem.
Our device will enable teachers in the schools for deaf-blind to teach to a larger audience, lower hardware costs for these devices and a lot more.

## Status

This project is open source now and any contributions are immensely welcome.

## Instructions to use and test

Coming up.