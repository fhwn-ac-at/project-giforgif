[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/vtde1l2O)

# Perropoly

Welcome to **Perropoly**, an exciting take on the classic Monopoly game with unique features, crafted by **Tuka**, **Prix**, and **Koch**. This project is powered by **SignalR** and **C#** on the backend, and **Angular** on the frontend, ensuring a smooth and interactive multiplayer experience.

## Table of Contents
- [Game Overview](#game-overview)
- [The God-System](#the-god-system)
- [Game Rules](#game-rules)
- [Application Architecture](#application-architecture)
- [Deployment](#deployment)
- [Links](#links)

## Game Overview
Perropoly is a modern reimagining of Monopoly, featuring:
- Real-time gameplay using SignalR.
- Backend logic implemented in C#.
- A sleek Angular-based frontend.
- A unique **God-System** that introduces random events during gameplay.

## The God-System
Perropoly introduces a new dimension to the classic game with the **God-System**, a special mechanic that has a 10% chance of activating each turn. When triggered, one of the following events occurs:

1. **Sigmar:** Gives every player $200.
2. **Khorne:** Moves every player to "Go".
3. **Tzeentch:** Awards a random unoccupied property to a random player.
4. **Slaanesh :** Doubles the buying price of all properties.
5. **Nurgle:** Doubles the rent on all properties.

These events add an element of unpredictability, keeping every game fresh and exciting!

## Game Rules
Perropoly follows the traditional Monopoly rules with a few additions:

1. Players take turns rolling dice to move around the board.
2. Properties can be purchased or auctioned when landed upon.
3. Landing on another player's property incurs rent payment.
4. Players can build houses and hotels to increase rent value.
5. The game ends when all but one player is bankrupt or a predefined time limit is reached.
6. **God-System events** can occur at any time, altering strategies and gameplay dynamics.

## Application Architecture
The Perropoly ecosystem comprises four key components:

### 1. Game Application
- **URL:** [Perropoly Game](https://perropoly.trucklix.at)
- Built with Angular and SignalR for seamless real-time interaction.

### 2. Stats Dashboard
- **URL:** [Game Stats](https://stats.trucklix.at)
- Tracks player stats and game history.
- Backend: C# with a REST API.
- Frontend: Angular.

### 3. API Services
- **URL:** [Game API](https://api.trucklix.at)
- Provides endpoints for the game logic and stats functionality.
- REST API powered by C#.

### 4. Deployment Dashboard
- **URL:** [Coolify Dashboard](https://coolify.trucklix.at)
- Self-hosted Coolify setup for streamlined deployments.
- Hosted on a **basic Hetzner server** for cost-effective and reliable performance.

## Deployment
The project leverages a **Hetzner server** for hosting and **Coolify (self-hosted)** for deployment automation.

## Links
Here are the links to the different parts of the Perropoly ecosystem:

- [Perropoly Game](https://perropoly.trucklix.at)
- [Game Stats](https://stats.trucklix.at)
- [Game API](https://api.trucklix.at)
- [Coolify Deployment Dashboard](https://coolify.trucklix.at)
